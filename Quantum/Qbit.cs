using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Schema;
using LinearAlgebra;
using Vector = LinearAlgebra.Vector;

namespace Quantum
{
    public class Qbit(Complex[] stateVector)
    {
        private Vector data = new Vector(stateVector);

        // Evolves a Quantum State Vector (Qbit) using an Operator
        public void Evolve(Operator @operator)
        {
            Vector evolvedState = Operations.MatrixVectorMult(@operator.Data, this.data);

            data = evolvedState;
        }

        // Override of to String
        public override string ToString()
        {
            return data.ToString();
        }
    }
}
