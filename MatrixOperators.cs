using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;


namespace MatrixOperators
{
    public class MatrixOperations
    {
        // Utility function to print matrix.
        public static void printMatrix(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write(a[i, j]);
                    Console.Write(", ");
                }
                Console.WriteLine();
            }
        }

        public static int[,] generateMatrix(int m, int n, int minNum, int maxNum)
        {
            Random rand = new Random();
            int num = 1;
            int[,] newMatrix = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //newMatrix[i, j] = rand.Next(minNum, maxNum);
                    newMatrix[i, j] = num++;
                }
            }
            return newMatrix;
        }
        // Multiplies matrix a against matrix b if the number of columns in a
        // is equal to the number of rows in b.
        public static int[,] matrixMult(int[,] a, int[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0))
            {
                throw new ArgumentException("Matrix Size Mismatch.");
            }
            else
            {
                int[,] product = new int[a.GetLength(0), b.GetLength(1)];
                // Going through each column in b.
                for (int i = 0; i < b.GetLength(1); i++)
                {
                    // Going through each row in a.
                    for (int j = 0; j < a.GetLength(0); j++)
                    {
                        // Setting the current product index to the dot product at
                        // the current row in a and the current column in b.
                        product[j, i] = dotProduct(j, i, a, b);
                    }
                }
                return product;
            }
        }

        // Computes the dot product of the row of a and the column of b.
        public static int dotProduct(int j, int i, int[,] a, int[,] b)
        {
            int dotProduct = 0;
            for (int q = 0; q < a.GetLength(1); q++)
            {
                dotProduct += a[j, q] * b[q, i];
            }
            return dotProduct;
        }


        public static int[,] strassMultiply(int[,] matrix1, int[,] matrix2)
        {
            int rowsA = matrix1.GetLength(0);
            int colsA = matrix1.GetLength(1);
            int rowsB = matrix2.GetLength(0);
            int colsB = matrix2.GetLength(1);

            if (colsA != rowsB)
                throw new ArgumentException("Number of columns in matrix1 must be equal to the number of rows in matrix2.");

            // Check if the dimensions are power of 2, if not pad with zeros
            int n = Math.Max(Math.Max(rowsA, colsA), Math.Max(rowsB, colsB));
            int dimension = 1;
            while (dimension < n)
                dimension *= 2;

            int[,] paddedMatrix1 = PadMatrix(matrix1, dimension);
            int[,] paddedMatrix2 = PadMatrix(matrix2, dimension);

            // Perform the multiplication using Strassen algorithm
            int[,] result = StrassenMultiply(paddedMatrix1, paddedMatrix2, dimension);
            return TrimMatrix(result, colsB, rowsA);
        }

        private static int[,] TrimMatrix(int[,] result, int colsB, int rowsA)
        {
            int[,] trimmedMatrix = new int[rowsA, colsB];
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    trimmedMatrix[i, j] = result[i, j];
                }
            }
            return trimmedMatrix;
        }

        private static int[,] StrassenMultiply(int[,] matrix1, int[,] matrix2, int n)
        {
            int[,] result = new int[n, n];

            if (n == 1)
            {
                result[0, 0] = matrix1[0, 0] * matrix2[0, 0];
                return result;
            }

            int halfSize = n / 2;

            int[,] a11 = GetSubMatrix(matrix1, 0, 0, halfSize);
            int[,] a12 = GetSubMatrix(matrix1, 0, halfSize, halfSize);
            int[,] a21 = GetSubMatrix(matrix1, halfSize, 0, halfSize);
            int[,] a22 = GetSubMatrix(matrix1, halfSize, halfSize, halfSize);

            int[,] b11 = GetSubMatrix(matrix2, 0, 0, halfSize);
            int[,] b12 = GetSubMatrix(matrix2, 0, halfSize, halfSize);
            int[,] b21 = GetSubMatrix(matrix2, halfSize, 0, halfSize);
            int[,] b22 = GetSubMatrix(matrix2, halfSize, halfSize, halfSize);

            int[,] s1 = Subtract(b12, b22);
            int[,] s2 = Add(a11, a12);
            int[,] s3 = Add(a21, a22);
            int[,] s4 = Subtract(b21, b11);
            int[,] s5 = Add(a11, a22);
            int[,] s6 = Add(b11, b22);
            int[,] s7 = Subtract(a12, a22);
            int[,] s8 = Add(b21, b22);
            int[,] s9 = Subtract(a11, a21);
            int[,] s10 = Add(b11, b12);

            int[,] p1 = StrassenMultiply(a11, s1, halfSize);
            int[,] p2 = StrassenMultiply(s2, b22, halfSize);
            int[,] p3 = StrassenMultiply(s3, b11, halfSize);
            int[,] p4 = StrassenMultiply(a22, s4, halfSize);
            int[,] p5 = StrassenMultiply(s5, s6, halfSize);
            int[,] p6 = StrassenMultiply(s7, s8, halfSize);
            int[,] p7 = StrassenMultiply(s9, s10, halfSize);

            int[,] c11 = Add(Subtract(Add(p5, p4), p2), p6);
            int[,] c12 = Add(p1, p2);
            int[,] c21 = Add(p3, p4);
            int[,] c22 = Subtract(Subtract(Add(p5, p1), p3), p7);

            CopySubMatrix(c11, result, 0, 0);
            CopySubMatrix(c12, result, 0, halfSize);
            CopySubMatrix(c21, result, halfSize, 0);
            CopySubMatrix(c22, result, halfSize, halfSize);

            return result;
        }


        private static int[,] PadMatrix(int[,] matrix, int newSize)
        {
            int[,] paddedMatrix = new int[newSize, newSize];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    paddedMatrix[i, j] = matrix[i, j];
                }
            }
            return paddedMatrix;
        }

        private static int[,] GetSubMatrix(int[,] matrix, int rowStart, int colStart, int size)
        {
            int[,] submatrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    submatrix[i, j] = matrix[rowStart + i, colStart + j];
                }
            }
            return submatrix;
        }

        private static int[,] Add(int[,] matrix1, int[,] matrix2)
        {
            int n = matrix1.GetLength(0);
            int[,] result = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }

        private static int[,] Subtract(int[,] matrix1, int[,] matrix2)
        {
            int n = matrix1.GetLength(0);
            int[,] result = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return result;
        }

        private static void CopySubMatrix(int[,] submatrix, int[,] result, int rowStart, int colStart)
        {
            int size = submatrix.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[rowStart + i, colStart + j] = submatrix[i, j];
                }
            }
        }

        // First pass at a tensor product algorithm, complexity of n^4.
        public static int[,] tensorProduct(int[,] a, int[,] b)
        {
            // Tensor product matrix is an n*mxk*p matrix for two matrices nxk and mxp
            int[,] tensorProduct = new int[a.GetLength(0) * b.GetLength(0), a.GetLength(1) * b.GetLength(1)];
            // Going down the rows of a and b
            for(int i = 0; i < a.GetLength(0); i++)
            {
                for(int j = 0; j < b.GetLength(0); j++)
                {
                    // Multiplying the index at row i and column q against row j and column z for the tensor product.
                    for(int q = 0; q < a.GetLength(1); q++)
                    {
                        for (int z = 0; z < b.GetLength(1); z++)
                        {
                            // Index for tensor product calculation.
                            tensorProduct[(b.GetLength(0) * i) + j, (b.GetLength(1) * q) + z] = a[i, q] * b[j, z];
                        }
                    }
                }
            }
            return tensorProduct;
        }
    }
}

