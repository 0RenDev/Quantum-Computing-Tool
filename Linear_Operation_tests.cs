using System;
using System.Numerics;
using LinearAlgebra;
using NUnit.Framework;

namespace Linear_Algebra_Testing
{
    // this class acts as test cases for the operations class. to run the tests click on "Test" on the top bar and click on "Run all Tests"

    [TestFixture]
    public class OperationsTests
    {
        [Test]
        public void Add_MatrixAddition_Success()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { new Complex(1, 2), new Complex(3, 4) }, { new Complex(5, 6), new Complex(7, 8) } });
            Matrix matrix2 = new Matrix(new Complex[,] { { new Complex(9, 10), new Complex(11, 12) }, { new Complex(13, 14), new Complex(15, 16) } });

            // Act
            Matrix resultMatrix = Operations.Add(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = { { new Complex(10, 12), new Complex(14, 16) }, { new Complex(18, 20), new Complex(22, 24) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }

        [Test]
        public void Multscaler_MatrixMultiplicationByScaler_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { new Complex(1, 2), new Complex(3, 4) }, { new Complex(5, 6), new Complex(7, 8) } });
            Complex scaler = new Complex(2, 3);

            // Act
            Matrix resultMatrix = Operations.Multscaler(matrix, scaler);

            // Assert
            Complex[,] expectedElements = { { new Complex(-4, 7), new Complex(-6, 17) }, { new Complex(-8, 27), new Complex(-10, 37) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }

        [Test]
        public void Multiply_VectorMatrix_Simple_ValidResult()
        {
            // Arrange
            Matrix vector = new Matrix(new Complex[,] { { 1 }, { 2 }, { 3 } });
            Matrix matrix = new Matrix(new Complex[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            // Act
            Matrix result = Operations.Multiply(matrix, vector);

            // Assert
            Complex[,] expectedElements = { { new Complex(14, 0) }, { new Complex(32, 0) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, result.elements);
        }

        [Test]
        public void Multiply_MatrixMatrix_Simple_ValidResult()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6 }, { 7, 8 } });

            // Act
            Matrix result = Operations.Multiply(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = { { new Complex(19, 0), new Complex(22, 0) }, { new Complex(43, 0), new Complex(50, 0) } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, result.elements);
        }

        [Test]
        public void Multiply_MatrixMatrix_InvalidDimensions_ThrowsException()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6, 7 }, { 8, 9, 10 } });

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Operations.Multiply(matrix2, matrix1));
        }

        [Test]
        public void TensorProduct_MatrixTensorProduct_Success()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6 }, { 7, 8 } });

            // Act
            Matrix resultMatrix = Operations.TensorProduct(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = { { 5, 6, 10, 12 }, { 7, 8, 14, 16 }, { 15, 18, 20, 24 }, { 21, 24, 28, 32 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }


        [Test]
        public void isEqual_MatricesEqual_ReturnsTrue()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });

            // Act
            bool result = Operations.isEqual(matrix1, matrix2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void isEqual_MatricesNotEqual_ReturnsFalse()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 5 } });

            // Act
            bool result = Operations.isEqual(matrix1, matrix2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MatrixMultiply_ValidMatrices_ReturnsCorrectResult()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6 }, { 7, 8 } });

            // Act
            Matrix resultMatrix = Operations.MatrixMultiply(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = { { 19, 22 }, { 43, 50 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }

        [Test]
        public void MatrixMultiply_InvalidDimensions_ThrowsException()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6, 7 }, { 8, 9, 10 } });

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Operations.MatrixMultiply(matrix2, matrix1));
        }
    }
}
