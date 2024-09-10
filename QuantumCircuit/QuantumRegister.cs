using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit
{
    public class QuantumRegister
    {
        public Complex[] State { get; private set; }
        public int QbitCount { get; private set; }

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
    }
}
