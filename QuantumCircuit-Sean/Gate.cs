using LinearAlgebra;
using System.Numerics;

namespace QuantumCircuit_Sean
{
    public class Gate(SparseMatrix operation, int control)
    {
        public SparseMatrix Operation { get; private set; } = operation;
        public int Control { get; private set; } = control;
        public int[] Targets { get; protected set; } = [];
        public bool IsControlled { get; protected set; } = false;
    }

    public class X(int control) : Gate(new SparseMatrix(new Complex[,] { { 0, 1 }, { 1, 0 } }), control)
    {

    }

    public class H(int control) : Gate(new SparseMatrix(new Complex[,] { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) }, { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } }), control)
    {

    }

    public class CX: Gate
    {

        public CX(int control, int target) : base(new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 1, 0 } }), control)
        {
            Targets = [target];
            IsControlled = true;
        }
    }
}