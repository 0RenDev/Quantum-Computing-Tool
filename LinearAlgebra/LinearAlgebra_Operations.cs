using System.Numerics;

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
                ThreadPool.QueueUserWorkItem(delegate (object? state)
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
        public static Complex EuclideanNorm(Vector vector)
        {
            Complex[] elements = vector.elements;
            int len = elements.Length;

            Complex sum = new Complex(0.0, 0.0);
            for(int i = 0; i < len; i++)
            {
                sum += Complex.Pow(elements[i], 2.0);
            }

            return Complex.Sqrt(sum);
        }


        // The Cauchy–Schwarz inequality
    }

}
