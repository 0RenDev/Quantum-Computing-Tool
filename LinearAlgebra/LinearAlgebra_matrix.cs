using System.Data.Common;
using System.Numerics;

namespace LinearAlgebra
{
    // this is one of the library classes 
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Matrix class representing a mathematical matrix
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Matrix
    {
        // array dimensions 
        public int rows { get; set; }
        public int cols { get; set; }

        // 2d array of complex numbers
        public Complex[,] elements;

        // Static lock object for use when multithreading
        private static Object _lock = new Object();

        // Constructor to initialize a matrix with complex numbers and diemensions
        public Matrix(int rows, int cols, Complex[] elements)
        {
            this.rows = rows;   
            this.cols = cols;   

            if (rows * cols != elements.Length)
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
            this.elements = elements ?? throw new ArgumentNullException(nameof(elements));
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
            // Create a new matrix with swapped rows and columns
            Complex[,] conjugateElements = new Complex[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    conjugateElements[i, j] = Complex.Conjugate(elements[i, j]);
                }
            }

            // Return the transposed matrix
            return new Matrix(conjugateElements);
        }

        // Override ToString
        public override string ToString()
        {
            int rows = elements.GetLength(0);
            int cols = elements.GetLength(1);

            string matrixString = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var complexNumber = elements[i, j];
                    matrixString += $"{complexNumber.Real}+{complexNumber.Imaginary}i \t";
                }
                matrixString += "\n";
            }

            return matrixString;
        }
    }
}

