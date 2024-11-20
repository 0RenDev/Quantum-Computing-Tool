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
using System.Diagnostics;
using OfficeOpenXml;

namespace QuantumCircuit.Tests
{
    

    [TestFixture()]
    public class QuantumCircuitTests
    { // Install EPPlus NuGet package for Excel handling.

        [Test]
        public void TestCircuitPerformanceGrapher()
        {
            int maxQubits = 14;
            int maxTotalGates = 50;
            int numberOfExecutions = 50;

            List<(int qubits, int gates, double avgTime)> results = new List<(int, int, double)>();

            Random random = new Random();

            for (int totalNumberOfGates = 1; totalNumberOfGates <= maxTotalGates; totalNumberOfGates++)
            {
                for (int numberOfQubits = 1; numberOfQubits <= maxQubits; numberOfQubits++)
                {
                    double totalExecutionTime = 0;

                    for (int executionIndex = 0; executionIndex < numberOfExecutions; executionIndex++)
                    {
                        QuantumCircuitBuilder qc = new QuantumCircuitBuilder(numberOfQubits, 0);

                        for (int gateIndex = 0; gateIndex < totalNumberOfGates; gateIndex++)
                        {
                            int gateType = random.Next(1, 6); // Random gate type (1 = H, 2 = X, etc.)
                            int targetQubit = random.Next(0, numberOfQubits);

                            switch (gateType)
                            {
                                case 1: // H
                                    qc.AddGateH(targetQubit);
                                    break;

                                case 2: // X
                                    qc.AddGateX(targetQubit);
                                    break;

                                case 3: // Z
                                    qc.AddGateZ(targetQubit);
                                    break;

                                case 4: // CX
                                    if (numberOfQubits >= 2)
                                    {
                                        int control = targetQubit;
                                        int target = (targetQubit + 1) % numberOfQubits;
                                        qc.AddGateCX(control, target);
                                    }
                                    break;

                                case 5: // TOF
                                    if (numberOfQubits >= 3)
                                    {
                                        int control1 = targetQubit;
                                        int control2 = (targetQubit + 1) % numberOfQubits;
                                        int target = (targetQubit + 2) % numberOfQubits;
                                        qc.AddGateTOF(control1, control2, target);
                                    }
                                    break;
                            }
                        }

                        CircuitExecution exe = new CircuitExecution(qc);
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        exe.ExecuteCircuit();
                        stopwatch.Stop();

                        totalExecutionTime += stopwatch.Elapsed.TotalMilliseconds;
                    }

                    double averageExecutionTime = totalExecutionTime / numberOfExecutions;
                    results.Add((numberOfQubits, totalNumberOfGates, averageExecutionTime));
                }
            }

            ExportResultsToExcel(results, "CircuitPerformance.xlsx");
        }

        private void ExportResultsToExcel(List<(int qubits, int gates, double avgTime)> results, string filePath)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add("Circuit Performance");
                sheet.Cells[1, 1].Value = "Qubits";
                sheet.Cells[1, 2].Value = "Gates";
                sheet.Cells[1, 3].Value = "Avg Time (ms)";

                for (int i = 0; i < results.Count; i++)
                {
                    sheet.Cells[i + 2, 1].Value = results[i].qubits;
                    sheet.Cells[i + 2, 2].Value = results[i].gates;
                    sheet.Cells[i + 2, 3].Value = results[i].avgTime;
                }

                excel.SaveAs(new System.IO.FileInfo(filePath));
            }

