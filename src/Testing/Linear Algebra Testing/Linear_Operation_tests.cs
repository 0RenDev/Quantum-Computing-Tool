using System;
using System.Numerics;
using LinearAlgebra;
using NUnit.Framework;
using Vector = LinearAlgebra.Vector;


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
        public void TensorProduct_MatrixTensorProduct1_Success()
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
        public void TensorProduct_MatrixTensorProduct2_Success()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 0 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 1, 0 } });

            // Act
            Matrix resultMatrix = Operations.TensorProduct(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = { { 1, 0, 0, 0 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }

        [Test]
        public void TensorProduct_MatrixTensorProduct3_Success()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 0, 0, 0 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 1, 0} });

            // Act
            Matrix resultMatrix = Operations.TensorProduct(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = { { 1, 0, 0, 0, 0, 0, 0, 0 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }

        [Test]
        public void TensorProduct_LargeMatrices_Success()
        {
            // Arrange
            int size = 50; // Define a large size for the matrices
            Complex[,] elements1 = new Complex[size, size];
            Complex[,] elements2 = new Complex[size, size];

            // Initialize the matrices with some values
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    elements1[i, j] = new Complex(i + 1, j + 1);
                    elements2[i, j] = new Complex(j + 1, i + 1);
                }
            }

            Matrix matrix1 = new Matrix(elements1);
            Matrix matrix2 = new Matrix(elements2);

            // Act
            Matrix resultMatrix = Operations.TensorProduct(matrix1, matrix2);

            // Assert
            Complex[,] expectedElements = new Complex[size * size, size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int m = 0; m < size; m++)
                    {
                        for (int n = 0; n < size; n++)
                        {
                            expectedElements[i * size + m, j * size + n] = elements1[i, j] * elements2[m, n];
                        }
                    }
                }
            }

            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, resultMatrix.elements);
        }

        [Test]
        public void IsEqual_MatricesEqual_ReturnsTrue()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });

            // Act
            bool result = Operations.IsEqual(matrix1, matrix2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEqual_MatricesNotEqual_ReturnsFalse()
        {
            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 5 } });

            // Act
            bool result = Operations.IsEqual(matrix1, matrix2);

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

        [Test]
        public void MatrixDeterminant_Invalid_ThrowsException()
        {
            Matrix matrix = new Matrix(new Complex[,] { { 2 }, { 3 } });
            Assert.Throws<ArgumentException>(() => Operations.Determinant(matrix));
        }

        [Test]
        public void MatrixDeterminant_Success()
        {
            // Arrange
            Matrix matrix = new Matrix(new Complex[,] { { 1, 5, 5, 8 }, { 2, 3, 4, 5 }, { 4, 55, 2, 6 }, { 6, 6, 1, 1 } });
            int expected = 1659;


            // Assert
            Assert.That(Operations.Determinant(matrix), Is.EqualTo(expected));
        }

        [Test]
        public void IdentityMatrixGeneration_Success()
        {
            Matrix matrix = Operations.GenerateIdentityMatrix(3);
            // Arrange
            Complex[,] expectedElements = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expectedElements, matrix.elements);

        }

        [Test]
        public void InnerProduct_Success()
        {
            // Arrange 
            Vector vector1 = new([1, 2, 3, 4, 5]);
            Vector vector2 = new([6, 7, 8, 9, 10]);

            // Act
            Complex result = Operations.InnerProduct(vector1, vector2);

            // Assert
            Complex expected = 130;
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void OuterProduct_Success()
        {
            // Arrange
            Vector vector1 = new([1, 2, 3, 4, 5]);
            Vector vector2 = new([6, 7, 8, 9, 10]);

            // Act 
            Matrix result = Operations.OuterProduct(vector1, vector2);

            // Assert
            Complex[,] expected = new Complex[,] { { 6, 7, 8, 9, 10 }, { 12, 14, 16, 18, 20 }, { 18, 21, 24, 27, 30 }, { 24, 28, 32, 36, 40 }, { 30, 35, 40, 45, 50 } };
            MatrixTestUtilities.AssertMatrixAreEqual(expected, result.elements);
        }

        
    }
}

