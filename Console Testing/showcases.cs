using LinearAlgebra;
using System.Diagnostics;
using System.Numerics;
using QuantumCircuits;

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


        public void HalfAdderTest()
        {
            // build a circuit with one quantum and one classical line
            QuantumCircuitBuilder qc = new QuantumCircuitBuilder(3, 1);

            // half adder

            //input bits
            qc.addGateX(0);
            qc.addGateX(1);

            qc.addGateTOF(2, 1, 0);
            qc.addGateCX(1, 0);

            // print out circuit
            Console.WriteLine(qc.ToString());

            CircuitExecution exe = new CircuitExecution(qc);

            // print out execution columns
            Console.WriteLine(exe.ToString());

            // returns the statevector after executing all columns
            Console.WriteLine(exe.ExecuteCircuit());

        }













       /*
        * 
        * 
        * 
         New implementation broke these showcases 


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
        cs.printCircuit();
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
    */

    }
}
