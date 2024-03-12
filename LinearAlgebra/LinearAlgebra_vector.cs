using System.Numerics;

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

        // Determine whether this is a row or column vector
        public bool isRowVector() { return rows == 1; }


        public bool isColVector() { return cols == 1; }

        // Transpose a Vector by swapping is rows and columns. Preserves original Vector and returns new Vector
        public Vector Transpose(Vector vector)
        {
            Vector transposed = new Vector(vector.elements);

            // Transposing is effectively swapping rows to columns for Vectors
            transposed.cols = vector.rows;
            transposed.rows = vector.cols;

            return transposed;
        }

        // Override ToString
        public override string ToString()
        {
            string vectorString = "";

            foreach (var complexNumber in elements)
            {
                vectorString += $"{complexNumber.Real}+{complexNumber.Imaginary}i\n";
            }

            return vectorString;
        }
    }
}