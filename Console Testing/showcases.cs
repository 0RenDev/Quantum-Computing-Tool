using LinearAlgebra;
using Quantum;
using System.Diagnostics;
using System.Numerics;
using QuantumCircuit;

namespace Console_Testing
{
    // this class is meant to contain tests that will run in main when the tests are too complicated to be made into a test case
    // or require some amount of interaction. they should be written as a method then added into main
    internal class Showcases
    {

        // private method used to generate random matrices of m x n size for use in the showcases
        private static Matrix GenerateRandomMatrix(int m, int n)
        {
            Random rand = new Random();

            Complex[,] array = new Complex[m, n];
            for (int r = 0; r < m; r++)
            {
                for (int j = 0; j < n; j++)
                {
                    array[r, j] = new Complex(rand.NextDouble(), rand.Next(10));
                }
            }

            // populate matices
            return new Matrix(array);
        }

        static Complex[] GenerateRandomStatevector(int numQubits)
        {
            // Calculate the length of the statevector (2^numQubits)
            int statevectorLength = 1 << numQubits;
            Complex[] statevector = new Complex[statevectorLength];

            // Populate statevector with random complex numbers
            Random rand = new Random();
            double normSquaredSum = 0;
            for (int i = 0; i < statevector.Length; i++)
            {
                double real = rand.NextDouble();
                double imag = rand.NextDouble();
                statevector[i] = new Complex(real, imag);
                normSquaredSum += Math.Pow(statevector[i].Magnitude, 2);
            }

            // Normalize the statevector
            double normalizationFactor = Math.Sqrt(normSquaredSum);
            for (int i = 0; i < statevector.Length; i++)
            {
                statevector[i] /= normalizationFactor;
            }

            return statevector;
        }


        // simple showcase that performs a vector matrix multiplication 
        public static void Vector_times_matrix()
        {
            // simple vector matrix multiplication
            Matrix vector = new Matrix(new Complex[,] { { 1 }, { 2 }, { 3 } });
            Console.WriteLine(vector);
            Matrix matrix = new Matrix(new Complex[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Console.WriteLine(matrix);
            Matrix result = Operations.Multiply(matrix, vector);
            Console.WriteLine(result);

            // Arrange
            Matrix matrix1 = new Matrix(new Complex[,] { { 1, 2 }, { 3, 4 } });
            Matrix matrix2 = new Matrix(new Complex[,] { { 5, 6, 7 }, { 8, 9, 10 } });

            // Act & Assert
            Console.WriteLine(Operations.MatrixMultiply(matrix1, matrix2));


        }


        // showcases the difference in performance between the naive matrix multipication and a multithreaded variation 
        public static void MultThreadedMult()
        {
            int n = 125;
            int m = 1000;
            int k = 350;

            Console.WriteLine(new string('-', 20));
            Console.WriteLine(new string('-', 20));

            Console.WriteLine("Beginning MatMul.");

            Stopwatch watch1 = new Stopwatch();

            // populate matrices
            Matrix A = GenerateRandomMatrix(n, m);
            Matrix B = GenerateRandomMatrix(m, k);

            watch1 = new Stopwatch();
            watch1.Start();
            Matrix C1 = Operations.MatrixMultiply(A, B);
            watch1.Stop();
            Console.WriteLine("Threaded MatMul time : " + watch1.ElapsedMilliseconds);

            watch1 = new Stopwatch();
            watch1.Start();
            Matrix C2 = Operations.Multiply(A, B);
            watch1.Stop();
            Console.WriteLine("Elementary MatMul time : " + watch1.ElapsedMilliseconds);

            Console.WriteLine("PASSED: " + (Operations.IsEqual(C1, C2)));
        }


        // tests the time needed to perform a tensor product on given sized matrices
        public static void TimeTestTensor(int m, int n, int k, int p)
        {


            // fill matrix arrays with random values 
            Random rand = new Random();

            // populate matrices
            Matrix A = GenerateRandomMatrix(n, m);
            Matrix B = GenerateRandomMatrix(m, k);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Matrix C = Operations.TensorProduct(A, B);
            stopwatch.Stop();
            TimeSpan time = stopwatch.Elapsed;
            //MatrixOperations.printMatrix(tp);
            Console.WriteLine("Matrices of " + m + "x" + n + " and " + k + "x" + p + "matrices");
            // add something that displays the dim of the resulting matrix
            Console.WriteLine($"Matrix Tensor Product: {time.TotalMilliseconds} milliseconds");
        }

        // Demonstrates the evolution of a qubit state by using varying quantum operators
        public static void QbitEvolutionDemo()
        {
            // Define matrices representing the required quantum gates
            Complex[,] x = { { 0, 1 }, { 1, 0 } };
            Complex[,] y = { { 0, -1 * Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } };
            Complex[,] z = { { 1, 0 }, { 0, -1 } };
            Complex[,] h = { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) }, { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } };
            Complex[,] s = { { 1, 0 }, { 0, Complex.ImaginaryOne } };
            Complex[,] t = { { 1, 0 }, { 0, (1 + Complex.ImaginaryOne) / Complex.Sqrt(2) } };

            // Make operator objects for each matrix
            Operator X = new Operator(x);
            Operator Y = new Operator(y);
            Operator Z = new Operator(z);
            Operator H = new Operator(h);
            Operator S = new Operator(s);
            Operator T = new Operator(t);

            // Initailize qubit in state |0> 
            Complex[] state = { 1, 0 };
            Qbit q = new Qbit(state);

            Console.WriteLine("Initial State Vector:\n" + q.ToString() + "\n");

            // Apply X, Y, Z operators sequentially to qubit
            q.Evolve(X);
            q.Evolve(Y);
            q.Evolve(Z);

            // Display final qubit state
            Console.WriteLine("State Vector after evolution:\n" + q.ToString() + "\n");

        }