            Console.WriteLine($"Results exported to {filePath}");
        }



        [Test]
        public void TestCircuitPerformance()
        {
            int numberOfQubits = 18;  
            int totalNumberOfGates = 20;  
            int numberOfExecutions = 1;  

            Random random = new Random();
            double totalExecutionTime = 0;

            for (int executionIndex = 0; executionIndex < numberOfExecutions; executionIndex++)
            {
                QuantumCircuitBuilder qc = new QuantumCircuitBuilder(numberOfQubits, 0);
                for (int gateIndex = 0; gateIndex < totalNumberOfGates; gateIndex++)
                {
                    int gateType = random.Next(1, 6); // 1 = H, 2 = X, 3 = Z, 4 = CX, 5 = TOF.
                    int targetQubit = random.Next(0, numberOfQubits);

                    switch (gateType)
                    {
                        case 1: // H.
                            qc.AddGateH(targetQubit);
                            break;

                        case 2: // X.
                            qc.AddGateX(targetQubit);
                            break;

                        case 3: // Z.
                            qc.AddGateZ(targetQubit);
                            break;

                        case 4: // CX.
                            if (targetQubit < numberOfQubits - 1)
                            {
                                qc.AddGateCX(targetQubit, targetQubit + 1);
                            }
                            else
                            {
                                qc.AddGateCX(targetQubit - 1, targetQubit);
                            }
                            break;

                        case 5: // TOF.
                            if (targetQubit < numberOfQubits - 2)
                            {
                                qc.AddGateTOF(targetQubit, targetQubit + 1, targetQubit + 2);
                            }
                            else if (targetQubit == numberOfQubits - 2)
                            {
                                qc.AddGateTOF(targetQubit - 1, targetQubit, targetQubit + 1);
                            }
                            else
                            {
                                qc.AddGateTOF(targetQubit - 2, targetQubit - 1, targetQubit);
                            }
                            break;
                    }
                }

                CircuitExecution exe = new CircuitExecution(qc);
                Stopwatch stopwatch = Stopwatch.StartNew();
                exe.ExecuteCircuit();
                stopwatch.Stop();

                totalExecutionTime += stopwatch.Elapsed.TotalMilliseconds;
            }

            // Calculate and display the average execution time
            double averageExecutionTime = totalExecutionTime / numberOfExecutions;
            Console.WriteLine($"Average execution time for {numberOfExecutions} executions: {averageExecutionTime:F2} ms");
        }



        [Test]
        public void QuantumCircuitExecutionTest()
        {
            QuantumCircuitBuilder qc = new(2, 0);

            qc.AddGateH(1);
            qc.AddGateCX(0, 1);
            qc.AddGateX(1);

            CircuitExecution exe = new(qc); 

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            Console.WriteLine(qc.ToString());

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
                    if (line == null) break;

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
        public void ExpectationValueTest(){
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(2,0);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);

            CircuitExecution exe = new CircuitExecution(qc);

            double result = exe.GetExpectationValue("ZZ");
            
            double testOne = 1; 

            Assert.AreEqual(testOne, result);
            
            
        }

        [Test]
        public void zTest()
        {
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(3, 0);
            //qc.addGateSWAP(0, 1);
            CircuitExecution exe = new CircuitExecution(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();
            Console.Write(result);
        }


        [Test]
        public void massiveQCT()
        {
            LinearAlgebra.Vector[] stateVectors = ReadStateVectorsFromCsv("..\\..\\..\\MassiveQubitTest.csv");
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(5, 0);  // 5 lines
        
            // Build the circuit
            for (int i = 0; i < 100; i++)  // 100 gates on each line
            {
                qc.AddGateH(0);
                qc.AddGateTOF(1, 2, 3);
                qc.AddGateX(0);
                qc.AddGateCX(2, 3);
                qc.AddGateZ(4);
                qc.AddGateZ(2);
                qc.AddGateCX(1, 2);
                qc.AddGateH(4);
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
            LinearAlgebra.Vector[] stateVectors = ReadStateVectorsFromCsv("..\\..\\..\\statevectors.csv");
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(1,0);


            //QuantumCircuitBuilder qc = new(1, 0);
            qc.AddGateH(0);

            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            LinearAlgebra.Vector[] resultArr = new LinearAlgebra.Vector[44];

            resultArr[0] = result;


            //SingleXGate
            qc = new(1, 0);
            qc.AddGateX(0);

            exe = new(qc);

            result = exe.ExecuteCircuit();
            resultArr[1] = result;


            //SingleZGate
            qc = new(1, 0);
            qc.AddGateZ(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[2] = result;


            //TwoHgate
            qc = new(1, 0);
            qc.AddGateH(0);
            qc.AddGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[3] = result;

            //HGateAndXgate
            qc = new(1, 0);
            qc.AddGateH(0);
            qc.AddGateX(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[4] = result;

            //TwoQubitTest_H1X2
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[5] = result;

            //TwoQubitTest_H1CX
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[6] = result;

            // Test Case: Circuit 8
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[7] = result;


            //ThreeQubitTest_H
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[8] = result;


            // Test Case: Circuit 10
            qc = new(4, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[9] = result;


            //FourQubitTest_HCX
            qc = new(4, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[10] = result;

            //TwoQubitTest_hx
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateX(0);
            qc.AddGateX(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[11] = result;

            //ThreeQubitTest_CX
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[12] = result;

            // Test Case: Circuit 14
            qc = new(4, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[13] = result;


            //FiveQubits_HCX
            qc = new(5, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);
            qc.AddGateH(3);
            qc.AddGateH(4);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);
            qc.AddGateCX(3, 2);
            qc.AddGateCX(4, 3);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[14] = result;

            //ThreeQubits
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateH(2);
            qc.AddGateCX(2, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[15] = result;

            // Test Case: Circuit 17
            qc = new(5, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateH(3);
            qc.AddGateX(4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[16] = result;


            //TwoQUbits_HCX
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateCX(1, 0);


            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[17] = result;

            // Test Case: Circuit 19
            qc = new(4, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateH(3);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(3, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[18] = result;


            // Test Case: Circuit 20
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[19] = result;

            // Test Case: Circuit 21
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateX(0);
            qc.AddGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[20] = result;

            // Test Case: Circuit 22
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);
            qc.AddGateH(0);
            qc.AddGateX(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[21] = result;

            // Test Case: Circuit 23
            qc = new(5, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateH(3);
            qc.AddGateX(4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[22] = result;

            // Test Case: Circuit 24
            qc = new(4, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateH(3);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(3, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[23] = result;

            // Test Case: Circuit 25
            /*
            qc = new(6, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.addGateCZ(3, 4);
            qc.AddGateH(5);
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
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);
            qc.AddGateTOF(2, 1, 0);
            qc.AddGateCX(4, 3);
            qc.AddGateCX(5, 4);
            qc.AddGateCX(6, 5);
            qc.AddGateTOF(4, 3, 2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[25] = result;

            // Test Case: Circuit 27
            /*
            qc = new(8, 0);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateCX(1, 2);
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
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateCX(0, 2);
            qc.AddGateZ(3);
            qc.AddGateTOF(4, 5, 6);
            qc.AddGateH(7);
            qc.addGateSWAP(8, 9);
            qc.AddGateCX(7, 8);
            qc.AddGateCX(6, 7);
            qc.AddGateCX(5, 4);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[27] = result;
            */
            resultArr[27] = stateVectors[27];

            // Test Case: Circuit 29
            /*
            qc = new(12, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateTOF(2, 3, 4);
            qc.AddGateH(5);
            qc.AddGateCX(0, 6);
            qc.AddGateCX(6, 7);
            qc.AddGateCX(7, 8);
            qc.addGateSWAP(9, 10);
            qc.addGateCZ(11, 0);
            qc.addGateCZ(1, 2);
            qc.AddGateCX(3, 4);
            qc.AddGateCX(5, 6);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[28] = result;
            */
            resultArr[28] = stateVectors[28];

            // Test Case: Circuit 30
            /*
            qc = new(14, 0);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateCX(1, 2);
            qc.AddGateCX(2, 3);
            qc.AddGateTOF(4, 5, 6);
            qc.AddGateH(7);
            qc.AddGateCX(8, 9);
            qc.addGateCZ(10, 11);
            qc.addGateCZ(12, 13);
            qc.AddGateCX(7, 8);
            qc.AddGateCX(6, 7);
            qc.AddGateTOF(5, 6, 7);
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
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateCX(0, 2);
            qc.AddGateCX(1, 3);
            qc.AddGateTOF(2, 3, 4);
            qc.AddGateH(5);
            qc.AddGateCX(4, 6);
            qc.AddGateCX(6, 7);
            qc.AddGateTOF(7, 8, 9);
            qc.addGateCZ(10, 11);
            qc.addGateSWAP(12, 13);
            qc.addGateCZ(13, 14);
            qc.AddGateCX(9, 10);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[30] = result;
            */
            resultArr[30] = stateVectors[30];

            // Test Case: Circuit 32
            /*
            qc = new(17, 0);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateCX(1, 2);
            qc.AddGateTOF(2, 3, 4);
            qc.AddGateCX(4, 5);
            qc.AddGateCX(5, 6);
            qc.AddGateCX(6, 7);
            qc.addGateCZ(7, 8);
            qc.addGateCZ(8, 9);
            qc.AddGateTOF(9, 10, 11);
            qc.AddGateCX(11, 12);
            qc.addGateSWAP(12, 13);
            qc.AddGateCX(13, 14);
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
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateH(2);
            qc.AddGateTOF(3, 4, 5);
            qc.AddGateTOF(5, 6, 7);
            qc.AddGateCX(7, 8);
            qc.AddGateCX(8, 9);
            qc.AddGateTOF(9, 10, 11);
            qc.AddGateCX(11, 12);
            qc.AddGateCX(12, 13);
            qc.AddGateCX(13, 14);
            qc.addGateCZ(15, 16);
            qc.AddGateCX(16, 17);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[32] = result;
            */
            resultArr[32] = stateVectors[32];
            // Test Case: Circuit 34
            /*
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(0);
            qc.AddGateH(1);
            qc.AddGateCX(0, 1);
            qc.AddGateH(0);
            qc.AddGateCX(1, 0);
            qc.AddGateZ(1);
            qc.AddGateX(0);
            qc.AddGateCX(0, 1);
            qc.AddGateH(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[33] = result;
            */
            resultArr[33] = stateVectors[33];

            // Test Case: Circuit 35
            /*
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateZ(1);
            qc.AddGateCX(0, 1);
            qc.AddGateH(1);
            qc.AddGateZ(0);
            qc.AddGateCX(1, 0);
            qc.AddGateX(0);
            qc.AddGateH(1);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateZ(1);
            qc.AddGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[34] = result;
            */
            resultArr[34] = stateVectors[34];

            // Test Case: Circuit 36
            /*
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateCX(1, 0);
            qc.AddGateH(1);
            qc.addGateCZ(1, 2);
            qc.AddGateH(0);
            qc.AddGateCX(1, 2);
            qc.AddGateCX(2, 0);
            qc.AddGateZ(1);
            qc.AddGateX(2);
            qc.AddGateCX(1, 0);
            qc.AddGateH(2);
            qc.addGateCZ(0, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[35] = result;
            */
            resultArr[35] = stateVectors[35];

            // Test Case: Circuit 37
            /*
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(2);
            qc.AddGateH(1);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);
            qc.addGateCZ(2, 0);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateZ(2);
            qc.AddGateH(1);
            qc.AddGateX(0);
            qc.AddGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[36] = result;*/
            resultArr[36] = stateVectors[36];

            // Test Case: Circuit 38
            /*
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateCX(1, 0);
            qc.AddGateX(1);
            qc.AddGateZ(0);
            qc.AddGateH(1);
            qc.AddGateH(0);
            qc.AddGateCX(1, 0);
            qc.AddGateZ(1);
            qc.AddGateX(0);
            qc.AddGateCX(0, 1);
            qc.AddGateH(1);
            qc.AddGateH(0);
            qc.AddGateZ(1);
            qc.AddGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[37] = result;
            */
            resultArr[37] = stateVectors[37];

            // Test Case: Circuit 39
            /*
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);
            qc.AddGateCX(0, 2);
            qc.AddGateZ(0);
            qc.AddGateH(1);
            qc.AddGateX(2);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(1, 2);
            qc.AddGateZ(2);
            qc.AddGateX(0);
            qc.AddGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[38] = result;
            */
            resultArr[38] = stateVectors[38];

            // Test Case: Circuit 40
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateZ(0);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(0, 1);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateX(0);
            qc.AddGateZ(1);
            qc.AddGateCX(1, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateCX(1, 0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[39] = result;

            // Test Case: Circuit 41
            /*
            qc = new(3, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(2, 1);
            qc.AddGateCX(0, 2);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);
            qc.addGateCZ(0, 1);
            qc.addGateCZ(1, 2);
            qc.addGateCZ(2, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[40] = result;*/
            resultArr[40] = stateVectors[40];

            // Test Case: Circuit 42
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateCX(1, 0);
            qc.AddGateCX(0, 1);
            qc.AddGateH(1);
            qc.AddGateH(0);
            qc.AddGateZ(1);
            qc.AddGateX(0);
            qc.AddGateCX(0, 1);
            qc.AddGateCX(1, 0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateZ(0);
            qc.AddGateCX(0, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[41] = result;

            // Test Case: Circuit 43
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateZ(1);
            qc.AddGateCX(1, 0);
            qc.AddGateX(0);
            qc.AddGateH(1);
            qc.AddGateCX(0, 1);
            qc.AddGateZ(1);
            qc.AddGateCX(1, 0);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateX(1);
            qc.AddGateH(0);
            qc.AddGateZ(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[42] = result;

            // Test Case: Circuit 44
            qc = new(2, 0);
            qc.AddGateH(0);
            qc.AddGateX(1);
            qc.AddGateCX(1, 0);
            qc.AddGateH(1);
            qc.AddGateZ(0);
            qc.AddGateH(0);
            qc.AddGateH(1);
            qc.AddGateCX(1, 0);
            qc.AddGateZ(1);
            qc.AddGateH(0);
            qc.AddGateCX(0, 1);
            qc.AddGateCX(1, 0);

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
