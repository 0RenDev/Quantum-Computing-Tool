using System.Numerics;
using System.Threading;

namespace LinearAlgebra
{
    // this is one of the library classes 
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Operations class containing common linear algebra operations
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Operations
    {
        // matrix addition
        public static Matrix Add(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.cols != matrix2.cols || matrix1.rows != matrix2.rows)
            {
                throw new ArgumentException("The number of columns in the first matrix must match the number of rows in the second matrix.");
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


        // matrix multplied by scaler
        public static Matrix Multscaler(Matrix matrix1 , Complex scaler)
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


        // Matrix-Matrix multiplication
        // naive approach should be replaced by multithreaded or other method
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
                    // Perform a matrix multiplication. Not sure why there are warnings here. should fix 
                    for (int row = startRow; row < endRow; row++)
                    {
                        for (int col = 0; col < n2; col++)
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
                }, doneEvents[i]);
            }

            // Wait for all threads to finish
            WaitHandle.WaitAll(doneEvents);

            // Return the result matrix
            return result;
        }

        // Checks equality of two matrices
        public static bool isEqual(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.cols != b.cols) return false;

            int rows = a.rows;
            int cols = b.cols;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (a.elements[i, j] != b.elements[i, j]) return false;
                }
            }

            return true;
        }
 
        // Tensor product between two matrices. simple appoarch
        public static Matrix TensorProduct(Matrix matrix1, Matrix matrix2)
        {
            int rows1 = matrix1.rows;
            int cols1 = matrix1.cols;
            int rows2 = matrix2.rows;
            int cols2 = matrix2.cols;

            // Tensor product matrix is an n*mxk*p matrix for two matrices nxk and mxp
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

        // Vector- Vector Inner Product
        // euclidean norm^2 = Inner Product of vector with itself
        // The Cauchy–Schwarz inequality

        public static double Determinant(Matrix matrix)
        {
            if (matrix.rows != matrix.cols)
                throw new ArgumentException("Matrix must be square.");

            int size = matrix.rows;

            if (size == 1)
            {
                return matrix.elements[0, 0].Real;
            }
            else if (size == 2)
            {
                return matrix.elements[0, 0].Real * matrix.elements[1, 1].Real -
                       matrix.elements[0, 1].Real * matrix.elements[1, 0].Real;
            }
            else
            {
                double det = 0;
                int sign = 1;

                for (int j = 0; j < size; j++)
                {
                    Matrix subMatrix = GetSubMatrix(matrix, 0, j);
                    det += sign * matrix.elements[0, j].Real * Determinant(subMatrix);
                    sign = -sign;
                }

                return det;
            }
        }

        private static Matrix GetSubMatrix(Matrix matrix, int rowToRemove, int colToRemove)
        {
            int size = matrix.rows;
            Complex[,] elements = new Complex[size - 1, size - 1];
            int rowCount = 0, colCount;

            for (int i = 0; i < size; i++)
            {
                if (i == rowToRemove)
                    continue;

                colCount = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j == colToRemove)
                        continue;

                    elements[rowCount, colCount] = matrix.elements[i, j];
                    colCount++;
                }
                rowCount++;
            }

            return new Matrix(elements);
        }

        public static Matrix GenerateIdentityMatrix(int size)
        {
            Complex[,] elements = new Complex[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    elements[i, j] = (i == j) ? Complex.One : Complex.Zero;
                }
            }

            return new Matrix(elements);
        }


        // Method to compute the inverse of a matrix.
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

        // Method to join two matrices
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