        // Demonstrates the creation of a simple quantum circuit object
        public static void QuantumConstructionPlay()
        {
            // Initialize quantum circuit object
            QuantumCircuitObject cs = new QuantumCircuitObject("Test");

            // Add quantum lines
            cs.AddQuantumLine("X");
            cs.AddQuantumLine("Y");
            cs.AddQuantumLine("Z");
            cs.AddQuantumLine("D");

            // Push tof gate with control Y, Z, D with target X
            String[] tofPushbacks = { "Y", "Z", "D" };
            cs.PushBackTOF(tofPushbacks, "X");

            // Apply three Hadamard gates to X
            cs.PushBackH("X");
            cs.PushBackH("X");
            cs.PushBackH("X");

            // Apply various gates to different lines
            cs.PushBackH("Y");
            cs.PushBackCNOT("X", "Y");
            cs.PushBackZ("X");
            cs.PushBackY("Y");

            // Apply more gates to Z and D
            cs.PushBackY("Z");
            cs.PushBackH("D");
            String[] tofPushbacks2 = { "X", "Y" };
            cs.PushBackTOF(tofPushbacks, "Z");
            cs.PushBackCNOT("Y", "X");

            // Print result
            cs.PrintCircuit();
            /*
        QuantumCircuitObject cs = new QuantumCircuitObject("Test");
        cs.AddQuantumLine("X");
        cs.AddQuantumLine("Y");
        //cs.pushBackH("X");
        cs.pushBackCNOT("X", "Y");
        cs.printCircuit();*/
        }

        public static void TofGateTest()
        {
            QuantumCircuitObject QA = new QuantumCircuitObject("TofTest");

            QA.AddQuantumLine("q[0]");
            QA.AddQuantumLine("q[1]");
            QA.AddQuantumLine("q[2]");
            QA.PushBackH("q[0]");
            QA.PushBackH("q[2]");
            QA.PushBackH("q[2]");
            QA.PushBackTOF(["q[0]", "q[1]"], "q[2]");
            QA.PrintCircuit();
        }

