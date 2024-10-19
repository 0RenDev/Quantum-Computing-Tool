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
    public class CircuitExecution
    {

        QuantumCircuitBuilder circuit;
        List<List<Gate>> executionColumns = new List<List<Gate>>();

        public int QbitCount { get; private set; }
        public Complex[] stateVector { get; private set; }

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

            stateVector = new Complex[1 << circuit.quantumLines.Length];
            stateVector[0] = 1;


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
                        Gate spacerNoOp = new NOP(j);
                        columnOfGates.Add(spacerNoOp);
                    }
                }
                executionColumns.Add(columnOfGates);
            }
        }


        public LinearAlgebra.Vector ExecuteCircuit()
        {
            while(executionColumns.Count > 0) 
            {
                for (int i = executionColumns[0].Count -1; i > -1; i--)
                {
                    Gate gate = executionColumns[0][i];

                    GateTypes gateType = gate.type;
                    SparseMatrix gateMatrix = gate.Operation;
                    int[] targetQubits = gate.Targets;
                    int[] controlQubits = gate.Controls;

                    SparseMatrix fullGateMatrix;

                    switch (gateType)
                    {
                        case GateTypes.NOP:
                            continue;
                        case GateTypes.CXC:
                            fullGateMatrix = CNOTCreation(QbitCount, controlQubits[0], targetQubits[0]);
                            break;
                        case GateTypes.TOF:
                            fullGateMatrix = ToffoliCreation(QbitCount, controlQubits[0], controlQubits[1], targetQubits[0]);
                            break;
                        default:
                            fullGateMatrix = BuildFullGateMatrix(gateMatrix, targetQubits[0]);
                            break;
                    }

                    // print statement for debugging 
                    //Console.WriteLine("Full Gate Matrix:");
                    //fullGateMatrix.Print();

                    // Apply the resulting matrix to the state vector
                    stateVector = fullGateMatrix.MultiplyWithVector(stateVector);



                    // print statement for debugging 
                    // Console.WriteLine("stateVector:");
                    //foreach (Complex number in stateVector)
                    //{
                    //    Console.WriteLine(number);
                    //}
                }
                executionColumns.RemoveAt(0);
            }
            return new LinearAlgebra.Vector(stateVector);
        }



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

        // add SwapCreation method 
        // might be similar to CNOT


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

        public int MeasureEntangledQubit(int targetQubit)
        {
            int stateSize = stateVector.Length;
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
                    probabilityZero += stateVector[i].Magnitude * stateVector[i].Magnitude;
                    zeroState[i] = stateVector[i];
                }
                else
                {
                    // Group states where target qubit is 1
                    probabilityOne += stateVector[i].Magnitude * stateVector[i].Magnitude;
                    oneState[i] = stateVector[i];
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
                stateVector = Normalize(zeroState);
                return 0;
            }
            else
            {
                // Collapse to the |1⟩ state
                stateVector = Normalize(oneState);
                return 1;
            }
        }

        public int[] MeasureAllQubits()
        {
            int qubitCount = QbitCount;
            int stateSize = stateVector.Length;

            // Array to store measurement results
            int[] measurementResults = new int[qubitCount];

            // Calculate the probabilities for each basis state
            double[] probabilities = new double[stateSize];
            for (int i = 0; i < stateSize; i++)
            {
                probabilities[i] = stateVector[i].Magnitude * stateVector[i].Magnitude;
            }

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
                measurementResults[qubit] = (selectedState >> qubit) & 1; // Extract the qubit values
            }

            // Collapse the state to the selected basis state
            for (int i = 0; i < stateSize; i++)
            {
                if (i == selectedState)
                {
                    stateVector[i] = new Complex(1, 0); // Set the selected state to amplitude 1
                }
                else
                {
                    stateVector[i] = new Complex(0, 0); // Set all other states to amplitude 0
                }
            }

            return measurementResults;
        }



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

        // Override the toString method
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Circuit Execution:");

            // Append qubit states
            sb.AppendLine("Qubits StateVector: ");

            foreach (Complex number in stateVector)
            {
                sb.AppendLine(number.ToString());
            }

            sb.Append(stateVector.ToString());

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
