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
        // Number of rows
        private int Row { get; set; }

        // Number of rows
        private int Column { get; set; }

        // Array of doubles to store Matrix
        double[,] arr;

        // Static lock object for used for multithreading
        private static Object _lock = new Object();

        // Constructor for Matrix
        public Matrix(int row, int column)
        {
            Row = row;
            Column = column;
            arr = new double[row, column];
        }

        // Get a single row of a matrix
        public double[] GetRow(int rowNumber)
        {
            return Enumerable.Range(0, Row)
                .Select(x => arr[rowNumber, x])
                .ToArray();
        }

        // Get a single column of a matrix
        public double[] GetColumn(int columnNumber)
        {
            return Enumerable.Range(0, Column)
                .Select(x => arr[x, columnNumber])
                .ToArray();
        }

        // Fill a matrix with random double
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

        /*
         * This method performs a fast multithreaded matrix multiplication.
         * It utilizes the a thread pool to allow all of the threads to run concurrently
         * without getting into a deadlock
         */
        public static Matrix MatrixMultiply(Matrix a, Matrix b)
        {
            // Get dimensions of the matrices
            int m = a.Row;
            int n = a.Column;
            int p = b.Column;

            // Create a matrix to return
            Matrix result = new Matrix(m,p);

            
            /*
             * Set the number of threads based on system requirements
             * We want enough threads to run the operation quickly, but not so many 
             * that it drastically starves the CPU
             */
            int numThreads = Environment.ProcessorCount; 
            ManualResetEvent[] doneEvents = new ManualResetEvent[numThreads]; // Create a list of reset events to wait on threads

            // Calculate the number of rows handled by a single thread
            int rowsPerThread = m / numThreads;
            Console.WriteLine("Num Threads: " + numThreads);
            Console.WriteLine("Rows per thread: " + rowsPerThread);

            // Spawn threads
            for (int i = 0; i < numThreads; i++)
            {
                // Calculate which range ith thread is operating on
                int startRow = i * rowsPerThread;
                int endRow = (i == numThreads - 1) ? m : (i + 1) * rowsPerThread;

                // Set the event as not done
                doneEvents[i] = new ManualResetEvent(false);

                // Add a delegate to the thread pool
                ThreadPool.QueueUserWorkItem(delegate (object state)
                {
                    // Perform a matrix multiplication
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
                    ((ManualResetEvent)state).Set(); // Set thread as up when operation is done
                }, doneEvents[i]);
            }
            
            // Wait for all threads to finish and return
            WaitHandle.WaitAll(doneEvents);
            return result;
        }

        // Performs a naive n^3 matrix multiplication
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

        // Checks equality of two matrices
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

        // Checks inequality of two matrices
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

        // Print a matrix
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