        // Demonstrates a quantum adder circuit
        public static void QuantumAdderConstruction()
        {
            // Create a new quantum circuit object 
            QuantumCircuitObject QA = new QuantumCircuitObject("Adder");

            // Add four quantum lines
            QA.AddQuantumLine("q[0]");
            QA.AddQuantumLine("q[1]");
            QA.AddQuantumLine("q[2]");
            QA.AddQuantumLine("q[3]");

            // Apply X gate to q[0] and q[2] 
            QA.PushBackX("q[0]");
            QA.PushBackX("q[2]");

            // Apply CNOT gates with different controls and targets to form the adder logic
            QA.PushBackCNOT("q[3]", ["q[0]", "q[1]"]);
            QA.PushBackCNOT("q[1]", ["q[0]"]);
            QA.PushBackCNOT("q[3]", ["q[1]", "q[2]"]);
            QA.PushBackCNOT("q[2]", ["q[1]"]);
            QA.PushBackCNOT("q[1]", ["q[0]"]);

            // Print result
            QA.PrintCircuit();

        }


        public void MeasurementTest()
        {
            for (int numQubits = 1; numQubits < 6; numQubits++)
            {
                Complex[] statevector = GenerateRandomStatevector(numQubits);

                // Print the generated statevector
                Console.WriteLine("Generated statevector:");
                for (int i = 0; i < statevector.Length; i++)
                {
                    Console.WriteLine("|" + Convert.ToString(i, 2).PadLeft(numQubits, '0') + "> = " + statevector[i]);
                }

                // get probabilites vector
                double[] probabilites = StatevectorProbabilities(statevector);

                // Print the generated statevector
                Console.WriteLine("Probability vector:");
                for (int i = 0; i < probabilites.Length; i++)
                {
                    Console.WriteLine("|" + Convert.ToString(i, 2).PadLeft(numQubits, '0') + "> = " + probabilites[i]);
                }

                // measurments 
                for (int i = 0; i < 10; i++)
                {
                    int measurement = MeasureProbabilities(probabilites);
                    Console.WriteLine("Measurement outcome: " + Convert.ToString(measurement, 2).PadLeft((int)Math.Log(statevector.Length, 2), '0'));
                }
            }
        }






        public static double[] StatevectorProbabilities(Complex[] statevector)
        {
            // Check if the input vector is a valid statevector
            if (!IsValidStatevector(statevector))
            {
                throw new ArgumentException("Invalid statevector. The norm (magnitude) of the statevector must equal 1.");
            }

            // Calculate unnormalized probabilities
            double[] unnormalizedProbabilities = new double[statevector.Length];
            for (int i = 0; i < statevector.Length; i++)
            {
                unnormalizedProbabilities[i] = Math.Pow(statevector[i].Magnitude, 2);
            }

            // Calculate normalization factor
            double normalizationFactor = 0;
            for (int i = 0; i < unnormalizedProbabilities.Length; i++)
            {
                normalizationFactor += unnormalizedProbabilities[i];
            }

            // Normalize probabilities
            double[] probabilities = new double[statevector.Length];
            for (int i = 0; i < probabilities.Length; i++)
            {
                probabilities[i] = unnormalizedProbabilities[i] / normalizationFactor;
            }

            return probabilities;
        }

        private int MeasureProbabilities(double[] probabilities)
        {
            // Perform measurement
            Random random = new Random();
            double rand = random.NextDouble();
            double cumulativeProbability = 0;
            int measurementOutcome = -1;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (rand < cumulativeProbability)
                {
                    measurementOutcome = i;
                    break;
                }
            }

            return measurementOutcome;
        }

        public static bool IsValidStatevector(Complex[] statevector)
        {
            // Calculate the norm (magnitude) of the statevector
            double norm = 0;
            for (int i = 0; i < statevector.Length; i++)
            {
                norm += Math.Pow(statevector[i].Magnitude, 2);
            }

            // Check if the norm equals 1 (within a small tolerance)
            return Math.Abs(norm - 1) < 1e-10;
        }





    }
}
