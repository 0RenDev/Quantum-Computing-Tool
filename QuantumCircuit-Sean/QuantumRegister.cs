using LinearAlgebra;
using Microsoft.Win32;
using System.Numerics;

namespace QuantumCircuit_Sean
{
    public class QuantumRegister
    {
        public Complex[] State { get; private set; }
        public int QbitCount { get; private set; }
        private int StateLength => 1 << QbitCount;

        public QuantumRegister(int qbitCount)
        {
            QbitCount = qbitCount;
            State = new Complex[1 << qbitCount];
            State[0] = 1;
        }

        public QuantumRegister(Complex[] state)
        {
            if (state.Length == 0 || state.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid state vector length");
            }

            if (state.Any(x => x.Magnitude != 1))
            {
                throw new ArgumentException("Invalid state vector");
            }

            State = state;
            QbitCount = (int)Math.Log(state.Length, 2);
        }

        public void Update(Complex[] state)
        {
            if (state.Length != StateLength)
            {
                throw new ArgumentException("Invalid state vector length");
            }
            State = state;
        }

        private SparseMatrix BuildFullGateMatrix(SparseMatrix gateMatrix, int[] controlQubits, int[] targetQubits)
        {
            // Start with the identity matrix for the full qubit space
            SparseMatrix scalar = SparseMatrix.Identity(1);
            SparseMatrix identity = SparseMatrix.Identity(2);
            SparseMatrix[] expression = new SparseMatrix[QbitCount];

            for (int i = 0; i < QbitCount; i++)
            {
                if (controlQubits.Contains(i))
                {
                    // This is the control qubit, so we tensor the scalar or identity depending on its state.
                    // The gate is applied if the control qubit is in state |1>
                    expression[i] = scalar;  // Leave it as scalar for control logic
                }
                else if (targetQubits.Contains(i))
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
            for (int i = 0; i < QbitCount; i++)
            {
                SparseMatrix term = expression[i];
                if (term.Rows == 1 && term.Cols == 1)
                {
                    continue; // Skip scalars, as they don't modify the state
                }
                result = result.TensorProduct(expression[i]);
            }

            return result;
        }

        // Apply gate on the state vector
        public void ApplyGate(Gate gate)
        {
            SparseMatrix gateMatrix = gate.Operation;
            int[] targetQubits = gate.Targets;
            int[] controlQubits = gate.Controls;

            // Build the tensor product of the gate and identities for unaffected qubits
            SparseMatrix fullGateMatrix = BuildFullGateMatrix(gateMatrix, controlQubits, targetQubits);
            // fullGateMatrix.Print();

            // Apply the resulting matrix to the state vector
            State = fullGateMatrix.MultiplyWithVector(State);
            /**
            foreach (Complex number in State)
            {
                Console.WriteLine(number);
            }
            **/
        }

        public double[] ProbabilityVector()
        {
            double[] probabilities = new double[StateLength];

            // Calculate probabilities for each basis state
            for (int i = 0; i < StateLength; i++)
            {
                probabilities[i] = Math.Pow(State[i].Magnitude, 2);
            }

            // Normalize the probabilities to ensure they sum to 1
            double sum = probabilities.Sum();
            if (sum > 0)
            {
                for (int i = 0; i < StateLength; i++)
                {
                    probabilities[i] /= sum;
                }
            }

            return probabilities;
        }
    }
}
