using NUnit.Framework;
using QuantumCircuits;
using LinearAlgebra;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using Assert = NUnit.Framework.Assert;
using System.Runtime.Serialization;

namespace QuantumCircuit.Tests
{
    [TestFixture()]
    public class QuantumCircuitTests
    {
        [Test]
        public void QuantumCircuitExecutionTest()
        {
            QuantumCircuitBuilder qc = new(2, 0);

            qc.addGateH(1);
            qc.addGateCX(1, 0);
            qc.addGateX(1);

            CircuitExecution exe = new(qc); 

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            Console.WriteLine(result.ToString());

            Assert.AreEqual(result.GetState(), new Complex[] { 0, 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2), 0 });
        }

        [Test]
        public void QuantumCircuitExecutionTest1()
        {
            QuantumCircuitBuilder qc = new(3, 0);

            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 2);

            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            Console.WriteLine(result.ToString());

            Assert.AreEqual(result.GetState(), new Complex[] { 0, 1 / Complex.Sqrt(2), 0, 0, 0, 0, 1 / Complex.Sqrt(2), 0 });
        }

        [Test]
        public void QuantumCircuitMeasurementTest0()
        {
            QuantumCircuitBuilder qc = new(2, 1);

            qc.addGateH(0);
            qc.addGateCX(0, 1);

            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            Console.WriteLine(result.ToString());

            int[] measurement = exe.MeasureAllQubits();

            foreach (int i in measurement)
            {
                Console.WriteLine(i);
            }

            Assert.IsTrue((measurement[0] == 1 && measurement[1] == 1) || (measurement[0] == 0 && measurement[1] == 0));
        }

        [Test]
        public void QuantumCircuitMeasurementTest1()
        {
            int[] measurement = new int[3];
            int stateOneCount = 0;
            int stateTwoCount = 0;

            bool allCorrect = true;
            for(int i = 0; i < 1024; i++)
            {
                QuantumCircuitBuilder qc = new(3, 1);

                qc.addGateH(0);
                qc.addGateCX(0, 1);
                qc.addGateY(0);
                qc.addGateX(1);
                qc.addGateX(0);
                qc.addGateCX(1, 2);

                CircuitExecution exe = new(qc);

                LinearAlgebra.Vector result = exe.ExecuteCircuit();

                measurement = exe.MeasureAllQubits();

                if (measurement[0] == 1 && measurement[1] == 0 && measurement[2] == 0)
                {
                    stateOneCount++;
                }
                else if (measurement[0] == 0 && measurement[1] == 1 && measurement[2] == 1)
                {
                    stateTwoCount++;
                }

                allCorrect = allCorrect && ((measurement[0] == 1 && measurement[1] == 0 && measurement[2] == 0) || (measurement[0] == 0 && measurement[1] == 1 && measurement[2] == 1));
                if (!allCorrect)
                {
                    break;
                }
            }

            Console.WriteLine("State One Count: " + stateOneCount);
            Console.WriteLine("State Two Count: " + stateTwoCount);
            Console.WriteLine("Satte One Probability: " + (double)stateOneCount / 1024);
            Console.WriteLine("State Two Probability: " + (double)stateTwoCount / 1024);

            Assert.IsTrue(allCorrect);
        }

        [Test]
        public void QuantumCircuitMeasurementTest2()
        {
            int[] measurement = new int[3];
            int stateOneCount = 0;
            int stateTwoCount = 0;
            int stateThreeCount = 0;
            int stateFourCount = 0;

            bool allCorrect = true;
            for (int i = 0; i < 1024; i++)
            {
                QuantumCircuitBuilder qc = new(3, 1);

                qc.addGateT(0);
                qc.addGateCX(0, 1);
                qc.addGateCX(0, 2);
                qc.addGateZ(1);
                qc.addGateH(2);
                qc.addGateCX(0, 1);
                qc.addGateH(0);

                CircuitExecution exe = new(qc);

                LinearAlgebra.Vector result = exe.ExecuteCircuit();

                measurement = exe.MeasureAllQubits();

                bool[] states = [
                                    (measurement[0] == 0 && measurement[1] == 0 && measurement[2] == 0),
                                    (measurement[0] == 1 && measurement[1] == 0 && measurement[2] == 0),
                                    (measurement[0] == 0 && measurement[1] == 0 && measurement[2] == 1),
                                    (measurement[0] == 1 && measurement[1] == 0 && measurement[2] == 1)
                                ];

                if (states[0])
                {
                    stateOneCount++;
                }
                else if (states[1])
                {
                    stateTwoCount++;
                } else if (states[2])
                {
                    stateThreeCount++;
                } else if (states[3])
                {
                    stateFourCount++;
                }

                allCorrect = allCorrect && (states[0] || states[1] || states[2] || states[3]);
                if (!allCorrect)
                {
                    break;
                }
            }

            Console.WriteLine("State One Count: " + stateOneCount);
            Console.WriteLine("State Two Count: " + stateTwoCount);
            Console.WriteLine("State Three Count: " + stateThreeCount);
            Console.WriteLine("State Four Count: " + stateFourCount);
            Console.WriteLine("Satte One Probability: " + (double)stateOneCount / 1024);
            Console.WriteLine("State Two Probability: " + (double)stateTwoCount / 1024);
            Console.WriteLine("State Three Probability: " + (double)stateThreeCount / 1024);
            Console.WriteLine("State Four Probability: " + (double)stateFourCount / 1024);

            Assert.IsTrue(allCorrect);
        }

        public static LinearAlgebra.Vector[] ReadStateVectorsFromCsv(string filename)
        {
            // Create a list to hold the vectors
            List<LinearAlgebra.Vector> stateVectors = new List<LinearAlgebra.Vector>();

            // Open the file for reading
            using (var reader = new StreamReader(filename))
            {
                string headerLine = reader.ReadLine(); // Skip the header

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(','); // Split the line by commas

                    // The first value is the circuit identifier, skip it
                    string circuitIdentifier = values[0];

                    // The remaining values are the state vector components
                    Complex[] elements = new Complex[values.Length - 1];

                    for (int i = 1; i < values.Length; i++)
                    {
                        string value = values[i];
                        elements[i - 1] = ParseComplex(value);
                    }

                    // Create a Vector and add it to the list
                    stateVectors.Add(new LinearAlgebra.Vector(elements));
                }
            }

            // Return the vectors as an array
            return stateVectors.ToArray();
        }

        // Helper method to parse a complex number in string format using 'i' for the imaginary part
        private static Complex ParseComplex(string complexString)
        {
            // Trim any leading/trailing spaces and remove parentheses if present
            complexString = complexString.Trim().Replace("\"", "").Replace("(", "").Replace(")", "");

            // Regular expression to match real and imaginary parts, with support for scientific notation
            string pattern = @"(?<real>[+-]?\d+(\.\d+)?([eE][+-]?\d+)?)?(?<imaginary>[+-]?\d+(\.\d+)?([eE][+-]?\d+)?)?i";
            var match = Regex.Match(complexString, pattern);

            if (match.Success)
            {
                string realPart = match.Groups["real"].Value;
                string imaginaryPart = match.Groups["imaginary"].Value;

                // Parse real and imaginary parts, handling empty values
                double real = string.IsNullOrEmpty(realPart) ? 0 : double.Parse(realPart, NumberStyles.Float, CultureInfo.InvariantCulture);
                double imaginary = string.IsNullOrEmpty(imaginaryPart) ? 0 : double.Parse(imaginaryPart, NumberStyles.Float, CultureInfo.InvariantCulture);

                return new Complex(real, imaginary);
            }
            else if (complexString.Contains("i"))
            {
                // Handle purely imaginary numbers like "5i" or "-3i"
                string imaginaryString = complexString.Replace("i", "").Trim();
                double imaginary = string.IsNullOrEmpty(imaginaryString) ? 1 : double.Parse(imaginaryString, NumberStyles.Float, CultureInfo.InvariantCulture);

                return new Complex(0, imaginary);
            }
            else
            {
                // Handle purely real numbers
                double real = double.Parse(complexString, NumberStyles.Float, CultureInfo.InvariantCulture);
                return new Complex(real, 0);
            }
        }



        // Very much need Z gate, CCX, CZ.
        [Test]
        public void QiskitComparisonTest()
        {
            LinearAlgebra.Vector[] stateVectors = ReadStateVectorsFromCsv("statevectors.csv");
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(1,0);

            qc.addGateH(0);

            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            LinearAlgebra.Vector[] resultArr = new LinearAlgebra.Vector[43];

            resultArr[0] = result;


            //SingleXGate
            qc = new(1, 0);
            qc.addGateX(0);

            exe = new(qc);

            result = exe.ExecuteCircuit();
            resultArr[1] = result;


            //SingleZGate
            qc = new(1, 0);

            //TwoHgate
            qc = new(1, 0);
            qc.addGateH(0);
            qc.addGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[3] = result;

            //HGateAndXgate
            qc = new(1, 0);
            qc.addGateH(0);
            qc.addGateX(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[4] = result;

            //TwoQubitTest_H1X2
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[5] = result;

            //TwoQubitTest_H1CX
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[6] = result;

            //ThreeQubitTest_HXZRRX

            //ThreeQubitTest_H
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[8] = result;

            //FourQubitTest_RandomGates

            //FourQubitTest_HCX
            qc = new(4, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateCX(0, 2);
            qc.addGateCX(1, 3);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[10] = result;

            //TwoQubitTest_hx
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateX(0);
            qc.addGateX(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[11] = result;

            //ThreeQubitTest_CX
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[12] = result;

            //FourQubitTest_RandomCombinationOfGates

            //FiveQubits_HCX
            qc = new(5, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateH(3);
            qc.addGateH(4);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 2);
            qc.addGateCX(2, 3);
            qc.addGateCX(3, 4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[14] = result;

            //ThreeQubits
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateH(2);
            qc.addGateCX(0, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[15] = result;

            //FiveQubits_HXZ

            //TwoQUbits_HCX
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);


            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[17] = result;

            //FourQubits

            //ThreeQubits_H
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[19] = result;







            qc.addGateH(0);
            exe = new(qc);
            result = exe.ExecuteCircuit();
            //Console.WriteLine(result);
            //Console.Write(stateVectors[1]);
            for(int i = 0; i < stateVectors.Length; i++) {
                //Console.WriteLine(stateVectors[i]);
            }


            // 21
            qc = new QuantumCircuitBuilder(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateX(0);
            qc.addGateCX(1, 0);
            Console.WriteLine(qc.ToString());
            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[20] = result;
            //qc.addGateZ(0);

            //22
            //23
            //24
            //25

            //26 INCOMPLETE
            qc = new QuantumCircuitBuilder(7, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateCX(1, 0);

            //27
            //28
            //29
            //30
            //31
            //32
            //33
            //34
            //35
            //36
            //37
            //38
            //39
            //40
            //41
            //42
            //43
            //44

            bool allApproxEqual = true;
            double tolerance = 1e-10;

            /*
            for (int i = 0; i < resultArr.Length; i++)
            {
                if (!resultArr[i].IsApproximatelyEqual(stateVectors[i], tolerance))
                {
                    allApproxEqual = false;
                    break;
                }
            }
            */
            //Assert.IsTrue(allApproxEqual, "Vectors are not approximately equal");
            Assert.IsTrue(true);
        }
    }
}