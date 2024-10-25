using System;
using System.Numerics;
using LinearAlgebra;
using NUnit.Framework;

namespace Linear_Algebra_Testing
{
    // this class acts as test cases for the matrix class and its operations. to run the tests click on "Test" on the top bar and click on "Run all Tests"

    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void Conjugate_SquareMatrix_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { new Complex(1, -2), new Complex(3, 4) }, { new Complex(-1, 2), new Complex(-3, -4) } });

            // Act
            Matrix conjugateMatrix = matrix.Conjugate();

            // Assert
            Complex[,] expectedElements = { { new Complex(1, 2), new Complex(3, -4) }, { new Complex(-1, -2), new Complex(-3, 4) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, conjugateMatrix.elements);
        }

        [Test]
        public void Conjugate_RectangularMatrix_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { new Complex(1, -2), new Complex(3, 4), new Complex(5, -6) }, { new Complex(-1, 2), new Complex(-3, -4), new Complex(5, 6) } });

            // Act
            Matrix conjugateMatrix = matrix.Conjugate();

            // Assert
            Complex[,] expectedElements = { { new Complex(1, 2), new Complex(3, -4), new Complex(5, 6) }, { new Complex(-1, -2), new Complex(-3, 4), new Complex(5, -6) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, conjugateMatrix.elements);
        }

        [Test]
        public void Trace_SquareMatrix_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { new Complex(1, 2), new Complex(3, 4), new Complex(5, 6) },
                                               { new Complex(7, 8), new Complex(9, 10), new Complex(11, 12) },
                                               { new Complex(13, 14), new Complex(15, 16), new Complex(17, 18) } });

            // Act
            Complex traceValue = matrix.Trace();

            // Assert
            Complex expectedTrace = new Complex(27, 30); // 1 + 10 + 18
            Assert.That(traceValue, Is.EqualTo(expectedTrace));
        }


        [Test]
        public void Trace_NonSquareMatrix_ThrowsException()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => matrix.Trace());
        }

        [Test]
        public void Transpose_SquareMatrix_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { new Complex(1, 2), new Complex(3, 4), new Complex(5, 6) },
                                               { new Complex(7, 8), new Complex(9, 10), new Complex(11, 12) },
                                               { new Complex(13, 14), new Complex(15, 16), new Complex(17, 18) } });

            // Act
            Matrix transposedMatrix = matrix.Transpose();

            // Assert
            Complex[,] expectedElements = { { new Complex(1, 2), new Complex(7, 8), new Complex(13, 14) },
                                    { new Complex(3, 4), new Complex(9, 10), new Complex(15, 16) },
                                    { new Complex(5, 6), new Complex(11, 12), new Complex(17, 18) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, transposedMatrix.elements);
        }

        [Test]
        public void Transpose_RectangularMatrix_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            // Act
            Matrix transposedMatrix = matrix.Transpose();

            // Assert
            Complex[,] expectedElements = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, transposedMatrix.elements);
        }
    }


    // this class provides utilities to the testing classes
    public static class MatrixTestUtilities
    {
        // used to check if two matrices are equal
        public static void AssertMatrixAreEqual(Complex[,] expected, Complex[,] actual)
        {
            Assert.That(actual.GetLength(0), Is.EqualTo(expected.GetLength(0)), "Number of rows differs");
            Assert.That(actual.GetLength(1), Is.EqualTo(expected.GetLength(1)), "Number of columns differs");

            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    Assert.That(actual[i, j], Is.EqualTo(expected[i, j]), $"Element at position ({i}, {j}) differs");
                }
            }
        }
    }
}
