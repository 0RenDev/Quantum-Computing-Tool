using LinearAlgebra;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuits
{
    /// <summary>
    /// This class is responsible for executing a quantum circuit
    /// </summary>
    public class CircuitExecution
    {
        /// <summary>
        /// The circuit object that will be executed
        /// </summary>
        QuantumCircuitBuilder circuit;

        /// <summary>
        /// The execution columns reprsent the gates that are executed in each time step.
        /// </summary>
        List<List<Gate>> executionColumns = new List<List<Gate>>();

        /// <summary>
        /// Gets the qbit count.
        /// </summary>
        /// <value>
        /// The qbit count.
        /// </value>
        public int QbitCount { get; private set; }

        /// <summary>
        /// Gets the state vector.
        /// </summary>
        /// <value>
        /// The state vector.
        /// </value>
        public Complex[] StateVector { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitExecution"/> class.
        /// </summary>
        /// <param name="inputcircuit">The inputcircuit.</param>
        public CircuitExecution(QuantumCircuitBuilder inputcircuit)
        {
            circuit = inputcircuit;
            QbitCount = circuit.quantumLines.Length;

            int longestLine = 0;

            List<LinearAlgebra.Vector> tempVectorList = new List<LinearAlgebra.Vector>();

            for( int i = 0; i < circuit.quantumLines.Length; i++)
            {
                if(longestLine < circuit.quantumLines[i].Count)
                {
                    longestLine = circuit.quantumLines[i].Count;
                }

                tempVectorList.Add(new LinearAlgebra.Vector(new Complex[] { 1, 0 }));
            }

            StateVector = new Complex[1 << circuit.quantumLines.Length];
            StateVector[0] = 1;


            for (int i = 0; i < longestLine; i++)
            {
                List<Gate> columnOfGates = new List<Gate>();
                for( int j = 0; j < circuit.quantumLines.Length; j++ )
                {
                    if (circuit.quantumLines[j].Count > 0)
                    {
                        columnOfGates.Add(circuit.quantumLines[j][0]);
                        circuit.quantumLines[j].RemoveAt(0);
                    }
                    else
                    {
                        Gate spacerNoOp = new NOP(j, GateTypes.NOP);
                        columnOfGates.Add(spacerNoOp);
                    }
                }
                executionColumns.Add(columnOfGates);
            }
        }

        /// <summary>
        /// Executes the quantum circuit.
        /// </summary>
        /// <returns>The result statevector as a <see cref="LinearAlgebra.Vector" /></returns>
        public LinearAlgebra.Vector ExecuteCircuit()
        {
            while(executionColumns.Count > 0) 
            {
                for (int i = executionColumns[0].Count -1; i > -1; i--)
                {
                    Gate gate = executionColumns[0][i];

                    GateTypes gateType = gate.Type;
                    SparseMatrix gateMatrix = gate.Operation;
                    int[] targetQubits = gate.Targets;
                    int[] controlQubits = gate.Controls;

                    SparseMatrix fullGateMatrix;

                    switch (gateType)
                    {
                        // no operation gates and spacers for proper control bits
                        case GateTypes.NOP:
                            continue;
                        case GateTypes.CXC:
                            continue;
                        case GateTypes.TOC:
                            continue;
                        case GateTypes.SWT:
                            continue;
                        // Actual operations 
                        case GateTypes.CXT:
                            fullGateMatrix = CNOTCreation(QbitCount, controlQubits[0], targetQubits[0]);
                            break;
                        case GateTypes.TOF:
                            fullGateMatrix = ToffoliCreation(QbitCount, controlQubits[0], controlQubits[1], targetQubits[0]);
                            break;
                        case GateTypes.SWP:
                            fullGateMatrix = SwapCreation(QbitCount, controlQubits[0], targetQubits[0]);
                            break;
                        default:
                            fullGateMatrix = BuildFullGateMatrix(gateMatrix, targetQubits[0]);
                            break;
                    }

                    // print statement for debugging 
                    //Console.WriteLine("Full Gate Matrix:");
                    //fullGateMatrix.Print();

                    // Apply the resulting matrix to the state vector
                    StateVector = fullGateMatrix.MultiplyWithVector(StateVector);



                    // print statement for debugging 
                    // Console.WriteLine("stateVector:");
                    //foreach (Complex number in stateVector)
                    //{
                    //    Console.WriteLine(number);
                    //}
                }
                executionColumns.RemoveAt(0);
            }
            return new LinearAlgebra.Vector(StateVector);
        }

        /// <summary>
        /// Builds the full gate matrix. In order to build the matrix, it is necessary to tensor the gate matrix with the identity matrix for the unaffected qubits.
        /// </summary>
        /// <param name="gateMatrix">The gate operator matrix.</param>
        /// <param name="targetQubits">The target qubits.</param>
        /// <returns>A <see cref="LinearAlgebra.SparseMatrix"/> reprsentation of the tensored operator matrix.</returns>
        private SparseMatrix BuildFullGateMatrix(SparseMatrix gateMatrix, int targetQubits)
        {
            // Start with the identity matrix for the full qubit space
            SparseMatrix scalar = SparseMatrix.Identity(1);
            SparseMatrix identity = SparseMatrix.Identity(2);
            SparseMatrix[] expression = new SparseMatrix[QbitCount];

            for (int i = QbitCount-1; i > -1; i--)
            {
                if (targetQubits == i)
                {
                    // Apply the gate matrix for the target qubits
                    expression[i] = gateMatrix;
                }
                else
                {
                    // Otherwise, tensor with the identity matrix for unaffected qubits
                    expression[i] = identity;
                }
            }

            SparseMatrix result = scalar;
            for (int i = QbitCount - 1; i > -1; i--)
            {
                SparseMatrix term = expression[i];
                if (term.Rows == 1 && term.Cols == 1)
                {
                    continue; // Skip scalars, as they don't modify the state
                }
                if (QbitCount < 15)
                {
                    result = result.TensorProduct(expression[i]);
                }
                else
                {
                    result = result.ParallelTensorProduct(expression[i]);
                }

            }

            return result;
        }

        /// <summary>
        /// Creates a tensored CNOT operator matrix.
        /// </summary>
        /// <param name="gatesize">The size of the gate.</param>
        /// <param name="controlbit">The control qubit.</param>
        /// <param name="targetbit">The target qubit.</param>
        /// <returns>A <see cref="LinearAlgebra.SparseMatrix"/> representation of the tensored CNOT operator matrix</returns>
        public SparseMatrix CNOTCreation(int gatesize, int controlbit , int targetbit)
        {
            int size = (int)Math.Pow(2, gatesize);  // The size of the matrix is 2^n x 2^n whith n = QbitCount
            SparseMatrix cnotGate = new SparseMatrix(size, size);

            // Loop over all states (from 0 to 2^n - 1)
            for (int i = 0; i < size; i++)
            {
                // Convert i to a binary string + pad left the rest of the zeros
                string basisState = Convert.ToString(i, 2).PadLeft(gatesize, '0');
                char[] reversedBasisState = basisState.ToCharArray();
                Array.Reverse(reversedBasisState);

                // Get the control and target qubit values from the state
                int controlBit = reversedBasisState[controlbit] - '0';
                int targetBit = reversedBasisState[targetbit] - '0';

                if (controlBit == 1)
                {
                    // Flip the target bit if control bit is 1
                    reversedBasisState[targetbit] = (targetBit == 0) ? '1' : '0'; // Flip the target bit

                    // Reverse the modified state back to big-endian before converting to an integer for indexing 
                    Array.Reverse(reversedBasisState);
                    int newIndex = Convert.ToInt32(new string(reversedBasisState), 2);
                    cnotGate[i, newIndex] = 1;
                }
                else
                {
                    // No change if control bit is 0
                    cnotGate[i, i] = 1;
                }
            }

            return cnotGate;
        }

        /// <summary>
        /// Creates a tensored SWAP operator matrix.
        /// </summary>
        /// <param name="gatesize">The size of the gate.</param>
        /// <param name="target1">The first target qubit.</param>
        /// <param name="target2">The second target qubit.</param>
        /// <returns>A <see cref="LinearAlgebra.SparseMatrix"/> representation of the tensored SWAP operator matrix</returns>
        public SparseMatrix SwapCreation(int gatesize, int target1, int target2)
        {
            int size = (int)Math.Pow(2, gatesize);  // The size of the matrix is 2^n x 2^n with n = QbitCount
            SparseMatrix swapGate = new SparseMatrix(size, size);

            // Loop over all states (from 0 to 2^n - 1)
            for (int i = 0; i < size; i++)
            {
                // Convert i to a binary string + pad left the rest of the zeros
                string basisState = Convert.ToString(i, 2).PadLeft(gatesize, '0');
                char[] reversedBasisState = basisState.ToCharArray();
                Array.Reverse(reversedBasisState); // Use little-endian representation

                // Get the values of the two target qubits
                int targetBit1 = reversedBasisState[target1] - '0';
                int targetBit2 = reversedBasisState[target2] - '0';

                if (targetBit1 != targetBit2)
                {
                    // Swap the target bits if they are different
                    char temp = reversedBasisState[target1];
                    reversedBasisState[target1] = reversedBasisState[target2];
                    reversedBasisState[target2] = temp;

                    // Reverse the modified state back to big-endian before converting to an integer
                    Array.Reverse(reversedBasisState);
                    int newIndex = Convert.ToInt32(new string(reversedBasisState), 2);
                    swapGate[i, newIndex] = 1;
                }
                else
                {
                    // No change if the target bits are the same
                    swapGate[i, i] = 1;
                }
            }

            return swapGate;
        }

        /// <summary>
        /// Creates a tensored Toffoli operator matrix.
        /// </summary>
        /// <param name="gatesize">The size of the gate.</param>
        /// <param name="controlbit1">The first control qubit.</param>
        /// <param name="controlbit2">The second control qubit.</param>
        /// <param name="targetbit">The target qubit.</param>
        /// <returns>A <see cref="LinearAlgebra.SparseMatrix"/> representation of the tensored Tofolli operator matrix</returns>
        public SparseMatrix ToffoliCreation(int gatesize, int controlbit1, int controlbit2, int targetbit)
        {
            int size = (int)Math.Pow(2, gatesize);  // The size of the matrix is 2^n x 2^n with n = QbitCount
            SparseMatrix toffoliGate = new SparseMatrix(size, size);

            // Loop over all states (from 0 to 2^n - 1)
            for (int i = 0; i < size; i++)
            {
                // Convert i to a binary string + pad left the rest of the zeros
                string basisState = Convert.ToString(i, 2).PadLeft(gatesize, '0');
                char[] reversedBasisState = basisState.ToCharArray();
                Array.Reverse(reversedBasisState);

                // Get the control and target qubit values from the state
                int controlBit1 = reversedBasisState[controlbit1] - '0';
                int controlBit2 = reversedBasisState[controlbit2] - '0';
                int targetBit = reversedBasisState[targetbit] - '0';

                if (controlBit1 == 1 && controlBit2 == 1)
                {
                    // Flip the target bit if both control bits are 1
                    reversedBasisState[targetbit] = (targetBit == 0) ? '1' : '0'; // Flip the target bit

                    // Reverse the modified state back to big-endian before converting to an integer for indexing
                    Array.Reverse(reversedBasisState);
                    int newIndex = Convert.ToInt32(new string(reversedBasisState), 2);
                    toffoliGate[i, newIndex] = 1;
                }
                else
                {
                    // No change if either control bit is 0
                    toffoliGate[i, i] = 1;
                }
            }

            return toffoliGate;
        }

        // This method measures a single qubit in a state vector
        // This method will likely get changed and be used for partial measurements
        // Not ready for use yet
        private int MeasureEntangledQubit(int targetQubit)
        {
            int stateSize = StateVector.Length;
            int qubitCount = QbitCount;

            // Arrays to store the collapsed states for measurement 0 or 1
            Complex[] zeroState = new Complex[stateSize];
            Complex[] oneState = new Complex[stateSize];

            // Variables to store the probabilities of measuring 0 or 1
            double probabilityZero = 0.0;
            double probabilityOne = 0.0;

            // Loop over all basis states
            for (int i = 0; i < stateSize; i++)
            {
                // Check the value of the target qubit in the basis state 'i'
                bool isTargetQubitZero = ((i >> targetQubit) & 1) == 0;

                if (isTargetQubitZero)
                {
                    // Group states where target qubit is 0
                    probabilityZero += StateVector[i].Magnitude * StateVector[i].Magnitude;
                    zeroState[i] = StateVector[i];
                }
                else
                {
                    // Group states where target qubit is 1
                    probabilityOne += StateVector[i].Magnitude * StateVector[i].Magnitude;
                    oneState[i] = StateVector[i];
                }
            }

            // Normalize the probabilities
            probabilityZero = Math.Sqrt(probabilityZero);
            probabilityOne = Math.Sqrt(probabilityOne);

            // Randomly select 0 or 1 based on their probabilities
            double randomValue = new Random().NextDouble();
            if (randomValue < probabilityZero)
            {
                // Collapse to the |0⟩ state
                StateVector = Normalize(zeroState);
                return 0;
            }
            else
            {
                // Collapse to the |1⟩ state
                StateVector = Normalize(oneState);
                return 1;
            }
        }

        /// <summary>
        /// Measures all qubits.
        /// </summary>
        /// <returns>An array of <see cref="System.Byte"/> of bits for the measured state.</returns>
        public byte[] MeasureAllQubits()
        {
            int qubitCount = QbitCount;
            int stateSize = StateVector.Length;

            // Array to store measurement results
            byte[] measurementResults = new byte[qubitCount];

            // Calculate the probabilities for each basis state
            double[] probabilities = GetStateProbabilities();

            // Select a basis state randomly based on the probabilities
            double randomValue = new Random().NextDouble();
            double cumulativeProbability = 0.0;
            int selectedState = 0;
            for (int i = 0; i < stateSize; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue < cumulativeProbability)
                {
                    selectedState = i;
                    break;
                }
            }

            // Convert the selected basis state (integer) to a bitstring for measurement results
            for (int qubit = 0; qubit < qubitCount; qubit++)
            {
                measurementResults[qubit] = (byte)((selectedState >> qubit) & 1); // Extract the qubit values
            }

            // Collapse the state to the selected basis state
            for (int i = 0; i < stateSize; i++)
            {
                if (i == selectedState)
                {
                    StateVector[i] = new Complex(1, 0); // Set the selected state to amplitude 1
                }
                else
                {
                    StateVector[i] = new Complex(0, 0); // Set all other states to amplitude 0
                }
            }

            return measurementResults;
        }

        /// <summary>
        /// Normalizes the specified statevector.
        /// </summary>
        /// <param name="state">The statevector</param>
        /// <returns>A normalized statevector as an array of <see cref="System.Numerics.Complex" /></returns>
        private static Complex[] Normalize(Complex[] state)
        {
            double norm = 0;
            for (int i = 0; i < state.Length; i++)
            {
                norm += state[i].Magnitude * state[i].Magnitude;
            }
            norm = Math.Sqrt(norm);
            for (int i = 0; i < state.Length; i++)
            {
                state[i] /= norm;
            }
            return state;
        }

        /// <summary>
        /// Calculates the probability distribution for the entire system.
        /// </summary>
        /// <returns>An array of doubles representing the probability of each basis state.</returns>
        public double[] GetStateProbabilities()
        {
            int stateSize = StateVector.Length;
            double[] probabilities = new double[stateSize];
            for (int i = 0; i < stateSize; i++)
            {
                probabilities[i] = StateVector[i].Magnitude * StateVector[i].Magnitude;
            }
            return probabilities;
        }

        /// <summary>
        /// Calculates the bitstring for a given index input.
        /// </summary>
        /// <param name="index">A number that we need to convert to a bitstring.</param>
        /// <returns>A string representing the bitstring of the inputted index.</returns>
        private string GetBitstring(int index)
        {
            // the index of the basis state simulated gets converted to base 2, and the left is padded
            // so that it is of size QbitCount with 0's, which is how our system handles the basis representation
            return Convert.ToString(index, 2).PadLeft(QbitCount, '0');
        }

        /// <summary>
        /// Simulates measurements on the entire quantum system.
        /// </summary>
        /// <param name="iterations">The number of simulations to perform.</param>
        /// <returns>A list of bitstrings representing the measurement outcomes.</returns>
        public List<string> SimulateMeasurements(int iterations = 1)
        {
            int stateSize = StateVector.Length;

            double[] probabilities = GetStateProbabilities();

            // make it a cumulative distribution for simulations
            double[] cumulativeProbabilities = new double[stateSize];
            cumulativeProbabilities[0] = probabilities[0];
            for (int i = 1; i < stateSize; i++)
            {
                cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + probabilities[i];
            }

            // fix floating point issues
            cumulativeProbabilities[stateSize - 1] = 1.0;

            // store bitstrings as they are generated
            List<string> measurementResults = new List<string>();

            Random random = new Random();

            for (int i = 0; i < iterations; i++)
            {
                // generate a random double between 0 and 1, search for where it is/should be in the CDF array
                // to simulate picking one based on their respective probabilities
                double randomValue = random.NextDouble();
                int stateIndex = Array.BinarySearch(cumulativeProbabilities, randomValue);
                if (stateIndex < 0)
                {
                    stateIndex = ~stateIndex; // e.g. -3 becomes 2, bin search returns where it would go but negative if not in the array
                }

                string bitstring = GetBitstring(stateIndex);

                measurementResults.Add(bitstring);
            }

            return measurementResults;
        }

        /// <summary>
        /// Prints simulated measurement bitstring(s) to the console.
        /// </summary>
        /// <param name="iterations">The number of simulations to perform.</param>
        public void PrintBitstrings(int iterations = 1)
        {
            List<string> measurementResults = SimulateMeasurements(iterations);
            Console.WriteLine("Bitstrings:\n");
            foreach (string bitstring in measurementResults)
            {
                Console.WriteLine(bitstring);
            }
        }

        /// <summary>
        /// Prints a sideways histogram based on the probabilities of each basis state alongside their respective probability.
        /// </summary>
        public void PrintHistogram(int bars = 100)
        {
            int stateSize = StateVector.Length;

            double[] probabilities = GetStateProbabilities();

            Console.WriteLine("Probability Histogram:\n");
            for (int i = 0; i < stateSize; i++)
            {
                string bitstring = GetBitstring(i);

                double probability = probabilities[i];

                // scale the bar length based off the probability
                int numBars = (int)Math.Round(probability * bars);

                string formattedProbability = $"{probability:P0}".PadLeft(3);

                Console.Write($"{bitstring}   |   ");
                Console.WriteLine(new string('#', numBars) + $" {formattedProbability}");
            }
        }

        /// <summary>
        /// Prints a sideways histogram of simulated measurement results, normalized to a specified number of bars.
        /// </summary>
        /// <param name="iterations">The number of simulations to perform.</param>
        /// <param name="bars">The total number of hyphens to display in the histogram.</param>
        public void SimulateHistogram(int iterations = 1000, int bars = 100)
        {
            int stateSize = StateVector.Length;

            List<string> measurementResults = SimulateMeasurements(iterations);

            // generate all possible bitstrings and initialize them to 0
            Dictionary<string, int> measurementCounts = new Dictionary<string, int>();
            for (int i = 0; i < stateSize; i++)
            {
                string bitstring = GetBitstring(i);
                measurementCounts[bitstring] = 0;
            }

            // each bitstring is initialized to 0 and in the dictionary, now count them
            foreach (string bitstring in measurementResults)
            {
                measurementCounts[bitstring]++;
            }

            Console.WriteLine("Measurement Histogram:\n");
            foreach (var kvp in measurementCounts.OrderBy(kvp => kvp.Key))
            {
                string bitstring = kvp.Key;
                int count = kvp.Value;

                // normalize the count based off the #bars and count of bitstrings (=iterations) for each count
                int numBars = (int)Math.Round((double)count * bars / iterations);

                string formattedPercentage = $"{(double)count / iterations:P0}".PadLeft(3);

                Console.Write($"{bitstring}   |   ");
                Console.WriteLine(new string('#', numBars) + $" {formattedPercentage}");
            }
        }

        /// <summary>
        /// Checks if the provided object is null.
        /// </summary>
        /// <param name="obj">The object we check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        private void ValidateNotNull(object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
            }
        }

        /// <summary>
        /// Checks if the sizes of two collections match.
        /// </summary>
        /// <param name="size1">Size of the first collection.</param>
        /// <param name="size2">Size of the second collection.</param>
        /// <param name="errorMessage">Custom error message to display if the sizes do not match.</param>
        private void ValidateSizeMatch(int size1, int size2, string errorMessage)
        {
            if (size1 != size2)
            {
                throw new ArgumentException($"{errorMessage} (Sizes: {size1} and {size2})");
            }
        }

        /// <summary>
        /// Gets the operator matrix for a given character.
        /// </summary>
        /// <param name="gateSymbol">The character representing the gate X, Y, Z and I are all that are supported.</param>
        /// <returns>The SparseMatrix representing the operator matrix of the gate.</returns>
        private SparseMatrix GetGateMatrix(char gateSymbol)
        {
            switch (gateSymbol)
            {
                // matrices for these are intrinsic so just use generalized target qubit 0
                case 'I':
                    return new NOP(0, GateTypes.NOP).Operation;
                case 'X':
                    return new X(0).Operation;
                case 'Y':
                    return new Y(0).Operation;
                case 'Z':
                    return new Z(0).Operation;
                default:
                    throw new ArgumentException($"Ruh roh '{gateSymbol}' is in observable string. Only 'X', 'Y', 'Z', and 'I' are supported.");
            }
        }


        /// <summary>
        /// Calculates expectation value using inputted observable matrix and the statevector.
        /// </summary>
        /// <param name="observable">The observable matrix used to perform calculations.</param>
        /// <param name="decimalPlaces">Optional parameter to control the number of decimal places it rounds to, if negative don't round.</param>
        /// <returns>A double representing the calculated expectation value after observing with the observable.</returns>
        public double GetExpectationValue(SparseMatrix observable, int decimalPlaces=6)
        {
            // error checking
            ValidateNotNull(observable, nameof(observable));

            // O * |psi>
            Complex[] Opsi = observable.MultiplyWithVector(StateVector);

            // <psi| (O |psi>)
            Complex expectationValue = new Complex(0, 0);
            for (int i = 0; i < StateVector.Length; i++)
            {
                expectationValue += Complex.Conjugate(StateVector[i]) * Opsi[i];
            }

            // if -1, return unrounded value, else, return value rounded to decimalPlaces many digits
            return decimalPlaces < 0 ? expectationValue.Real : Math.Round(expectationValue.Real, decimalPlaces);
        }

        /// <summary>
        /// Calculates expectation value using inputted observable matrix string representation and the statevector.
        /// </summary>
        /// <param name="observable">A string consisting of a sequence of gates to represent the observable. Big-Endian for ease of use.</param>
        /// <param name="decimalPlaces">Optional parameter to control the number of decimal places it rounds to, if negative don't round.</param>
        /// <returns>A double representing the calculated expectation value after observing with the observable.</returns>
        public double GetExpectationValue(string observable, int decimalPlaces=6)
        {
            ValidateNotNull(observable, nameof(observable));
            ValidateSizeMatch(observable.Length, QbitCount, "Observable string does not have 1 gate per qubit in the Hilbert space.");

            // initialize to identity scalar
            SparseMatrix fullObservable = new SparseMatrix(new Complex[,] { { 1 } });

            // again, Big-Endian processing since it is easier to input, result is the same
            for (int i = 0; i < observable.Length; i++)
            {
                char gateSymbol = observable[i];
                SparseMatrix gateMatrix = GetGateMatrix(gateSymbol); // only X Y Z and I supported 

                // update observable with new gate matrix representation
                fullObservable = fullObservable.TensorProduct(gateMatrix);
            }

            return GetExpectationValue(fullObservable, decimalPlaces);
        }

        /// <summary>
        /// Calculates expectation values for each inputted observable string using the statevector and other overloaded functions.
        /// </summary>
        /// <param name="observables">A string array of observable matrix string representations.</param>
        /// <param name="decimalPlaces">Optional parameter to control the number of decimal places it rounds to, if negative don't round.</param>
        /// <returns>A list of doubles representing the expectation value for the corresponding observable.</returns>
        public List<double> GetExpectationValue(string[] observables, int decimalPlaces=6)
        {
            ValidateNotNull(observables, nameof(observables));

            List<double> results = new List<double>();

            foreach (string observable in observables)
            {
                double expectation = GetExpectationValue(observable, decimalPlaces);
                results.Add(expectation);
            }

            return results;
        }

        /// <summary>
        /// Calculates the linear combination expectation value.
        /// </summary>
        /// <param name="coefficients">An array of coefficients corresponding to each observable.</param>
        /// <param name="observables">An array of observable string representations.</param>
        /// <param name="decimalPlaces">Optional parameter to control the number of decimal places it rounds to, if negative don't round.</param>
        /// <returns>The expectation value of the linear combination.</returns>
        public double GetExpectationValue(double[] coefficients, string[] observables, int decimalPlaces = 6)
        {
            // error checking
            ValidateNotNull(coefficients, nameof(coefficients));
            ValidateSizeMatch(coefficients.Length, observables.Length, "Coefficient list size and observable array size must match.");

            List<double> expectationValues = GetExpectationValue(observables, -1); // use unrounded in the intermediate step for more precision, round later 

            double linCombinationExpectationValue = 0.0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                linCombinationExpectationValue += coefficients[i] * expectationValues[i];
            }

            // round now
            if (decimalPlaces >= 0)
            {
                linCombinationExpectationValue = Math.Round(linCombinationExpectationValue, decimalPlaces);
            }

            return linCombinationExpectationValue;
        }

        /// <summary>
        /// Prints the expectation values for an inputted array of observable string representations.
        /// </summary>
        /// <param name="observables">A string array of observable matrix string representations.</param>
        /// <param name="decimalPlaces">Optional parameter to control the number of decimal places it rounds to, if negative don't round.</param>
        public void PrintExpectationValues(string[] observables, int decimalPlaces = 6)
        {
            List<double> expectationValues = GetExpectationValue(observables, decimalPlaces);

            Console.WriteLine("Observable   | Expectation Value");
            Console.WriteLine(new string('-', 30));

            for (int i = 0; i < observables.Length; i++)
            {
                Console.WriteLine($"{observables[i].PadRight(12)} | {expectationValues[i]}");
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Circuit Execution:");

            // Append qubit states
            sb.AppendLine("Qubits StateVector: ");

            foreach (Complex number in StateVector)
            {
                sb.AppendLine(number.ToString());
            }

            sb.Append(StateVector.ToString());

            sb.Append("\n");

            for (int i = 0; i < executionColumns.Count; i++)
            {
                sb.Append($"Column {i + 1}:\n");
                foreach (Gate gate in executionColumns[i])
                {
                    sb.Append(gate.ToString() + "\t");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

    }
}
