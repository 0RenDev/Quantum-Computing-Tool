using LinearAlgebra;
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

        private SparseMatrix BuildFullGateMatrix(SparseMatrix gateMatrix, int control, int[] targetQubits)
        {
            // Start with the identity matrix for the full qubit space
            SparseMatrix result = SparseMatrix.Identity(1);

            for (int i = 0; i < QbitCount; i++)
            {
                if (control == i)
                {
                    // If this qubit is affected by the gate, tensor the gate matrix
                    result = result.TensorProduct(gateMatrix);
                }
                else if (targetQubits.Contains(i))
                {
                    continue;
                }
                else
                {
                    // Otherwise, tensor with the identity matrix
                    result = result.TensorProduct(SparseMatrix.Identity(2));
                }
            }

            return result;
        }

        // Apply gate on the state vector
        public void ApplyGate(Gate gate)
        {
            SparseMatrix gateMatrix = gate.Operation;
            int[] targetQubits = gate.Targets;

            // Build the tensor product of the gate and identities for unaffected qubits
            SparseMatrix fullGateMatrix = BuildFullGateMatrix(gateMatrix, gate.Control, targetQubits);

            // Apply the resulting matrix to the state vector
            State = fullGateMatrix.MultiplyWithVector(State);
        }
    }
}
