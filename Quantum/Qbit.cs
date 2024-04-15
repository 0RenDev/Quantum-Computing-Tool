using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Schema;
using LinearAlgebra;
using Vector = LinearAlgebra.Vector;

//-------------------------------------------------------------------------------------------------------------------------------------------------------------
// This is the Qbit class representing an instance of the Qbit
// it has the following methods: Evolve, ToString, IsValid, IsEqual (check each method for more detailed descriptions)
//-------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace Quantum
{
    public class Qbit
    {
        private Vector data;

        public Vector Data { get { return data; } }

        public Qbit(Complex[] stateVector)
        {

            switch(IsValid(stateVector))
            {
                case 0: break;
                case 1: throw new ArgumentException("A Qbit must contain only two probability amplitudes."); 
                case 2: throw new ArgumentException("The sum of the squares of probability amplitudes must be equal to one.");
                default: break;
            }

            data = new Vector(stateVector);
            
        }

        // Evolves a Quantum State Vector (Qbit) using an Operator
        public void Evolve(Operator @operator)
        {
            Vector evolvedState = Operations.MatrixVectorMult(@operator.Data, this.data);
            Console.WriteLine(evolvedState);
            data = evolvedState;
        }

        // Override of to String
        public override string ToString()
        {
            return data.ToString();
        }

        private static ushort IsValid(Complex[] statevector)
        {
            if(statevector.Length != 2)
            {
                return 1;
            } else if (Math.Round((statevector[0] * Complex.Conjugate(statevector[0]) + statevector[1] * Complex.Conjugate(statevector[1])).Real, 1) != 1.0)
            {
                Console.WriteLine(statevector[0] * Complex.Conjugate(statevector[0]) + statevector[1] * Complex.Conjugate(statevector[1]));
                return 2;
            } else
            {
                return 0;
            }
        }

        public static bool IsEqual(Qbit qbit1, Qbit qbit2)
        {
            return qbit1.data.Equals(qbit2.data);
        }
    }
}
