using System;
using System.Numerics;
using Quantum;
using NUnit.Framework;

//-------------------------------------------------------------------------------------------------------------------------------------------------------------
// This is the QuantumTest class where we store and run tests on our various implementations to see if they perform accurately and efficiently
// it has the following methods: InitializeQbit_WrongLength & _NotValid, Evolve_Success()
//                               QuantumTestUtilities (check each method for more detailed descriptions)
//-------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace QuantumTests
{
    [TestFixture]
    public class QuantumTests
    {

        [Test]
        public void InitializeQbit_WrongLength_ThrowsException()
        {
            // Arrange
            Complex[] testVector1 = { 1, 2, 3 };
            Complex[] testVector2 = { 1 };
            Complex[] testVector3 = { };
            Complex[] testVector4 = { 1, 2, 3, -100, 100 };

            // Assert
            Assert.Throws<ArgumentException>(() => new Qbit(testVector1));
            Assert.Throws<ArgumentException>(() => new Qbit(testVector2));
            Assert.Throws<ArgumentException>(() => new Qbit(testVector3));
            Assert.Throws<ArgumentException>(() => new Qbit(testVector4));

        }

        [Test]
        public void InitializeQbit_NotValid_ThrowsException()
        {
            // Arrange
            Complex[] testVector1 = { 1, 2 };
            Complex[] testVector2 = { 1, Complex.ImaginaryOne };
            Complex[] testVector3 = { -1, -1 * Complex.ImaginaryOne };
            Complex[] testVector4 = { Complex.ImaginaryOne, Complex.Sqrt(2) };

            // Assert
            Assert.Throws<ArgumentException>(() => new Qbit(testVector1));
            Assert.Throws<ArgumentException>(() => new Qbit(testVector2));
            Assert.Throws<ArgumentException>(() => new Qbit(testVector3));
            Assert.Throws<ArgumentException>(() => new Qbit(testVector3));

        }
        
        [Test]
        public void Evolve_Success()
        {
            // Arrange
            Complex[] testVector1 = { 1, 0 };
            Complex[] testVector2 = { 0, 1 };
            Complex[] testVector3 = { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) };

            Qbit q1 = new Qbit(testVector1);
            Qbit q2 = new Qbit(testVector2);
            Qbit q3 = new Qbit(testVector3);

            Complex[,] x = { { 0, 1 }, { 1, 0 } };
            Complex[,] y = { { 0, -1 * Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } };
            Complex[,] z = { { 1, 0 }, { 0, -1 } };
            Complex[,] h = { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) }, { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } };
            Complex[,] s = { { 1, 0 }, { 0, Complex.ImaginaryOne } };
            Complex[,] t = { { 1, 0 }, { 0, (1 + Complex.ImaginaryOne) / Complex.Sqrt(2) } };

            Operator X = new Operator(x);
            Operator Y = new Operator(y);
            Operator Z = new Operator(z);
            Operator H = new Operator(h);
            Operator S = new Operator(s);
            Operator T = new Operator(t);

            // Act
            q1.Evolve(X);
            q1.Evolve(Y);
            q1.Evolve(Z);

            q2.Evolve(H);
            q2.Evolve(S);
            q2.Evolve(T);

            q3.Evolve(H);
            q3.Evolve(S);
            q3.Evolve(T);

            Qbit q1_expected = new([-1 * Complex.ImaginaryOne, 0]);
            Qbit q2_expected = new([1 / Complex.Sqrt(2), 0.5 - 0.5 * Complex.ImaginaryOne]);
            Qbit q3_expected = new([1, 0]);

            QuantumTestUtilities.AssertThatQbitsAreEqual(q1, q1_expected);
            QuantumTestUtilities.AssertThatQbitsAreEqual(q2, q2_expected);
            QuantumTestUtilities.AssertThatQbitsAreEqual(q3, q3_expected); ;
            

        }

        public static class QuantumTestUtilities
        {
            public static void AssertThatQbitsAreEqual(Qbit actual,  Qbit expected)
            {
                Assert.That(actual.Data.rows, Is.EqualTo(expected.Data.rows), "Number of rows differs");
                for (int i = 0; i < actual.Data.elements.Length; i++)
                {
                    Complex actual_rounded = new(Math.Round(actual.Data.elements[i].Real, 6), Math.Round(actual.Data.elements[i].Imaginary, 6));
                    Complex expected_rounded = new(Math.Round(expected.Data.elements[i].Real, 6), Math.Round(expected.Data.elements[i].Imaginary, 6));
                    Assert.That(actual_rounded, Is.EqualTo(expected_rounded), $"Element at position ({i}) differs");
                }
            }
        }
        
    }
}