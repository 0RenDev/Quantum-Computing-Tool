using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadedMatmul
{

    class Matrix
    {
        private int Row { get; set; }
        private int Column { get; set; }
        double[,] arr;

        private static Object _lock = new Object();

        public Matrix(int row, int column)
        {
            Row = row;
            Column = column;
            arr = new double[row, column];
        }

        public double[] GetRow(int rowNumber)
        {
            return Enumerable.Range(0, Row)
                .Select(x => arr[rowNumber, x])
                .ToArray();
        }

        public double[] GetColumn(int columnNumber)
        {
            return Enumerable.Range(0, Column)
                .Select(x => arr[x, columnNumber])
                .ToArray();
        }

        public void RandomValues()
        {
            Random rand = new Random();
            for(int i = 0; i < Row; i++)
            {
                for(int j = 0; j < Column; j++)
                {
                    arr[i, j] = rand.NextDouble();
                }
            }
        }

        public static void VectorMul(int tmp,  Matrix a, Matrix b, Matrix r)
        {
            int i = tmp / b.Column;
            int j = tmp % b.Column;

            double[] x = a.GetRow(i);
            double[] y = b.GetColumn(j);

            double sum = 0;
            for (int k = 0; k < x.Length; k++)
            {
                sum += x[k] * y[k];
            }

            lock(_lock)
            {
                r.arr[i, j] = sum;
            }


            Console.WriteLine("Calculating [" + i + ", " + j + "]");
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix result = new Matrix(a.Row, a.Column);
            List<Thread> threads = new List<Thread>();

            for(int i = 0; i < a.Row * b.Column; i++)
            {
                int temp = i;
                Thread thread = new Thread(() => VectorMul(temp, a, b, result));
                thread.Start();
                threads.Add(thread);
            }

            // foreach(Thread thread in threads) { thread.Join(); }

            return result;
        }

        public static Matrix MatrixMultiply(Matrix a, Matrix b)
        {
            int m = a.Row;
            int n = a.Column;
            int p = b.Column;

            Matrix result = new Matrix(m,p);

            // May use Environment.ProcessorCount
            int numThreads = Environment.ProcessorCount; // Or set it according to your system or requirement
            ManualResetEvent[] doneEvents = new ManualResetEvent[numThreads];
            int rowsPerThread = m / numThreads;
            Console.WriteLine("Num Threads: " + numThreads);
            Console.WriteLine("Rows per thread: " + rowsPerThread);

            for (int i = 0; i < numThreads; i++)
            {
                int startRow = i * rowsPerThread;
                int endRow = (i == numThreads - 1) ? m : (i + 1) * rowsPerThread;
                doneEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(delegate (object state)
                {
                    for (int row = startRow; row < endRow; row++)
                    {
                        for (int col = 0; col < p; col++)
                        {
                            double sum = 0;
                            for (int k = 0; k < n; k++)
                            {
                                sum += a.arr[row, k] * b.arr[k, col];
                            }
                            result.arr[row, col] = sum;
                        }
                    }
                    ((ManualResetEvent)state).Set();
                }, doneEvents[i]);
            }

            WaitHandle.WaitAll(doneEvents);
            return result;
        }

        public static Matrix ElemMatMul(Matrix a, Matrix b)
        {

            Matrix result = new Matrix(a.Row, a.Column);
            for(int i = 0; i<a.Row; i++)
            {
                for(int j = 0; j < b.Column; j++)
                {
                    for(int k =0; k < b.Row; k++)
                    {
                        result.arr[i, j] += a.arr[i, k] * b.arr[k, j];
                    }
                }
            }

            return result;
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.Row != b.Row || a.Column != b.Column) return false;

            int rows = a.Row;
            int cols = b.Column;
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    if (a.arr[i, j] !=  b.arr[i, j]) return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            if (a.Row != b.Row || a.Column != b.Column) return true;

            int rows = a.Row;
            int cols = b.Column;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (a.arr[i, j] != b.arr[i, j]) return true;
                }
            }

            return false;
        }

        public void Print()
        {
            Console.WriteLine("Matrix: " + Row + "x"+ Column);
            for(int i = 0; i < Row; i++)
            {
                Console.Write("[");
                for(int j = 0; j < Column - 1; j++)
                {
                    Console.Write(arr[i, j] + ", ");
                }
                Console.Write(arr[i, Column - 1] + "]\n");
            }
        }
    }

    public class Class1
    {

        public static void Main(string[] args)
        {
            int n = 1000;
            int m = 1000;
            int k = 1000;

            Matrix A = new Matrix(n, m);
            Matrix B = new Matrix(m, k);

            A.RandomValues();
            B.RandomValues();

            Console.WriteLine(new string('-', 20));
            Console.WriteLine(new string('-', 20));

            Console.WriteLine("Beginning MatMul.");

            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            Matrix C1 = Matrix.MatrixMultiply(A, B);
            watch1.Stop();
            Console.WriteLine("Threaded MatMul Complete: " + watch1.ElapsedMilliseconds);

            watch1.Start();
            Matrix C2 = Matrix.ElemMatMul(A, B);
            watch1.Stop();
            Console.WriteLine("Elementary MatMul Complete: " + watch1.ElapsedMilliseconds);

            Console.WriteLine("PASSED: " + (C1 == C2));

            for (int i = 0; i < 10; i++)
            {
                A.RandomValues();
                B.RandomValues();

                watch1 = new Stopwatch();
                watch1.Start();
                C1 = Matrix.MatrixMultiply(A, B);
                watch1.Stop();
                Console.WriteLine("Threaded MatMul time: " + watch1.ElapsedMilliseconds);

                watch1 = new Stopwatch();
                watch1.Start();
                C2 = Matrix.ElemMatMul(A, B);
                watch1.Stop();
                Console.WriteLine("Elementary MatMul time: " + watch1.ElapsedMilliseconds);

                Console.WriteLine("PASSED: " + (C1 == C2));
            }

            Console.ReadKey();
        }

        

    }
}
