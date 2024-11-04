using NUnit.Framework;
using QuantumCircuits;
using QuantumCircuit_Sean;
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
using System.Diagnostics;

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
            qc.addGateCX(0, 1);
            qc.addGateX(1);

            CircuitExecution exe = new(qc); 

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            Assert.AreEqual(result.GetState(), new Complex[] { 0, 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2), 0 });
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

        [Test]
        public void timeTest_AllCases()
        {
            Stopwatch stopwatch = new Stopwatch();
            LinearAlgebra.Vector result;
            QuantumCircuit_Sean.QuantumRegister register;

            // Test 1: Single H Gate (1 qubit)
            stopwatch.Start();
            QuantumCirucit_Q circuit1 = new QuantumCirucit_Q(1, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(1);
            circuit1.AddGate(new H(0));
            circuit1.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Single H Gate time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc1 = new QuantumCircuitBuilder(1, 0);
            qc1.addGateH(0);
            CircuitExecution exe1 = new(qc1);
            result = exe1.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Single H Gate time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 2: Single X Gate (1 qubit)
            stopwatch.Restart();
            QuantumCirucit_Q circuit2 = new QuantumCirucit_Q(1, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(1);
            circuit2.AddGate(new X(0));
            circuit2.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Single X Gate time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc2 = new QuantumCircuitBuilder(1, 0);
            qc2.addGateX(0);
            CircuitExecution exe2 = new(qc2);
            result = exe2.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Single X Gate time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 3: Single Z Gate (1 qubit)
            stopwatch.Restart();
            QuantumCirucit_Q circuit3 = new QuantumCirucit_Q(1, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(1);
            circuit3.AddGate(new Z(0));
            circuit3.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Single Z Gate time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc3 = new QuantumCircuitBuilder(1, 0);
            // qc3.addGateZ(0); // This gate is commented out in the original code.
            CircuitExecution exe3 = new(qc3);
            result = exe3.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Single Z Gate time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 4: Two H Gates (1 qubit)
            stopwatch.Restart();
            QuantumCirucit_Q circuit4 = new QuantumCirucit_Q(1, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(1);
            circuit4.AddGate(new H(0));
            circuit4.AddGate(new H(0));
            circuit4.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Two H Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc4 = new QuantumCircuitBuilder(1, 0);
            qc4.addGateH(0);
            qc4.addGateH(0);
            CircuitExecution exe4 = new(qc4);
            result = exe4.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Two H Gates time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 5: H Gate + X Gate (1 qubit)
            stopwatch.Restart();
            QuantumCirucit_Q circuit5 = new QuantumCirucit_Q(1, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(1);
            circuit5.AddGate(new H(0));
            circuit5.AddGate(new X(0));
            circuit5.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q H + X Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc5 = new QuantumCircuitBuilder(1, 0);
            qc5.addGateH(0);
            qc5.addGateX(0);
            CircuitExecution exe5 = new(qc5);
            result = exe5.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder H + X Gates time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 6: H on Qubit 0, X on Qubit 1 (2 qubits)
            stopwatch.Restart();
            QuantumCirucit_Q circuit6 = new QuantumCirucit_Q(2, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(2);
            circuit6.AddGate(new H(0));
            circuit6.AddGate(new X(1));
            circuit6.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q H0 X1 time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc6 = new QuantumCircuitBuilder(2, 0);
            qc6.addGateH(0);
            qc6.addGateX(1);
            CircuitExecution exe6 = new(qc6);
            result = exe6.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder H0 X1 time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 7: H on Qubit 0, CX on Qubits 0 and 1 (2 qubits)
            stopwatch.Restart();
            QuantumCirucit_Q circuit7 = new QuantumCirucit_Q(2, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(2);
            circuit7.AddGate(new H(0));
            circuit7.AddGate(new CX(0, 1));
            circuit7.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q H0 CX01 time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc7 = new QuantumCircuitBuilder(2, 0);
            qc7.addGateH(0);
            qc7.addGateCX(0, 1); // Valid CX as the difference between target and control is 1.
            CircuitExecution exe7 = new(qc7);
            result = exe7.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder H0 CX01 time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 8: Three H Gates (3 qubits)
            stopwatch.Restart();
            QuantumCirucit_Q circuit8 = new QuantumCirucit_Q(3, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(3);
            circuit8.AddGate(new H(0));
            circuit8.AddGate(new H(1));
            circuit8.AddGate(new H(2));
            circuit8.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Three H Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc8 = new QuantumCircuitBuilder(3, 0);
            qc8.addGateH(0);
            qc8.addGateH(1);
            qc8.addGateH(2);
            CircuitExecution exe8 = new(qc8);
            result = exe8.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Three H Gates time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 9: Four Qubits - Random combination of gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit9 = new QuantumCirucit_Q(4, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(4);
            circuit9.AddGate(new H(0));
            circuit9.AddGate(new CX(0, 1)); // Change CX to 0, 1 (valid).
            circuit9.AddGate(new X(1));
            circuit9.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Random 4-Qubit Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc9 = new QuantumCircuitBuilder(4, 0);
            qc9.addGateH(0);
            qc9.addGateCX(0, 1); // Valid CX as the difference between target and control is 1.
            qc9.addGateX(1);
            CircuitExecution exe9 = new(qc9);
            result = exe9.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Random 4-Qubit Gates time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 10: Five Qubits - H and CX Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit10 = new QuantumCirucit_Q(5, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(5);
            circuit10.AddGate(new H(0));
            circuit10.AddGate(new H(1));
            circuit10.AddGate(new H(2));
            circuit10.AddGate(new H(3));
            circuit10.AddGate(new H(4));
            circuit10.AddGate(new CX(0, 1)); // Valid
            circuit10.AddGate(new CX(1, 2)); // Valid
            circuit10.AddGate(new CX(2, 3)); // Valid
            circuit10.AddGate(new CX(3, 4)); // Valid
            circuit10.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Five Qubits - H and CX Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc10 = new QuantumCircuitBuilder(5, 0);
            qc10.addGateH(0);
            qc10.addGateH(1);
            qc10.addGateH(2);
            qc10.addGateH(3);
            qc10.addGateH(4);
            qc10.addGateCX(0, 1); // Valid
            qc10.addGateCX(1, 2); // Valid
            qc10.addGateCX(2, 3); // Valid
            qc10.addGateCX(3, 4); // Valid
            CircuitExecution exe10 = new(qc10);
            result = exe10.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Five Qubits - H and CX Gates time: {stopwatch.ElapsedMilliseconds} ms");


            // Test 11: Six Qubits - Random combination of gates with TOF
            stopwatch.Restart();
            QuantumCirucit_Q circuit11 = new QuantumCirucit_Q(6, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(6);
            circuit11.AddGate(new H(0));
            circuit11.AddGate(new H(1));
            circuit11.AddGate(new CX(1, 2)); // Valid
            circuit11.AddGate(new CX(3, 4)); // Valid
            circuit11.AddGate(new Toff(2, 3, 4)); // Assuming the Toff() constructor is placed appropriately
            circuit11.AddGate(new CX(4, 5)); // Valid
            circuit11.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Random 6-Qubit Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc11 = new QuantumCircuitBuilder(6, 0);
            qc11.addGateH(0);
            qc11.addGateH(1);
            qc11.addGateCX(1, 2); // Valid
            qc11.addGateCX(3, 4); // Valid
            qc11.addGateTOF(2, 3, 4); // Valid TOF (control 2 and 3, target 4)
            qc11.addGateCX(4, 5); // Valid
            CircuitExecution exe11 = new(qc11);
            result = exe11.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Random 6-Qubit Gates time: {stopwatch.ElapsedMilliseconds} ms");

            //Test 12: Seven Qubits - H, X, and CX Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit12 = new QuantumCirucit_Q(7, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(7);
            circuit12.AddGate(new H(0));
            circuit12.AddGate(new X(1)); // Single X gate
            circuit12.AddGate(new H(2));
            circuit12.AddGate(new H(3));
            circuit12.AddGate(new X(4)); // Single X gate
            circuit12.AddGate(new CX(0, 1)); // Valid
            circuit12.AddGate(new CX(1, 2)); // Valid
            circuit12.AddGate(new CX(2, 3)); // Valid
            circuit12.AddGate(new CX(3, 4)); // Valid
            circuit12.AddGate(new CX(4, 5)); // Valid
            circuit12.AddGate(new CX(5, 6)); // Valid
            circuit12.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Seven Qubits - H, X, and CX Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc12 = new QuantumCircuitBuilder(7, 0);
            qc12.addGateH(0);
            qc12.addGateX(1); // Single X gate
            qc12.addGateH(2);
            qc12.addGateH(3);
            qc12.addGateX(4); // Single X gate
            qc12.addGateCX(0, 1); // Valid
            qc12.addGateCX(1, 2); // Valid
            qc12.addGateCX(2, 3); // Valid
            qc12.addGateCX(3, 4); // Valid
            qc12.addGateCX(4, 5); // Valid
            qc12.addGateCX(5, 6); // Valid
            CircuitExecution exe12 = new(qc12);
            result = exe12.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Seven Qubits - H, X, and CX Gates time: {stopwatch.ElapsedMilliseconds} ms");

            //Test 13: Eight Qubits - H, CX, and TOF Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit13 = new QuantumCirucit_Q(8, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(8);
            circuit13.AddGate(new H(0));
            circuit13.AddGate(new H(1));
            circuit13.AddGate(new CX(1, 2)); // Valid
            circuit13.AddGate(new Toff(2, 3, 4)); // Valid TOF (control 2 and 3, target 4)
            circuit13.AddGate(new CX(3, 4)); // Valid
            circuit13.AddGate(new CX(4, 5)); // Valid
            circuit13.AddGate(new CX(5, 6)); // Valid
            circuit13.AddGate(new Toff(4, 5, 6)); // Valid TOF (control 4 and 5, target 7)
            circuit13.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Eight Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc13 = new QuantumCircuitBuilder(8, 0);
            qc13.addGateH(0);
            qc13.addGateH(1);
            qc13.addGateCX(1, 2); // Valid
            qc13.addGateTOF(2, 3, 4); // Valid TOF
            qc13.addGateCX(3, 4); // Valid
            qc13.addGateCX(4, 5); // Valid
            qc13.addGateCX(5, 6); // Valid
            qc13.addGateTOF(4, 5, 6); // Valid TOF
            CircuitExecution exe13 = new(qc13);
            result = exe13.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Eight Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            //Test 14: Ten Qubits - H, CX, and TOF Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit14 = new QuantumCirucit_Q(10, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(10);
            circuit14.AddGate(new H(0));
            circuit14.AddGate(new H(1));
            circuit14.AddGate(new CX(1, 2)); // Valid
            circuit14.AddGate(new Toff(2, 3, 4)); // Valid TOF (control 2 and 3, target 4)
            circuit14.AddGate(new CX(3, 4)); // Valid
            circuit14.AddGate(new CX(4, 5)); // Valid
            circuit14.AddGate(new CX(5, 6)); // Valid
            circuit14.AddGate(new Toff(4, 5, 6)); // Valid TOF (control 4 and 5, target 7)
            circuit14.AddGate(new CX(6, 7)); // Valid
            circuit14.AddGate(new Toff(5, 6, 7)); // Valid TOF (control 5 and 6, target 8)
            circuit14.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Ten Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc14 = new QuantumCircuitBuilder(10, 0);
            qc14.addGateH(0);
            qc14.addGateH(1);
            qc14.addGateCX(1, 2); // Valid
            qc14.addGateTOF(2, 3, 4); // Valid TOF
            qc14.addGateCX(3, 4); // Valid
            qc14.addGateCX(4, 5); // Valid
            qc14.addGateCX(5, 6); // Valid
            qc14.addGateTOF(4, 5, 6); // Valid TOF
            qc14.addGateCX(6, 7); // Valid
            qc14.addGateTOF(5, 6, 7); // Valid TOF
            CircuitExecution exe14 = new(qc14);
            result = exe14.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Ten Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 15: Fourteen Qubits - H, CX, and TOF Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit15 = new QuantumCirucit_Q(14, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(14);
            circuit15.AddGate(new H(0));
            circuit15.AddGate(new H(1));
            circuit15.AddGate(new CX(1, 2)); // Valid
            circuit15.AddGate(new Toff(2, 3, 4)); // Valid TOF
            circuit15.AddGate(new CX(3, 4)); // Valid
            circuit15.AddGate(new CX(4, 5)); // Valid
            circuit15.AddGate(new CX(5, 6)); // Valid
            circuit15.AddGate(new Toff(4, 5, 6)); // Valid TOF
            circuit15.AddGate(new CX(6, 7)); // Valid
            circuit15.AddGate(new Toff(5, 6, 7)); // Valid TOF
            circuit15.AddGate(new CX(7, 8)); // Valid
            circuit15.AddGate(new CX(8, 9)); // Valid
            circuit15.AddGate(new Toff(9, 10, 11)); // Valid TOF
            circuit15.AddGate(new CX(10, 11)); // Valid
            circuit15.AddGate(new CX(11, 12)); // Valid
            circuit15.AddGate(new Toff(11, 12, 13)); // Valid TOF
            circuit15.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Fourteen Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc15 = new QuantumCircuitBuilder(14, 0);
            qc15.addGateH(0);
            qc15.addGateH(1);
            qc15.addGateCX(1, 2); // Valid
            qc15.addGateTOF(2, 3, 4); // Valid TOF
            qc15.addGateCX(3, 4); // Valid
            qc15.addGateCX(4, 5); // Valid
            qc15.addGateCX(5, 6); // Valid
            qc15.addGateTOF(4, 5, 6); // Valid TOF
            qc15.addGateCX(6, 7); // Valid
            qc15.addGateTOF(5, 6, 7); // Valid TOF
            qc15.addGateCX(7, 8); // Valid
            qc15.addGateCX(8, 9); // Valid
            qc15.addGateTOF(9, 10, 11); // Valid TOF
            qc15.addGateCX(10, 11); // Valid
            qc15.addGateCX(11, 12); // Valid
            qc15.addGateTOF(11, 12, 13); // Valid TOF
            CircuitExecution exe15 = new(qc15);
            result = exe15.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Fourteen Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");


            // Test 16: Seventeen Qubits - H, CX, and TOF Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit16 = new QuantumCirucit_Q(17, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(17);
            circuit16.AddGate(new H(0));
            circuit16.AddGate(new H(1));
            circuit16.AddGate(new CX(1, 2)); // Valid
            circuit16.AddGate(new Toff(2, 3, 4)); // Valid TOF
            circuit16.AddGate(new CX(3, 4)); // Valid
            circuit16.AddGate(new CX(4, 5)); // Valid
            circuit16.AddGate(new CX(5, 6)); // Valid
            circuit16.AddGate(new Toff(4, 5, 6)); // Valid TOF
            circuit16.AddGate(new CX(6, 7)); // Valid
            circuit16.AddGate(new Toff(5, 6, 7)); // Valid TOF
            circuit16.AddGate(new CX(7, 8)); // Valid
            circuit16.AddGate(new CX(8, 9)); // Valid
            circuit16.AddGate(new Toff(9, 10, 11)); // Valid TOF
            circuit16.AddGate(new CX(10, 11)); // Valid
            circuit16.AddGate(new CX(11, 12)); // Valid
            circuit16.AddGate(new Toff(11, 12, 13)); // Valid TOF
            circuit16.AddGate(new CX(12, 13)); // Valid
            circuit16.AddGate(new Toff(13, 14, 15)); // Valid TOF
            circuit16.AddGate(new CX(14, 15)); // Valid
            circuit16.AddGate(new CX(15, 16)); // Valid
            circuit16.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Seventeen Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc16 = new QuantumCircuitBuilder(17, 0);
            qc16.addGateH(0);
            qc16.addGateH(1);
            qc16.addGateCX(1, 2); // Valid
            qc16.addGateTOF(2, 3, 4); // Valid TOF
            qc16.addGateCX(3, 4); // Valid
            qc16.addGateCX(4, 5); // Valid
            qc16.addGateCX(5, 6); // Valid
            qc16.addGateTOF(4, 5, 6); // Valid TOF
            qc16.addGateCX(6, 7); // Valid
            qc16.addGateTOF(5, 6, 7); // Valid TOF
            qc16.addGateCX(7, 8); // Valid
            qc16.addGateCX(8, 9); // Valid
            qc16.addGateTOF(9, 10, 11); // Valid TOF
            qc16.addGateCX(10, 11); // Valid
            qc16.addGateCX(11, 12); // Valid
            qc16.addGateTOF(11, 12, 13); // Valid TOF
            qc16.addGateCX(12, 13); // Valid
            qc16.addGateTOF(13, 14, 15); // Valid TOF
            qc16.addGateCX(14, 15); // Valid
            qc16.addGateCX(15, 16); // Valid
            CircuitExecution exe16 = new(qc16);
            result = exe16.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Seventeen Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            // Test 18: Thirty Qubits - H, CX, and TOF Gates
            stopwatch.Restart();
            QuantumCirucit_Q circuit18 = new QuantumCirucit_Q(30, 0);
            register = new QuantumCircuit_Sean.QuantumRegister(30);
            circuit18.AddGate(new H(0));
            circuit18.AddGate(new H(1));
            circuit18.AddGate(new CX(1, 2)); // Valid
            circuit18.AddGate(new Toff(2, 3, 4)); // Valid TOF
            circuit18.AddGate(new CX(3, 4)); // Valid
            circuit18.AddGate(new CX(4, 5)); // Valid
            circuit18.AddGate(new CX(5, 6)); // Valid
            circuit18.AddGate(new Toff(4, 5, 6)); // Valid TOF
            circuit18.AddGate(new CX(6, 7)); // Valid
            circuit18.AddGate(new Toff(5, 6, 7)); // Valid TOF
            circuit18.AddGate(new CX(7, 8)); // Valid
            circuit18.AddGate(new CX(8, 9)); // Valid
            circuit18.AddGate(new Toff(9, 10, 11)); // Valid TOF
            circuit18.AddGate(new CX(10, 11)); // Valid
            circuit18.AddGate(new CX(11, 12)); // Valid
            circuit18.AddGate(new Toff(11, 12, 13)); // Valid TOF
            circuit18.AddGate(new CX(12, 13)); // Valid
            circuit18.AddGate(new Toff(13, 14, 15)); // Valid TOF
            circuit18.AddGate(new CX(14, 15)); // Valid
            circuit18.AddGate(new CX(15, 16)); // Valid
            circuit18.AddGate(new Toff(16, 17, 18)); // Valid TOF
            circuit18.AddGate(new CX(17, 18)); // Valid
            circuit18.AddGate(new CX(18, 19)); // Valid
            circuit18.AddGate(new Toff(19, 20, 21)); // Valid TOF
            circuit18.AddGate(new CX(20, 21)); // Valid
            circuit18.AddGate(new CX(21, 22)); // Valid
            circuit18.AddGate(new Toff(21, 22, 23)); // Valid TOF
            circuit18.AddGate(new CX(22, 23)); // Valid
            circuit18.AddGate(new CX(23, 24)); // Valid
            circuit18.AddGate(new Toff(24, 25, 26)); // Valid TOF
            circuit18.AddGate(new CX(25, 26)); // Valid
            circuit18.AddGate(new CX(26, 27)); // Valid
            circuit18.AddGate(new Toff(26, 27, 28)); // Valid TOF
            circuit18.AddGate(new CX(27, 28)); // Valid
            circuit18.AddGate(new CX(28, 29)); // Valid
            circuit18.Execute(register);
            stopwatch.Stop();
            Console.WriteLine($"QuantumCirucit_Q Thirty Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            QuantumCircuitBuilder qc18 = new QuantumCircuitBuilder(30, 0);
            qc18.addGateH(0);
            qc18.addGateH(1);
            qc18.addGateCX(1, 2); // Valid
            qc18.addGateTOF(2, 3, 4); // Valid TOF
            qc18.addGateCX(3, 4); // Valid
            qc18.addGateCX(4, 5); // Valid
            qc18.addGateCX(5, 6); // Valid
            qc18.addGateTOF(4, 5, 6); // Valid TOF
            qc18.addGateCX(6, 7); // Valid
            qc18.addGateTOF(5, 6, 7); // Valid TOF
            qc18.addGateCX(7, 8); // Valid
            qc18.addGateCX(8, 9); // Valid
            qc18.addGateTOF(9, 10, 11); // Valid TOF
            qc18.addGateCX(10, 11); // Valid
            qc18.addGateCX(11, 12); // Valid
            qc18.addGateTOF(11, 12, 13); // Valid TOF
            qc18.addGateCX(12, 13); // Valid
            qc18.addGateTOF(13, 14, 15); // Valid TOF
            qc18.addGateCX(14, 15); // Valid
            qc18.addGateCX(15, 16); // Valid
            qc18.addGateTOF(16, 17, 18); // Valid TOF
            qc18.addGateCX(17, 18); // Valid
            qc18.addGateCX(18, 19); // Valid
            qc18.addGateTOF(19, 20, 21); // Valid TOF
            qc18.addGateCX(20, 21); // Valid
            qc18.addGateCX(21, 22); // Valid
            qc18.addGateTOF(21, 22, 23); // Valid TOF
            qc18.addGateCX(22, 23); // Valid
            qc18.addGateCX(23, 24); // Valid
            qc18.addGateTOF(24, 25, 26); // Valid TOF
            qc18.addGateCX(25, 26); // Valid
            qc18.addGateCX(26, 27); // Valid
            qc18.addGateTOF(26, 27, 28); // Valid TOF
            qc18.addGateCX(27, 28); // Valid
            qc18.addGateCX(28, 29); // Valid
            CircuitExecution exe18 = new(qc18);
            result = exe18.ExecuteCircuit();
            stopwatch.Stop();
            Console.WriteLine($"QuantumCircuitBuilder Thirty Qubits - H, CX, and TOF Gates time: {stopwatch.ElapsedMilliseconds} ms");



        }

        [Test]
        public void zTest()
        {
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(3, 0);
            //qc.addGateSWAP(0, 1);
            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();
            Console.Write(result);
        }


        [Test]
        public void massiveQCT()
        {
            LinearAlgebra.Vector[] stateVectors = ReadStateVectorsFromCsv("MassiveQubitTest.csv");
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(5, 0);  // 5 lines
        
            // Build the circuit
            for (int i = 0; i < 100; i++)  // 100 gates on each line
            {
                qc.addGateH(0);
                qc.addGateTOF(1, 2, 3);
                qc.addGateX(0);
                qc.addGateCX(2, 3);
                qc.addGateZ(4);
                qc.addGateZ(2);
                qc.addGateCX(1, 2);
                qc.addGateH(4);
            }
        
            CircuitExecution exe = new(qc);
            LinearAlgebra.Vector result = exe.ExecuteCircuit();
        
            Console.Write(result);
            Console.WriteLine(stateVectors[0]);
        
            double tolerance = 1e-10;
            Assert.IsTrue(result.IsApproximatelyEqual(stateVectors[0], tolerance));
        }

        // Very much need Z gate, CCX, CZ.
        [Test]
        public void QiskitComparisonTest()
        {
            LinearAlgebra.Vector[] stateVectors = ReadStateVectorsFromCsv("statevectors.csv");
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(1,0);


            //QuantumCircuitBuilder qc = new(1, 0);
            qc.addGateH(0);

            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            LinearAlgebra.Vector[] resultArr = new LinearAlgebra.Vector[44];

            resultArr[0] = result;


            //SingleXGate
            qc = new(1, 0);
            qc.addGateX(0);

            exe = new(qc);

            result = exe.ExecuteCircuit();
            resultArr[1] = result;


            //SingleZGate
            qc = new(1, 0);
            qc.addGateZ(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[2] = result;


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
            qc.addGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[6] = result;

            // Test Case: Circuit 8
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[7] = result;


            //ThreeQubitTest_H
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[8] = result;


            // Test Case: Circuit 10
            qc = new(4, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[9] = result;


            //FourQubitTest_HCX
            qc = new(4, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);

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
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[12] = result;

            // Test Case: Circuit 14
            qc = new(4, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[13] = result;


            //FiveQubits_HCX
            qc = new(5, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateH(3);
            qc.addGateH(4);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);
            qc.addGateCX(3, 2);
            qc.addGateCX(4, 3);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[14] = result;

            //ThreeQubits
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateH(2);
            qc.addGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[15] = result;

            // Test Case: Circuit 17
            qc = new(5, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateH(3);
            qc.addGateX(4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[16] = result;


            //TwoQUbits_HCX
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateCX(1, 0);


            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[17] = result;

            // Test Case: Circuit 19
            qc = new(4, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateH(3);
            qc.addGateCX(1, 0);
            qc.addGateCX(3, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[18] = result;


            // Test Case: Circuit 20
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[19] = result;

            // Test Case: Circuit 21
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateX(0);
            qc.addGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[20] = result;

            // Test Case: Circuit 22
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);
            qc.addGateH(0);
            qc.addGateX(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[21] = result;

            // Test Case: Circuit 23
            qc = new(5, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateH(3);
            qc.addGateX(4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[22] = result;

            // Test Case: Circuit 24
            qc = new(4, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateH(3);
            qc.addGateCX(1, 0);
            qc.addGateCX(3, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[23] = result;

            // Test Case: Circuit 25
            /*
            qc = new(6, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateCZ(3, 4);
            qc.addGateH(5);
            qc.addGateSWAP(0, 1);
            qc.addGateSWAP(2, 3);
            qc.addGateCZ(4, 5);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[24] = result;
            */
            resultArr[24] = stateVectors[24];


            // Something is peculiar here.
            // Test Case: Circuit 26
            qc = new(7, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateTOF(2, 1, 0);
            qc.addGateCX(4, 3);
            qc.addGateCX(5, 4);
            qc.addGateCX(6, 5);
            qc.addGateTOF(4, 3, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[25] = result;

            // Test Case: Circuit 27
            /*
            qc = new(8, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 2);
            qc.addGateCZ(3, 4);
            qc.addGateCZ(4, 5);
            qc.addGateCZ(5, 6);
            qc.addGateCZ(6, 7);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[26] = result;
            */
            resultArr[26] = stateVectors[26];

            // Test Case: Circuit 28
            /*
            qc = new(10, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateCX(0, 2);
            qc.addGateZ(3);
            qc.addGateTOF(4, 5, 6);
            qc.addGateH(7);
            qc.addGateSWAP(8, 9);
            qc.addGateCX(7, 8);
            qc.addGateCX(6, 7);
            qc.addGateCX(5, 4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[27] = result;
            */
            resultArr[27] = stateVectors[27];

            // Test Case: Circuit 29
            /*
            qc = new(12, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateTOF(2, 3, 4);
            qc.addGateH(5);
            qc.addGateCX(0, 6);
            qc.addGateCX(6, 7);
            qc.addGateCX(7, 8);
            qc.addGateSWAP(9, 10);
            qc.addGateCZ(11, 0);
            qc.addGateCZ(1, 2);
            qc.addGateCX(3, 4);
            qc.addGateCX(5, 6);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[28] = result;
            */
            resultArr[28] = stateVectors[28];

            // Test Case: Circuit 30
            /*
            qc = new(14, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 2);
            qc.addGateCX(2, 3);
            qc.addGateTOF(4, 5, 6);
            qc.addGateH(7);
            qc.addGateCX(8, 9);
            qc.addGateCZ(10, 11);
            qc.addGateCZ(12, 13);
            qc.addGateCX(7, 8);
            qc.addGateCX(6, 7);
            qc.addGateTOF(5, 6, 7);
            qc.addGateSWAP(10, 12);
            qc.addGateSWAP(11, 13);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[29] = result;
            */
            resultArr[29] = stateVectors[29];

            // Test Case: Circuit 31
            /*
            qc = new(15, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateCX(0, 2);
            qc.addGateCX(1, 3);
            qc.addGateTOF(2, 3, 4);
            qc.addGateH(5);
            qc.addGateCX(4, 6);
            qc.addGateCX(6, 7);
            qc.addGateTOF(7, 8, 9);
            qc.addGateCZ(10, 11);
            qc.addGateSWAP(12, 13);
            qc.addGateCZ(13, 14);
            qc.addGateCX(9, 10);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[30] = result;
            */
            resultArr[30] = stateVectors[30];

            // Test Case: Circuit 32
            /*
            qc = new(17, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 2);
            qc.addGateTOF(2, 3, 4);
            qc.addGateCX(4, 5);
            qc.addGateCX(5, 6);
            qc.addGateCX(6, 7);
            qc.addGateCZ(7, 8);
            qc.addGateCZ(8, 9);
            qc.addGateTOF(9, 10, 11);
            qc.addGateCX(11, 12);
            qc.addGateSWAP(12, 13);
            qc.addGateCX(13, 14);
            qc.addGateCZ(14, 15);
            qc.addGateSWAP(15, 16);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[31] = result;
            */
            resultArr[31] = stateVectors[31];

            // Test Case: Circuit 33
            /*
            qc = new(18, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateH(2);
            qc.addGateTOF(3, 4, 5);
            qc.addGateTOF(5, 6, 7);
            qc.addGateCX(7, 8);
            qc.addGateCX(8, 9);
            qc.addGateTOF(9, 10, 11);
            qc.addGateCX(11, 12);
            qc.addGateCX(12, 13);
            qc.addGateCX(13, 14);
            qc.addGateCZ(15, 16);
            qc.addGateCX(16, 17);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[32] = result;
            */
            resultArr[32] = stateVectors[32];
            // Test Case: Circuit 34
            /*
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(0);
            qc.addGateH(1);
            qc.addGateCX(0, 1);
            qc.addGateH(0);
            qc.addGateCX(1, 0);
            qc.addGateZ(1);
            qc.addGateX(0);
            qc.addGateCX(0, 1);
            qc.addGateH(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[33] = result;
            */
            resultArr[33] = stateVectors[33];

            // Test Case: Circuit 35
            /*
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateZ(1);
            qc.addGateCX(0, 1);
            qc.addGateH(1);
            qc.addGateZ(0);
            qc.addGateCX(1, 0);
            qc.addGateX(0);
            qc.addGateH(1);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateZ(1);
            qc.addGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[34] = result;
            */
            resultArr[34] = stateVectors[34];

            // Test Case: Circuit 36
            /*
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateCX(1, 0);
            qc.addGateH(1);
            qc.addGateCZ(1, 2);
            qc.addGateH(0);
            qc.addGateCX(1, 2);
            qc.addGateCX(2, 0);
            qc.addGateZ(1);
            qc.addGateX(2);
            qc.addGateCX(1, 0);
            qc.addGateH(2);
            qc.addGateCZ(0, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[35] = result;
            */
            resultArr[35] = stateVectors[35];

            // Test Case: Circuit 37
            /*
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(2);
            qc.addGateH(1);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);
            qc.addGateCZ(2, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateZ(2);
            qc.addGateH(1);
            qc.addGateX(0);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[36] = result;*/
            resultArr[36] = stateVectors[36];

            // Test Case: Circuit 38
            /*
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateCX(1, 0);
            qc.addGateX(1);
            qc.addGateZ(0);
            qc.addGateH(1);
            qc.addGateH(0);
            qc.addGateCX(1, 0);
            qc.addGateZ(1);
            qc.addGateX(0);
            qc.addGateCX(0, 1);
            qc.addGateH(1);
            qc.addGateH(0);
            qc.addGateZ(1);
            qc.addGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[37] = result;
            */
            resultArr[37] = stateVectors[37];

            // Test Case: Circuit 39
            /*
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);
            qc.addGateCX(0, 2);
            qc.addGateZ(0);
            qc.addGateH(1);
            qc.addGateX(2);
            qc.addGateCX(1, 0);
            qc.addGateCX(1, 2);
            qc.addGateZ(2);
            qc.addGateX(0);
            qc.addGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[38] = result;
            */
            resultArr[38] = stateVectors[38];

            // Test Case: Circuit 40
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateZ(0);
            qc.addGateCX(1, 0);
            qc.addGateCX(0, 1);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateX(0);
            qc.addGateZ(1);
            qc.addGateCX(1, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[39] = result;

            // Test Case: Circuit 41
            /*
            qc = new(3, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateCX(1, 0);
            qc.addGateCX(2, 1);
            qc.addGateCX(0, 2);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);
            qc.addGateCZ(0, 1);
            qc.addGateCZ(1, 2);
            qc.addGateCZ(2, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[40] = result;*/
            resultArr[40] = stateVectors[40];

            // Test Case: Circuit 42
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateCX(1, 0);
            qc.addGateCX(0, 1);
            qc.addGateH(1);
            qc.addGateH(0);
            qc.addGateZ(1);
            qc.addGateX(0);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateZ(0);
            qc.addGateCX(0, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[41] = result;

            // Test Case: Circuit 43
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateZ(1);
            qc.addGateCX(1, 0);
            qc.addGateX(0);
            qc.addGateH(1);
            qc.addGateCX(0, 1);
            qc.addGateZ(1);
            qc.addGateCX(1, 0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateX(1);
            qc.addGateH(0);
            qc.addGateZ(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[42] = result;

            // Test Case: Circuit 44
            qc = new(2, 0);
            qc.addGateH(0);
            qc.addGateX(1);
            qc.addGateCX(1, 0);
            qc.addGateH(1);
            qc.addGateZ(0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateCX(1, 0);
            qc.addGateZ(1);
            qc.addGateH(0);
            qc.addGateCX(0, 1);
            qc.addGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[43] = result;



            bool allApproxEqual = true;
            double tolerance = 1e-10;

            for (int i = 0; i < resultArr.Length; i++)
            {
                //Console.WriteLine("TEST NUMBER " + i);
                //Console.WriteLine(resultArr[i]);
                //Console.WriteLine(stateVectors[i]);

            }

                for (int i = 0; i < resultArr.Length; i++)
            {
                if (!resultArr[i].IsApproximatelyEqual(stateVectors[i], tolerance))
                {
                    Console.WriteLine(resultArr[i]);
                    Console.WriteLine(stateVectors[i]);
                    Console.WriteLine(i);
                    allApproxEqual = false;
                    break;
                }
            }

            Assert.IsTrue(allApproxEqual, "Vectors are not approximately equal");
           
        }
    }
}
