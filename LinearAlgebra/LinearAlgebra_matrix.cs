using System.Data.Common;
using System.Numerics;
using System.Text;

namespace LinearAlgebra
{
    // this is one of the library classes 
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Matrix class representing a mathematical matrix
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Matrix
    {
        // array dimensions 
        public int rows { get; }
        public int cols { get; }

        // 2d array of complex numbers
        public Complex[,] elements;

        // Static lock object for use when multithreading
        private static Object _lock = new Object(); // TODO

        // Constructor to initialize a matrix with complex numbers and diemensions
        public Matrix(int rows, int cols, Complex[] elements)
        {
            this.rows = rows;   
            this.cols = cols;   

            if (rows * cols != elements.Length) // if there are a different number of spaces and elements, throw ArgumentException
            {
                throw new ArgumentException("The number of elements must match the matrix dimensions.");
            }

            this.elements = new Complex[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    this.elements[i, j] = elements[i * cols + j];
                }
            }
        }

        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.cols = columns;
            this.elements = new Complex[rows, columns];
        }
        
        // Constructor to initialize a matrix with complex numbers using a 2D array
        public Matrix(Complex[,] elements)
        {
            this.elements = elements ?? throw new ArgumentNullException(nameof(elements)); // if elements is null, throw ArgumentNullException
            this.rows = elements.GetLength(0);
            this.cols = elements.GetLength(1);
        }

        // Get a single row of a matrix
        public Complex[] GetRow(int rowNumber)
        {
            if (rowNumber < 0 || rowNumber >= rows) 
            {
                throw new ArgumentOutOfRangeException(nameof(rowNumber), "Row number is out of bounds.");
            }

            Complex[] row = new Complex[cols];

            for (int j = 0; j < cols; j++)
            {
                row[j] = elements[rowNumber, j];
            }

            return row;
        }

        // Get a single column of a matrix
        public Complex[] GetColumn(int columnNumber)
        {
            if (columnNumber < 0 || columnNumber >= cols)
            {
                throw new ArgumentOutOfRangeException(nameof(columnNumber), "Column number is out of bounds.");
            }

            Complex[] column = new Complex[rows];

            for (int i = 0; i < rows; i++)
            {
                column[i] = elements[i, columnNumber];
            }

            return column;
        }

        // Get a specific element of the matrix (indexer)
        public Complex this[int i, int j]
        {
            get { return elements[i, j]; }
            set { elements[i, j] = value; }
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Matrix-related operations that can be done on a single matrix 
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Transpose the matrix
        public Matrix Transpose()
        {
            // Create a new matrix with swapped rows and columns
            Complex[,] transposedElements = new Complex[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    transposedElements[i, j] = elements[j, i];
                }
            }

            // Return the transposed matrix
            return new Matrix(transposedElements);
        }

        // Transpose square matrix in place to reduce memory overhead
        public void TransposeInPlace()
        {
            if (rows != cols)
            {
                throw new InvalidOperationException("In-place transpose can only be performed on square matrices.");
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = i + 1; j < cols; j++)
                {
                    Complex temp = elements[i, j];
                    elements[i, j] = elements[j, i];
                    elements[j, i] = temp;
                }
            }
        }

        // Calculate the trace of the matrix
        public Complex Trace()
        {
            if (rows != cols)
            {
                throw new InvalidOperationException("Trace is only defined for square matrices.");
            }

            Complex trace = Complex.Zero;

            for (int i = 0; i < rows; i++)
            {
                trace += elements[i, i];
            }

            return trace;
        }

        // calculate the complex conjugate of the matrix
        public Matrix Conjugate()
        {
            // Create a new matrix to store result
            Complex[,] conjugateElements = new Complex[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    conjugateElements[i, j] = Complex.Conjugate(elements[i, j]);
                }
            }

            // Return the elementally conjugated matrix
            return new Matrix(conjugateElements);
        }

        // Calculate the elemental conjugate for a matrix in place
        public void ConjugateInPlace()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    elements[i, j] = Complex.Conjugate(elements[i, j]);
                }
            }
        }

        // Override ToString
        public override string ToString()
        {
            int rows = elements.GetLength(0);
            int cols = elements.GetLength(1);

            StringBuilder matrixString = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var complexNumber = elements[i, j];
                    matrixString.AppendFormat("{0}+{1}i \t", complexNumber.Real, complexNumber.Imaginary);
                }
                matrixString.AppendLine();
            }

            return matrixString.ToString();
        }




        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Matrix-related operations that involve 2 objects
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Operator Overloading
        // Addition 
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            return Operations.Add(matrix1, matrix2);
        }

        // Subtraction
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            return Operations.Subtract(matrix1, matrix2);
        }

        // Multiplication
        // Matrix x Matrix
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            // change to multi-threaded implementation when working also consider dynamic approach where multi-threaded is used
            // if a matrix passes a size threshold to avoid the expensive overhead that the multi-threaded approach uses for small matrices
            return Operations.Multiply(matrix1, matrix2);
        }

        // Matrix x Vector
        public static Vector operator *(Matrix matrix, Vector vector)
        {
            return Operations.MatrixVectorMult(matrix, vector);
        }

        // Matrix x Scalar
        public static Matrix operator *(Matrix matrix, Complex scalar)
        {
            return Operations.Multscaler(matrix, scalar);
        }

        // Equality
        // Floating point comparison could cause issues later down the line, keep this in mind
        public static bool operator ==(Matrix a, Matrix b)
        {
            // Check for null on left side.
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            // Use IsEqual to check for equality.
            return Operations.IsEqual(a, b);
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }

        // In-place operations to cut down on memory overhead, they alter the instance that calls them and preserves the other one
        // Addition
        public void AddInPlace(Matrix matrixOther)
        {
            if (this.cols != matrixOther.cols || this.rows != matrixOther.rows) // If size mismatch, throw ArgumentException
            {
                throw new ArgumentException("The dimensions of both matrices must match.");
            }

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    this.elements[i, j] += matrixOther.elements[i, j];
                }
            }
        }

        // Subtraction
        public void SubtractInPlace(Matrix matrixOther)
        {
            if (this.cols != matrixOther.cols || this.rows != matrixOther.rows) // If size mismatch, throw ArgumentException
            {
                throw new ArgumentException("The dimensions of both matrices must match.");
            }

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    this.elements[i, j] -= matrixOther.elements[i, j];
                }
            }
        }
    }
}

