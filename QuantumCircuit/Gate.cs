using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using LinearAlgebra;
using System.Text;
using System.Threading.Tasks;
using Quantum;

namespace QuantumCircuit
{
    internal class Gate
    {
        protected string gateType;
        protected Operator operation;

        // The following are for the likes of CNOT or swap. 
        // Just a thought.
        string rootName;
        int rootIndex;
        string targetName;
        int targetIndex;

        public Gate(string gateType, Operator operation)
        {
            this.gateType = gateType;
            this.operation = operation;
        }

        public virtual string GetGateType()
        {
            return gateType;
        }
    }

    internal class MultiLineGateRoot : Gate
    {
        // Additional properties for MultiLineGate
        protected string[] targets;
        protected int[] targetsIndexes;

        public MultiLineGateRoot(string gateType, Operator operation, string[] targets, int[] targetsIndexes)
            : base(gateType, operation)
        {
            this.targetsIndexes = targetsIndexes;
            this.targets = targets;
        }

        public override string GetGateType()
        {
            string returnString = "[" + gateType + " targetting at:";
            for (int i = 0; i < targets.Length; i++)
            {
                returnString += " " + targets[i] + ":" + targetsIndexes[i];
            }
            return returnString + "]";
        }
    }
    internal class MultiLineGateTarget : Gate
    {
        // Additional properties for MultiLineGate
        protected String source;
        protected int sourceIndex;

        public MultiLineGateTarget(string gateType, Operator operation, string source, int sourceIndex)
            : base(gateType, operation)
        {
            this.source = source;
            this.sourceIndex = sourceIndex;
        }

        public override string GetGateType()
        {
            String returnString = "[" + gateType + " from " + source + sourceIndex + "]";
            return returnString;
        }


    }
}
