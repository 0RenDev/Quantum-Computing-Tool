using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using LinearAlgebra;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit
{
    public class Gate
    {
        protected String gateType;
        protected Matrix operation;

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

        public virtual string GetGateType()
        {
            return gateType;
        }

        public Matrix GetOperator()
        {
            return operation;
        }

        public virtual Complex[] Apply(Complex[] state)
        {
            return Operations.MatrixVectorMult(operation, new LinearAlgebra.Vector(state)).GetState();
        }
    }

    public class CNOT : Gate
    {
        private int TargetIndex { get; }
        public CNOT(int targetIndex) : base("CNOT", new Matrix(new Complex[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 1, 0 } }))
        {
            this.TargetIndex = targetIndex;
        }
    }

    internal class MultiLineGateRoot : Gate
    {
        // Additional properties for MultiLineGate
        protected String[] targets;
        protected int[] targetsIndexes;

        public MultiLineGateRoot(String gateType, Matrix operation, String[] targets, int[] targetsIndexes)
            : base(gateType, operation)
        {
            this.targetsIndexes = targetsIndexes;
            this.targets = targets;
        }

        // returns string representation of the gate, giving type and target qbits, O(n) with minimal memory usage
        public override string GetGateType()
        {
            String returnString = "[" + gateType + " targetting at:";
            for (int i = 0; i < targets.Length; i++)
            {
                returnString += " " + targets[i] + targetsIndexes[i];
            }
            return returnString + "]";
        }
    }
    internal class MultiLineGateTarget : Gate
    {
        // Additional properties for MultiLineGate
        protected String[] sources;
        protected int[] sourceIndexes;

        public MultiLineGateTarget(string gateType, Matrix operation, string[] sources, int[] sourceIndexes)
            : base(gateType, operation)
        {
            this.sources = sources;
            this.sourceIndexes = sourceIndexes;
        }

        public override string GetGateType()
        {
            String returnString = "[" + gateType + " from:";
            for (int i = 0; i < sources.Length; i++)
            {
                returnString += " " + sources[i] + sourceIndexes[i];
            }
            return returnString + "]";
        }
    }
}
