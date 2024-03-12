using System.Numerics;

namespace LinearAlgebra
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Vector class representing a mathematical vector
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Vectors
    {
        public int rows;
        public int cols;
        // Array to store vector elements
        public Complex[] elements;

        // Constructor to initialize a vector with complex numbers
        public Vectors(Complex[] elements)
        {
            this.elements = elements;
            this.rows = elements.Length;
            this.cols = 1;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Vector-related operations that can be done on one vector go here
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        // complex conjugate of each value in vector
        public Vectors Conjugate()
        {
            int vectorLength = rows;
            Complex[] conjugateElements = new Complex[vectorLength];

            for (int i = 0; i < vectorLength; i++)
            {
                conjugateElements[i] = Complex.Conjugate(elements[i]);
            }

            return new Vectors(conjugateElements);
        }

        public Vectors Transpose()
        {
            
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