using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit2
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

        public void ApplyGate(Gate gate)
        {
            if (gate.Target >= QbitCount)
            {
                throw new ArgumentException("Invalid qubit index");
            }

            if (gate.Operation.Rows != StateLength || gate.Operation.Cols != StateLength)
            {
                throw new ArgumentException("Invalid gate size");
            }

            Complex[] newState = new Complex[StateLength];
            for (int i = 0; i < StateLength; i++)
            {
                Complex sum = 0;
                for (int j = 0; j < StateLength; j++)
                {
                    sum += gate.Operation[i, j] * State[j];
                }
                newState[i] = sum;
            }

            State = newState;
        }   
    }
}
