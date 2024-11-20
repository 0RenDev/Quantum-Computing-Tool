using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace QuantumComputing.LinearAlgebra
{
    // this is one of the library classes 
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Operations class containing common linear algebra operations
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// This class contains common linear algebra operations that can be performed on matrices and vectors.
    /// </summary>
    public class Operations
    {
        /// <summary>
        /// Adds the instance with the specified matrix.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The dimensions of both matrices must match.</exception>
        public static Matrix Add(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.cols != matrix2.cols || matrix1.rows != matrix2.rows)
            {
                throw new ArgumentException("The dimensions of both matrices must match.");
            }

            Complex[,] addedElements = new Complex[matrix2.rows, matrix1.cols];

            for (int i = 0; i < matrix2.rows; i++)
            {
                for (int j = 0; j < matrix1.cols; j++)
                {
                    addedElements[i, j] = matrix1.elements[i, j] + matrix2.elements[i, j];
                }
            }
            return new Matrix(addedElements);
        }



        /// <summary>
        /// Subtracts the specified matrix from the instance.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The dimensions of both matrices must match.</exception>
        public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.cols != matrix2.cols || matrix1.rows != matrix2.rows)
            {
                throw new ArgumentException("The dimensions of both matrices must match.");
            }

            Complex[,] resultElements = new Complex[matrix1.rows, matrix1.cols];

            for (int i = 0; i < matrix1.rows; i++)
            {
                for (int j = 0; j < matrix1.cols; j++)
                {
                    resultElements[i, j] = matrix1.elements[i, j] - matrix2.elements[i, j];
                }
            }
            return new Matrix(resultElements);
        }



        /// <summary>
        /// Multiplies the specified matrix with the instance. 
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The number of columns in the first matrix must match the number of rows in the second matrix.</exception>
        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.cols != matrix2.rows)
            {
                throw new ArgumentException("The number of columns in the first matrix must match the number of rows in the second matrix.");
            }

            Complex[,] resultElements = new Complex[matrix1.rows, matrix2.cols];

            for (int i = 0; i < matrix1.rows; i++)
            {
                for (int j = 0; j < matrix2.cols; j++)
                {
                    for (int k = 0; k < matrix1.cols; k++)
                    {
                        resultElements[i, j] += matrix1.elements[i, k] * matrix2.elements[k, j];
                    }
                }
            }

            return new Matrix(resultElements);
        }

        /// <summary>
        /// Multiplies the matrices concurrently.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The number of columns in the first matrix must match the number of rows in the second matrix.</exception>
        public static Matrix MatrixMultiply(Matrix a, Matrix b)
        {
            // Get dimensions of the matrices
            int m1 = a.rows;
            int n1 = a.cols;
            int m2 = b.rows;
            int n2 = b.cols;

            // Check if matrices can be multiplied
            if (n1 != m2)
            {
                throw new ArgumentException("The number of columns in the first matrix must match the number of rows in the second matrix.");
            }

            // Create a matrix to return
            Complex[,] array = new Complex[m1, n2];
            Matrix result = new Matrix(array);

            // Set the number of threads based on system requirements. We want enough threads to run the operation quickly, but not so many that it drastically starves the CPU

            int numThreads = Environment.ProcessorCount;
            ManualResetEvent[] doneEvents = new ManualResetEvent[numThreads]; // Create a list of reset events to wait on threads

            // Calculate the number of rows handled by a single thread
            int rowsPerThread = m1 / numThreads;
            Console.WriteLine("Num Threads: " + numThreads);
            Console.WriteLine("Rows per thread: " + rowsPerThread);

            // Spawn threads
            for (int i = 0; i < numThreads; i++)
            {
                // Calculate which range ith thread is operating on
                int startRow = i * rowsPerThread;
                int endRow = (i == numThreads - 1) ? m1 : (i + 1) * rowsPerThread;

                // Set the event as not done
                doneEvents[i] = new ManualResetEvent(false);

                // Add a delegate to the thread pool
                ThreadPool.QueueUserWorkItem(delegate (object state)
                {
                    if (state != null)
                    {
                        for (int row = startRow; row < endRow; row++)
                        {
                            for (var col = 0; col < n2; col++)
                            {
                                Complex sum = 0;
                                for (int k = 0; k < n1; k++)
                                {
                                    sum += a.elements[row, k] * b.elements[k, col];
                                }
                                result.elements[row, col] = sum;
                            }
                        }
                        ((ManualResetEvent)state).Set(); // Set thread as up when operation is done
                    }

                }, doneEvents[i]);
            }

            // Wait for all threads to finish
            WaitHandle.WaitAll(doneEvents);

            // Return the result matrix
            return result;
        }

        /// <summary>
        /// Multiplies the matrix with the vector.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>A <see cref="Vector"/></returns>
        /// <exception cref="System.ArgumentException">Left Multiplications must have similar dimensions.</exception>
        public static Vector MatrixVectorMult(Matrix matrix, Vector vector)
        {
            if(matrix.cols != vector.rows)
            {
                throw new ArgumentException("Left Multiplications must have similar dimensions.");
            }

            int matrixCols = matrix.cols;
            int rows = matrix.rows;

            Complex[] result = new Complex[rows];

            for(int i = 0; i < rows; i++)
            {
                Vector row = new Vector(matrix.GetRow(i));
                result[i] = InnerProduct(row, vector);
            }

            return new Vector(result); 
        }

        /// <summary>
        /// Multiplies a matrix with a scalar.
        /// </summary>
        /// <param name="matrix1">The matrix.</param>
        /// <param name="scaler">The scaler.</param>
        /// <returns></returns>
        public static Matrix Multscaler(Matrix matrix1, Complex scaler)
        {

            Complex[,] scaledElements = new Complex[matrix1.rows, matrix1.cols];

            for (int i = 0; i < matrix1.rows; i++)
            {
                for (int j = 0; j < matrix1.cols; j++)
                {
                    scaledElements[i, j] = scaler * matrix1.elements[i, j];
                }
            }
            return new Matrix(scaledElements);
        }



        /// <summary>
        /// Determines whether the specified a is equal.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns>
        ///   <c>true</c> if the specified a is equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEqual(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.cols != b.cols) return false; // If dimensions don't match, they aren't equal

            int rows = a.rows;
            int cols = b.cols;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (a.elements[i, j] != b.elements[i, j]) return false; // If each element does not match, they aren't equal
                }
            }

            return true; // Else, they are equal
        }



        /// <summary>
        /// Tensors the product.
        /// </summary>
        /// <param name="matrix1">The matrix1.</param>
        /// <param name="matrix2">The matrix2.</param>
        /// <returns></returns>
        public static Matrix TensorProduct(Matrix matrix1, Matrix matrix2)
        {
            int rows1 = matrix1.rows;
            int cols1 = matrix1.cols;
            int rows2 = matrix2.rows;
            int cols2 = matrix2.cols;

            // Tensor product matrix is an n*m x k*p matrix for two matrices nxk and mxp
            Complex[,] resultElements = new Complex[rows1 * rows2, cols1 * cols2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols1; j++)
                {
                    for (int m = 0; m < rows2; m++)
                    {
                        for (int n = 0; n < cols2; n++)
                        {
                            resultElements[i * rows2 + m, j * cols2 + n] = matrix1.elements[i, j] * matrix2.elements[m, n];
                        }
                    }
                }
            }

            return new Matrix(resultElements);
        }

        /// <summary>
        /// A method to tensor two <see cref="Vector"/> together.
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>A <see cref="Vector"/>.</returns>
        public static Vector TensorProductofVectors(Vector vector1, Vector vector2)
        {
            int rows1 = vector1.rows;
            int rows2 = vector2.rows;

            Complex[] new_elements = new Complex[rows1 * rows2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < rows2; j++)
                {
                    new_elements[i * rows2 + j] = vector1.elements[i] * vector2.elements[j];
                }
            }


            return new Vector(new_elements);
        }

        /// <summary>
        /// A method to perform the inner product (dot product) of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Complex InnerProduct(Vector vector1, Vector vector2)
        {

            Complex[] elements1 = vector1.elements;
            int len1 = elements1.Length;
            Complex[] elements2 = vector2.elements;
            int len2 = elements2.Length;


            if(vector1 == null || vector2 == null)
            {
                throw new ArgumentNullException("One or more input arguments are Null.");
            }

          
            if(len1 != len2)
            {
                throw new ArgumentException("Two vectors must have the same length to perform an inner product.");
            }

            if(len1 == 0)
            {
                return Complex.Zero;
            }

            Complex innerProduct = Complex.Zero;

            for(int i = 0; i < len1; i++)
            {
                innerProduct += elements1[i] * elements2[i];
            }
            
            return innerProduct;
        }

        /// <summary>
        /// A method to perform the outer product (cross product) of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Matrix OuterProduct(Vector vector1, Vector vector2)
        {

            Complex[] elements1 = vector1.elements;
            int len1 = elements1.Length;
            Complex[] elements2 = vector2.elements;
            int len2 = elements2.Length;


            if (vector1 == null || vector2 == null)
            {
                throw new ArgumentNullException("One or more input arguments are null.");
            }


            if (len1 != len2)
            {
                throw new ArgumentException("Two vectors must have the same length to perform an outer product.");
            }

            if (len1 == 0)
            {
                throw new ArgumentException("One or more vectors has a length of zero.");
            }

            Matrix outerProduct = new Matrix(len1, len2);

            for (int i = 0; i < len1; i++)
            {
                for(int j = 0; j < len2; j++)
                {
                    outerProduct.elements[i, j] = elements1[i] * elements2[j];
                }
                
            }

            return outerProduct;
        }



        /// <summary>
        /// A method to calculate the Euclidean norm of a <see cref="Vector"/>.
        /// 
        /// The Euclidean norm refers to the squart root of the sum of the squares of the elements of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double EuclideanNorm(Vector vector)
        {
            double sum = 0.0; 

            for (int i = 0; i < vector.elements.Length; i++)
            {
                sum += Complex.Abs(vector.elements[i]) * Complex.Abs(vector.elements[i]); // Absolute value squared
            }

            return Math.Sqrt(sum); // Return the square root of the sum.
        }

        /// <summary>
        /// A method to calculate the Euclidean norm of a <see cref="Vector"/> as a <see cref="System.Numerics.Complex"/>
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Complex EuclideanNormAsComplex(Vector vector)
        {
            return new Complex(EuclideanNorm(vector), 0);
        }

        /// <summary>
        /// Calculate determinant of a <see cref="Matrix"/>
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static double Determinant(Matrix matrix)
        {
            if (matrix.rows != matrix.cols)
                throw new ArgumentException("Matrix must be square.");

            int size = matrix.rows;

            if (size == 1) // Base case for 1x1
            {
                return matrix.elements[0, 0].Real;
            }
            else if (size == 2) // Base case for 2x2
            {
                return matrix.elements[0, 0].Real * matrix.elements[1, 1].Real -
                       matrix.elements[0, 1].Real * matrix.elements[1, 0].Real;
            }
            else
            {
                // Recursive case for matrices larger than 2x2
                double det = 0;
                int sign = 1; // utility variable to alternate signs in expansion

                for (int j = 0; j < size; j++)
                {
                    // Get subMatrix by removing 1st row and current column
                    Matrix subMatrix = GetSubMatrix(matrix, 0, j); 
                    // Recursive call to get the determinant of submatrix
                    // Accumulate all the rsults, adjusting based on the sign
                    det += sign * matrix.elements[0, j].Real * Determinant(subMatrix);
                    // Inverse sign
                    sign = -sign;
                }

                return det;
            }
        }

        // Utility method for determinant method
        // Generates a submatrix by removing a row and a column from the input matrix
        private static Matrix GetSubMatrix(Matrix matrix, int rowToRemove, int colToRemove)
        {
            int size = matrix.rows;
            Complex[,] elements = new Complex[size - 1, size - 1];
            int rowCount = 0, colCount;

            for (int i = 0; i < size; i++)
            {
                if (i == rowToRemove) // Skip the row to remove
                    continue;

                colCount = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j == colToRemove) // Skip the column to remove
                        continue;

                    // Assign the remaining elements to the new submatrix
                    elements[rowCount, colCount] = matrix.elements[i, j]; 
                    colCount++;
                }
                rowCount++;
            }

            return new Matrix(elements);
        }

        /// <summary>
        /// Return a <see cref="Matrix"/> with all ones along the diaganol
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix GenerateIdentityMatrix(int size)
        {
            Complex[,] elements = new Complex[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Fill matrix with 1's on the diagonal and 0 everywhere else
                    elements[i, j] = (i == j) ? Complex.One : Complex.Zero; 
                }
            }

            return new Matrix(elements);
        }



        /// <summary>
        /// A method to convert the inverse of a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix Invert(Matrix matrix)
        {
            if (matrix.rows != matrix.cols)
            {
                throw new InvalidOperationException("Matrix must be square for inversion.");
            }

            if (Determinant(matrix) == 0)
            {
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            }

            int size = matrix.rows;
            Matrix identity = Operations.GenerateIdentityMatrix(size);
            Matrix augmentedMatrix = JoinMatrices(matrix, identity);

            // Apply Gauss-Jordan elimination
            for (int i = 0; i < size; i++)
            {
                // Divide the row by the diagonal element to make it 1
                Complex divisor = augmentedMatrix.elements[i, i];
                for (int j = 0; j < 2 * size; j++)
                {
                    augmentedMatrix.elements[i, j] /= divisor;
                }

                // Eliminate other elements in the column
                for (int k = 0; k < size; k++)
                {
                    if (k != i)
                    {
                        Complex factor = augmentedMatrix.elements[k, i];
                        for (int j = 0; j < 2 * size; j++)
                        {
                            augmentedMatrix.elements[k, j] -= factor * augmentedMatrix.elements[i, j];
                        }
                    }
                }
            }

            // Extract the inverse matrix
            Complex[,] inverseElements = new Complex[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    inverseElements[i, j] = augmentedMatrix.elements[i, j + size];
                }
            }

            return new Matrix(inverseElements);
        }

        /// <summary>
        /// A method for joining two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix JoinMatrices(Matrix matrix1, Matrix matrix2)
        {
            int rows = matrix1.rows;
            int cols = matrix1.cols + matrix2.cols;
            Complex[,] joinedElements = new Complex[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < matrix1.cols; j++)
                {
                    joinedElements[i, j] = matrix1.elements[i, j];
                }

                for (int j = 0; j < matrix2.cols; j++)
                {
                    joinedElements[i, j + matrix1.cols] = matrix2.elements[i, j];
                }
            }

            return new Matrix(joinedElements);
        }

    }



}
