using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Collections.Generic;

namespace QuantumCircuit2
{
    public class QuantumCirucit
    {
        private int Qubits { get; }
        private int ClassicalBits { get; }
        Queue<Gate> Gates = new Queue<Gate>();

        public QuantumCirucit(int qubits, int classicalBits)
        {
            Qubits = qubits;
            ClassicalBits = classicalBits;
        }

        public void AddGate(Gate gate)
        {
            Gates.Enqueue(gate);
        }

        public void Execute(QuantumRegister register)
        {
            foreach (var gate in Gates)
            {
                register.ApplyGate(gate);
            }
        }
    }
}
