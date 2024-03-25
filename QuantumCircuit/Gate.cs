using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using LinearAlgebra;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit
{
    internal class Gate
    {
        String gateType;
        Matrix operation;

        // The following are for the likes of CNOT or swap. 
        // Just a thought.
        String rootName;
        int rootIndex;
        String targetName;
        int targetIndex;

        public Gate(String gateType, Matrix operation)
        {
            this.gateType = gateType;
            this.operation = operation;
        }

        public String getGateType()
        {
            return gateType;
        }
    }
}
