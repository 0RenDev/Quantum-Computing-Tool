using System.Numerics;
using System.Text;

namespace LinearAlgebra
{
    /// <summary>
    /// This class represents a vector in linear algebra. A vector is a one-dimensional array of complex numbers.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// The number of rows in the vector.
        /// </summary>
        public int rows;

        /// <summary>
        /// The number of columns in the vector.
        /// </summary>
        public int cols;

        /// <summary>
        /// The elements of the vector.
        /// </summary>
        public Complex[] elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="rows">The number of rows (or number of elements) in the vector.</param>
        public Vector(int rows)
        {
            this.rows = rows;
            cols = 1;
            elements = new Complex[rows];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="elements">The elements of the Vector.</param>
        public Vector(Complex[] elements)
        {
            this.elements = elements;
            this.rows = elements.Length;
            this.cols = 1;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Vector-related operations that can be done on one vector go here
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Conjugates this <see cref="Vector"/> instance.
        /// </summary>
        /// <returns>A conjugated <see cref="Vector"/></returns>
        public Vector Conjugate()
        {
            int vectorLength = rows;
            Complex[] conjugateElements = new Complex[vectorLength];

            for (int i = 0; i < vectorLength; i++)
            {
                conjugateElements[i] = Complex.Conjugate(elements[i]);
            }

            return new Vector(conjugateElements);
        }

        /// <summary>
        /// Conjugates the <see cref="Vector"/> in place. This method modifies the original <see cref="Vector"/>.
        /// </summary>
        public void ConjugateInPlace()
        {
            for (int i = 0; i < rows; i++)
            {
                elements[i] = Complex.Conjugate(elements[i]); 
            }
        }

        /// <summary>
        /// Determines whether [is row vector].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is row vector]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRowVector() { return rows == 1; }

        /// <summary>
        /// Determines whether [is col vector].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is col vector]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsColVector() { return cols == 1; }

        /// <summary>
        /// Transposes the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A transposed <see cref="Vector"/></returns>
        public static Vector Transpose(Vector vector)
        {
            Vector transposed = new Vector(vector.elements)
            {
                // Transposing is effectively swapping rows to columns for Vectors
                cols = vector.rows,
                rows = vector.cols
            };

            return transposed;
        }

        /// <summary>
        /// Transposes the <see cref="Vector"/> in place. This method modifies the original <see cref="Vector"/>.
        /// </summary>
        public void TransposeInPlace()
        {
            // Swap rows and cols
            int temp = rows;
            rows = cols;
            cols = temp;
        }

        /// <summary>
        /// Determines whether [is approximately equal] [the specified other].
        /// </summary>
        /// <param name="other">The other <see cref="Vector"/>.</param>
        /// <param name="tolerance">The tolerance. Default tolerance is 1e-10</param>
        /// <returns>
        ///   <c>true</c> if [is approximately equal] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsApproximatelyEqual(Vector other, double tolerance = 1e-10)
        {
            if (this.rows != other.rows || this.cols != other.cols)
                return false;

            for (int i = 0; i < this.elements.Length; i++)
            {
                if (Complex.Abs(this.elements[i] - other.elements[i]) > tolerance)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Converts to matrix.
        /// </summary>
        /// <value>
        /// To <see cref="Matrix"/>.
        /// </value>
        public Matrix ToMatrix => new Matrix(rows, cols, elements);

        /// <summary>
        /// Gets the state of the <see cref="Vector"/>.
        /// </summary>
        /// <returns>A <see cref="System.Numerics.Complex[]"/> of the elements.</returns>
        public Complex[] GetState() => elements;

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder vectorString = new StringBuilder("Vector:\n");
            int count = 0;

            foreach (var complexNumber in elements)
            {
                vectorString.AppendFormat("{0:0.0000}+{1:0.0000}i, ",
                                       complexNumber.Real,complexNumber.Imaginary);
                count++;

                // Insert a newline after every 8 elements
                if (count % 8 == 0)
                {
                    vectorString.AppendLine();
                }
            }

            // Trim any trailing whitespace or newlines if needed
            return vectorString.ToString().TrimEnd();
        }
    }
}
