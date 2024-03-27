using LinearAlgebra;
using Quantum;
using System.Diagnostics;
using System.Numerics;

namespace Console_Testing
{
    // this class is meant to contain tests that will run in main when the tests are too complicated to be made into a test case
    // or require some amount of interaction. they should be written as a method then added into main
    internal class Showcases
    {

        // private method used to generate random matrices of m x n size for use in the showcases
        private static Matrix GenerateRandomMatrix(int m, int n)
        {
            Random rand = new Random();

            Complex[,] array = new Complex[m, n];
            for (int r = 0; r < m; r++)
            {
                for (int j = 0; j < n; j++)
                {
                    array[r, j] = new Complex(rand.NextDouble(), rand.Next(10));
                }
            }

            // populate matices
            return new Matrix(array);
        }


        // simple showcase that performs a vector matrix multiplication 
        public static void Vector_times_matrix()
        {
            // simple vector matrix multiplication
            Matrix vector = new Matrix(new Complex[,] { { 1 }, { 2 }, { 3 } });
            Console.WriteLine(vector);
            Matrix matrix = new Matrix(new Complex[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Console.WriteLine(matrix);
            Matrix result = Operations.Multiply(matrix, vector);
            Console.WriteLine(result);

            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6, 7 }, { 8, 9, 10 } });

            // Act & Assert
            Console.WriteLine(Operations.MatrixMultiply(matrix1, matrix2));


        }


        // showcases the difference in performance between the naive matrix multipication and a multithreaded variation 
        public static void MultThreadedMult()
        {
            int n = 125;
            int m = 1000;
            int k = 350;

            Console.WriteLine(new string('-', 20));
            Console.WriteLine(new string('-', 20));

            Console.WriteLine("Beginning MatMul.");

            Stopwatch watch1 = new Stopwatch();

            // populate matrices
            Matrix A = GenerateRandomMatrix(n, m);
            Matrix B = GenerateRandomMatrix(m, k);

            watch1 = new Stopwatch();
            watch1.Start();
            Matrix C1 = Operations.MatrixMultiply(A, B);
            watch1.Stop();
            Console.WriteLine("Threaded MatMul time : " + watch1.ElapsedMilliseconds);

            watch1 = new Stopwatch();
            watch1.Start();
            Matrix C2 = Operations.Multiply(A, B);
            watch1.Stop();
            Console.WriteLine("Elementary MatMul time : " + watch1.ElapsedMilliseconds);

            Console.WriteLine("PASSED: " + (Operations.IsEqual(C1, C2)));
        }


        // tests the time needed to perform a tensor product on given sized matrices
        public static void TimeTestTensor(int m, int n, int k, int p)
        {


            // fill matrix arrays with random values 
            Random rand = new Random();

            // populate matrices
            Matrix A = GenerateRandomMatrix(n, m);
            Matrix B = GenerateRandomMatrix(m, k);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Matrix C = Operations.TensorProduct(A, B);
            stopwatch.Stop();
            TimeSpan time = stopwatch.Elapsed;
            //MatrixOperations.printMatrix(tp);
            Console.WriteLine("Matrices of " + m + "x" + n + " and " + k + "x" + p + "matrices");
            // add something that displays the dim of the resulting matrix
            Console.WriteLine($"Matrix Tensor Product: {time.TotalMilliseconds} milliseconds");
        }

        public static void QbitEvolutionDemo()
        {
            Complex[,] x = { { 0, 1 }, { 1, 0 } };
            Complex[,] y = { { 0, -1*Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } };
            Complex[,] z = { { 1, 0 }, { 0, -1 } };
            Complex[,] h = { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) }, { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } };
            Complex[,] s = { { 1, 0 }, { 0, Complex.ImaginaryOne } };
            Complex[,] t = { { 1, 0 }, { 0, (1 + Complex.ImaginaryOne) / Complex.Sqrt(2) } };

            Operator X = new Operator(x);
            Operator Y = new Operator(y);
            Operator Z = new Operator(z);
            Operator H = new Operator(h);
            Operator S = new Operator(s);
            Operator T = new Operator(t);


            Console.WriteLine("X Operator:\n" + X.ToString() + "\n");

            Complex[] state = { 1, 0 };
            Qbit q = new Qbit(state);

            Console.WriteLine("Initial State Vector:\n" + q.ToString() + "\n");

            q.Evolve(H);
            q.Evolve(T);
            q.Evolve(H);
            q.Evolve(T);
            q.Evolve(Z);

            Console.WriteLine("State Vector after evolution:\n" + q.ToString() + "\n");

        }

    }
}
