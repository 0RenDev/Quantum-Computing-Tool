using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Console_Testing
{
    // this class is meant to contain tests that will run in main when the tests are too complicated to be made into a test case
    // or require some amount of interaction. they should be written as a method then added into main
    internal class Showcases
    {
        public void vector_times_matrix()
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


        public void mmultThreadedMult()
        {
            int n = 125;
            int m = 1000;
            int k = 350;

            Console.WriteLine(new string('-', 20));
            Console.WriteLine(new string('-', 20));

            Console.WriteLine("Beginning MatMul.");

            Stopwatch watch1 = new Stopwatch();

            for (int i = 1; i < 5; i++)
            {
                // fill matrix arrays with random values 
                Random rand = new Random();

                Complex[,] array1 = new Complex[n, m];
                for (int r = 0; r < n; r++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        array1[r, j] = new Complex(rand.NextDouble(), rand.Next(10)) ;
                    }
                }

                Complex[,] array2 = new Complex[m, k];
                for (int r = 0; r < m; r++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        array2[r, j] = new Complex(rand.NextDouble(), rand.Next(10));
                    }
                }


                Matrix A = new Matrix(array1);
                Matrix B = new Matrix(array2);

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

                Console.WriteLine("PASSED: " + (Operations.isEqual(C1, C2)));
            }

            Console.ReadKey();
        }



    }
}
