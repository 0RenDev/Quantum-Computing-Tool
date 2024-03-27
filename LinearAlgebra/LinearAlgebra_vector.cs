using System.Numerics;
using System.Text;

namespace LinearAlgebra
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Vector class representing a mathematical vector
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Vector
    {
        public int rows;
        public int cols;
        // Array to store vector elements
        public Complex[] elements;

        public Vector(int rows)
        {
            this.rows = rows;
            cols = 1;
            elements = new Complex[rows];
        }

        // Constructor to initialize a vector with complex numbers
        public Vector(Complex[] elements)
        {
            this.elements = elements;
            this.rows = elements.Length;
            this.cols = 1;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Vector-related operations that can be done on one vector go here
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        // complex conjugate of each value in vector
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

        // Calculate the conjugate of each element in place
        public void ConjugateInPlace()
        {
            for (int i = 0; i < rows; i++)
            {
                elements[i] = Complex.Conjugate(elements[i]); 
            }
        }

        // Determine whether this is a row or column vector
        public bool IsRowVector() { return rows == 1; }


        public bool IsColVector() { return cols == 1; }

        // Transpose a Vector by swapping its rows and columns. Preserves original Vector and returns new Vector
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

        // Transpose a Vector in place
        public void TransposeInPlace()
        {
            // Swap rows and cols
            int temp = rows;
            rows = cols;
            cols = temp;
        }

        public Matrix ToMatrix => new Matrix(rows, cols, elements);

        // Override ToString
        public override string ToString()
        {
            StringBuilder vectorString = new StringBuilder("Vector:\n");

            foreach (var complexNumber in elements)
            {
                vectorString.AppendFormat("{0}+{1}i\n", complexNumber.Real, complexNumber.Imaginary);
            }

            return vectorString.ToString();
        }
    }
}