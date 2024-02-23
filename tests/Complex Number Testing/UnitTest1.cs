using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum_Computing;

namespace ComplexNumbersTests
{
    [TestClass]
    public class ComplexNumberTests
    {
        // Test the addition of two complex numbers
        [TestMethod]
        public void TestAddition()
        {
            // Arrange
            var num1 = new ComplexNumber(1, 2);
            var num2 = new ComplexNumber(3, 4);
            var expected = new ComplexNumber(4, 6); // Expected result of addition

            // Act
            var result = num1 + num2;

            // Assert
            Assert.AreEqual(expected, result, "Addition did not return the expected result.");
        }

        // Test the subtraction of two complex numbers
        [TestMethod]
        public void TestSubtraction()
        {
            // Arrange
            var num1 = new ComplexNumber(1, 2);
            var num2 = new ComplexNumber(3, 4);
            var expected = new ComplexNumber(-2, -2); // Expected result of subtraction

            // Act
            var result = num1 - num2;

            // Assert
            Assert.AreEqual(expected, result, "Subtraction did not return the expected result.");
        }

        // Test the multiplication of two complex numbers
        [TestMethod]
        public void TestMultiplication()
        {
            // Arrange
            var num1 = new ComplexNumber(1, 2);
            var num2 = new ComplexNumber(3, 4);
            var expected = new ComplexNumber(-5, 10); // Expected result of multiplication

            // Act
            var result = num1 * num2;

            // Assert
            Assert.AreEqual(expected, result, "Multiplication did not return the expected result.");
        }

        // Test the division of two complex numbers
        [TestMethod]
        public void TestDivision()
        {
            // Arrange
            var num1 = new ComplexNumber(1, 2);
            var num2 = new ComplexNumber(3, 4);
            var expected = new ComplexNumber(0.44, 0.08); // Expected result of division

            // Act
            var result = num1 / num2;

            // Assert
            Assert.AreEqual(expected, result, "Division did not return the expected result.");

            // Test division by zero using 
            var num3 = new ComplexNumber(); // Constructor with no parameters test as well
            var num4 = new ComplexNumber(0, 0);
            Assert.ThrowsException<DivideByZeroException>(() => num3 / num4, "Division did not return the expected result.");
        }

        // Test the equivalence of two complex numbers
        [TestMethod]
        public void TestEquality()
        {
            // Arrange
            var num1 = new ComplexNumber(1, 2);
            var num2 = new ComplexNumber(1, 2);

            // Act
            var result = num1 == num2;

            // Assert
            Assert.IsTrue(result, "Equality operator did not return the expected result.");

            // Test reflexivity
            Assert.IsTrue(num1 == num1, "Equality operator did not return the expected result.");

            // Test symmetry
            Assert.IsTrue(num1 == num2 && num2 == num1, "Equality operator did not return the expected result.");

            // Test transitivity
            var num3 = new ComplexNumber(1, 2);
            Assert.IsTrue(num1 == num2 && num2 == num3 && num1 == num3, "Equality operator did not return the expected result.");

            // Test null
            Assert.IsFalse(num1 == null, "Equality operator did not return the expected result.");
        }

        // Test the inequality of two complex numbers
        [TestMethod]
        public void TestInequality()
        {
            // Arrange
            var num1 = new ComplexNumber(1, 2);
            var num2 = new ComplexNumber(3, 4);

            // Act
            var result = num1 != num2;

            // Assert
            Assert.IsTrue(result, "Inequality operator did not return the expected result.");

            // Test reflexivity
            Assert.IsFalse(num1 != num1, "Inequality operator did not return the expected result.");

            // Test symmetry
            Assert.IsTrue(num1 != num2 && num2 != num1, "Inequality operator did not return the expected result.");

            // Test transitivity
            var num3 = new ComplexNumber(1, 3);
            Assert.IsTrue(num1 != num2 && num2 != num3 && num1 != num3, "Inequality operator did not return the expected result.");

            // Test null
            Assert.IsTrue(num1 != null, "Inequality operator did not return the expected result.");
        }

        // Test the unary negation of a complex number
        [TestMethod]
        public void TestUnaryNegation()
        {
            // Arrange
            var num = new ComplexNumber(1, 2);
            var expected = new ComplexNumber(-1, -2); // Expected result of unary negation

            // Act
            var result = -num;

            // Assert
            Assert.AreEqual(expected, result, "Unary negation did not return the expected result.");
        }

        // Test the increment operator for a complex number
        [TestMethod]
        public void TestIncrement()
        {
            // Arrange
            var num = new ComplexNumber(1, 2);
            var expected = new ComplexNumber(2, 2); // Expected result of increment

            // Act
            var result = ++num;

            // Assert
            Assert.AreEqual(expected, result, "Increment operator did not return the expected result.");
        }

        // Test the decrement operator for a complex number
        [TestMethod]
        public void TestDecrement()
        {
            // Arrange
            var num = new ComplexNumber(1, 2);
            var expected = new ComplexNumber(0, 2); // Expected result of decrement

            // Act
            var result = --num;

            // Assert
            Assert.AreEqual(expected, result, "Decrement operator did not return the expected result.");
        }

        // Test the ToString method for a complex number
        [TestMethod]
        public void TestToString()
        {
            // Arrange
            var num = new ComplexNumber(1, 2);
            var expected = "1 + 2i"; // Expected result of ToString

            // Act
            var result = num.ToString();

            // Assert
            Assert.AreEqual(expected, result, "ToString method did not return the expected result.");
        }

        // Test the Conjugate method for a complex number
        [TestMethod]
        public void TestConjugate()
        {
            // Arrange
            var num = new ComplexNumber(1, 2);
            var expected = new ComplexNumber(1, -2); // Expected result of Conjugate

            // Act
            var result = num.Conjugate();

            // Assert
            Assert.AreEqual(expected, result, "Conjugate method did not return the expected result.");
        }

        // Test the Modulus method for a complex number
        [TestMethod]
        public void TestModulus()
        {
            // Arrange
            var num = new ComplexNumber(3, 4);
            var expected = 5; // Expected result of Modulus

            // Act
            var result = num.Modulus();

            // Assert
            Assert.AreEqual(expected, result, "Modulus method did not return the expected result.");
        }

    }
}

