using System.Data.Common;
using System.Numerics;
using System.Text;

namespace LinearAlgebra
{
    // this is one of the library classes 
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Matrix class representing a mathematical matrix
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// This class represents a matrix in linear algebra. A matrix is a two-dimensional array of complex numbers.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.cols = columns;
            this.elements = new Complex[rows, columns];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="elements">The elements of the Matrix.</param>
        /// <exception cref="System.ArgumentNullException">elements</exception>
        public Matrix(Complex[,] elements)
        {
            this.elements = elements ?? throw new ArgumentNullException(nameof(elements)); // if elements is null, throw ArgumentNullException
            this.rows = elements.GetLength(0);
            this.cols = elements.GetLength(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="elements">The elements as a <see cref="Vector"/></param>
        public Matrix(Vector elements)
        {
            this.elements = new Complex[elements.rows, elements.cols];
            

        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <param name="rowNumber">The row index.</param>
        /// <returns>A specific row of a Matrix as a <see cref="System.Numerics.Complex[]"/></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">rowNumber - Row number is out of bounds.</exception>
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

        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <param name="columnNumber">The column index.</param>
        /// <returns>A specific column of a Matrix as a <see cref="System.Numerics.Complex[]"/></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">columnNumber - Column number is out of bounds.</exception>
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

        /// <summary>
        /// Gets or sets the element with the specified <see cref="Complex"/> number.
        /// </summary>
        /// <value>
        /// The <see cref="Complex"/>.
        /// </value>
        /// <param name="i">The row index.</param>
        /// <param name="j">The column index.</param>
        /// <returns>A <see cref="System.Numerics.Complex"/> element.</returns>
        public Complex this[int i, int j]
        {
            get { return elements[i, j]; }
            set { elements[i, j] = value; }
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Matrix-related operations that can be done on a single matrix 
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Transposes this instance.
        /// </summary>
        /// <returns>A transposed <see cref="Matrix"/></returns>
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

        /// <summary>
        /// Transposes the <see cref="Matrix"/> in place. 
        /// </summary>
        /// <exception cref="System.InvalidOperationException">In-place transpose can only be performed on square matrices.</exception>
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

        /// <summary>
        /// Calculates the trace this instance.
        /// </summary>
        /// <returns>The trace of the Matrix as a <see cref="System.Numerics.Complex"/>.</returns>
        /// <exception cref="System.InvalidOperationException">Trace is only defined for square matrices.</exception>
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

        /// <summary>
        /// Conjugates this instance.
        /// </summary>
        /// <returns>A conjugated <see cref="Matrix"/></returns>
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

        /// <summary>
        /// Conjugates the <see cref="Matrix"/> in place.
        /// </summary>
/        public void ConjugateInPlace()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    elements[i, j] = Complex.Conjugate(elements[i, j]);
                }
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Implements the operator op_Addition.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>
        /// The result of the additon.
        /// </returns>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            return Operations.Add(matrix1, matrix2);
        }

        /// <summary>
        /// Implements the operator op_Subtraction.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            return Operations.Subtract(matrix1, matrix2);
        }

        /// <summary>
        /// Implements the operator op_Multiply.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>
        /// The result of the subtraction.
        /// </returns>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            // change to multi-threaded implementation when working also consider dynamic approach where multi-threaded is used
            // if a matrix passes a size threshold to avoid the expensive overhead that the multi-threaded approach uses for small matrices
            return Operations.Multiply(matrix1, matrix2);
        }

        /// <summary>
        /// Implements the operator op_Multiply.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>
        /// The result of the multiplication.
        /// </returns>
        public static Vector operator *(Matrix matrix, Vector vector)
        {
            return Operations.MatrixVectorMult(matrix, vector);
        }

        /// <summary>
        /// Implements the operator op_Multiply.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>
        /// The result of the multiplication.
        /// </returns>
        public static Matrix operator *(Matrix matrix, Complex scalar)
        {
            return Operations.Multscaler(matrix, scalar);
        }

        /// <summary>
        /// Implements the operator op_Equality.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Matrix a, Matrix b)
        {
            // Check for null on left side.
            if (a is null)
            {
                return b is null;
            }

            // Use IsEqual to check for equality.
            return Operations.IsEqual(a, b);
        }

        /// <summary>
        /// Implements the operator op_Inequality.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Equalses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Adds the Matrices in place.
        /// </summary>
        /// <param name="matrixOther">The matrix to add.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The dimensions of both matrices must match.</exception>
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

        /// <summary>
        /// Subtracts the Matrices in place.
        /// </summary>
        /// <param name="matrixOther">The other matrix.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The dimensions of both matrices must match.</exception>
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

    /// <summary>
    /// This class represents an identity matrix in linear algebra. An identity matrix is a square matrix with ones on the main diagonal and zeros elsewhere.
    /// </summary>
    /// <seealso cref="LinearAlgebra.Matrix" />
    public class Idenity : Matrix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Idenity"/> class.
        /// </summary>
        /// <param name="size">The size of the matrix.</param>
        public Idenity(int size) : base(size, size)
        {
            for (int i = 0; i < size; i++)
            {
                elements[i, i] = 1;
            }
        }
    }
}

