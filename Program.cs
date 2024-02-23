using System;
using System.Diagnostics;
using Accord.Math;
/*
 * Matrix Operations tester
 * 
 * @Author Jett Bauman
 * 
 * 2-10-24
 * 
 * Description: This is a first pass implementation of a custom matrix library for our Quantum Computing Tool for education.
 * The purpose is to create only the needed features as to avoid bloat from larger libraries. MatrixOperations is the 
 * object in which to access multiplication, Strass multiplication, and tensor products. More to come. 
 * 
 */
namespace MatrixOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            timeTestTensor(4, 1, 6, 2);
            timeTestTensor(14, 12, 6, 24);
            timeTestTensor(44, 41, 96, 82);
            timeTestTensor(106, 12, 90, 16);
            // Issues arise here. Need new implementation of tensor product/way to store large array.
            // MemoryPool seems promising.
            timeTestTensor(400, 100, 600, 200);
        }

        static void timeTestingMult()
        {
            //MultiplicationTesting();
            timeTestComparisonMatrixMult(8192, 8192, 8192);

            timeTestComparisonMatrixMult(4096, 4096, 4096);
            timeTestComparisonMatrixMult(2048, 2048, 2048);
            timeTestComparisonMatrixMult(4, 4, 4);
            timeTestComparisonMatrixMult(8, 8, 8);

            timeTestComparisonMatrixMult(7, 2, 5);

            timeTestComparisonMatrixMult(12, 12, 12);

            timeTestComparisonMatrixMult(32, 32, 32);

            timeTestComparisonMatrixMult(512, 512, 512);

            timeTestComparisonMatrixMult(500, 515, 510);
        }

        static void timeTestTensor(int m, int n, int k, int p)
        {
            Stopwatch stopwatch = new Stopwatch();
            int[,] a = MatrixOperations.generateMatrix(m, n, 1, 10);
            int[,] b = MatrixOperations.generateMatrix(k, p, 1, 10);
            stopwatch.Start();
            int[,] tp = MatrixOperations.tensorProduct(a, b);
            stopwatch.Stop();
            TimeSpan time = stopwatch.Elapsed;
            //MatrixOperations.printMatrix(tp);
            Console.WriteLine("Matrices of " + m + "x" + n + " and " + k + "x" + p + "matrices");
            Console.WriteLine($"Matrix Tensor Product: {time.TotalMilliseconds} milliseconds");
        }
        static void timeTestComparisonMatrixMult(int m, int n, int k)
        {
            // Define matrix dimensions
            //int m = 1026;
            //int n = 1024;
            //int k = 1022;

            // Generate two matrices
            int[,] a = MatrixOperations.generateMatrix(m, n, 1, 10);
            int[,] b = MatrixOperations.generateMatrix(n, k, 1, 10);

            // Time and compare speed of multiplication algorithms
            Stopwatch stopwatch = new Stopwatch();

            // Time and execute MatrixOperations.Multiply(a, b)
            stopwatch.Start();
            int[,] resultMultiply = MatrixOperations.strassMultiply(a, b);
            stopwatch.Stop();
            TimeSpan timeMultiply = stopwatch.Elapsed;
            //Console.WriteLine("Strassen Matrix");
            //MatrixOperations.printMatrix(resultMultiply);

            // Reset stopwatch
            stopwatch.Reset();

            // Time and execute MatrixOperations.matrixMult(a, b)
            stopwatch.Start();
            int[,] resultMatrixMult = MatrixOperations.matrixMult(a, b);
            stopwatch.Stop();
            TimeSpan timeMatrixMult = stopwatch.Elapsed;
            //Console.WriteLine("MatrixMult Matrix");
            //MatrixOperations.printMatrix(resultMatrixMult);
            // Check if results are the same
            bool resultsMatch = resultMultiply == resultMatrixMult;

            // Print results
            Console.WriteLine("Matrix A m x n " + m + " " + n);
            Console.WriteLine("Matrix A n x k " + n + " " + k);
            Console.WriteLine($"Matrix multiplication using Strassen took: {timeMultiply.TotalMilliseconds} milliseconds");
            Console.WriteLine($"Matrix multiplication using Elementary took: {timeMatrixMult.TotalMilliseconds} milliseconds");
            double speedDiff = timeMultiply.TotalMilliseconds / timeMatrixMult.TotalMilliseconds;
            Console.WriteLine("Elementary multiplication is faster by " + speedDiff.ToString() + " times Strauss Algorithm.");
            Console.WriteLine("--------------");
            Console.WriteLine();

            //if (resultsMatch)
                //Console.WriteLine("Results match.");
            //else
                //Console.WriteLine("Results do not match.");
        

    }
    static void MultiplicationTesting()
        {
            int[,] a = { { 3, 4, 2 } };
            int[,] b = { { 13, 9, 7, 15 }, { 8, 7, 4, 6 }, { 6, 4, 0, 3 } };
            int[,] product = MatrixOperations.strassMultiply(a, b);
            MatrixOperations.printMatrix(product);
            Console.WriteLine();

            int[,] c = {
                {1, 2, 3},
                {4, 5, 6}
            };
            int[,] d = {
                {7, 8},
                {9, 10},
                {11, 12}
            };
            MatrixOperations.printMatrix(MatrixOperations.tensorProduct(c, d));
            int[,] e = {
                {1, 2, 3},
                {4, 5, 6}
            };
            int[,] f = {
                {7, 8},
                {9, 10},
                {11, 12}
            };
            int[,] result = MatrixOperations.strassMultiply(c, d);
            MatrixOperations.printMatrix(result);
            Console.WriteLine();

            int[,] x = {
                {1, 2, 3, 4, 5, 6},
                {7, 8, 9, 10, 11, 12},
                {13, 14, 15, 16, 17, 18},
                {19, 20, 21, 22, 23, 24},
                {25, 26, 27, 28, 29, 30},
                {31, 32, 33, 34, 35, 36}
            };

            int[,] y = {
                {37, 38, 39},
                {40, 41, 42},
                {43, 44, 45},
                {46, 47, 48},
                {49, 50, 51},
                {52, 53, 54}
            };

            int[,] xy = MatrixOperations.strassMultiply(x, y);
            MatrixOperations.printMatrix(xy);
            Console.WriteLine();

            MatrixOperations.printMatrix(MatrixOperations.generateMatrix(500, 20, 1, 15));
        }
    }
}
