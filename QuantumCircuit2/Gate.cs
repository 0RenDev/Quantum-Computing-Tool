using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit2
{
    public class Gate(SparseMatrix operation, int target)
    {
        public SparseMatrix Operation { get; private set; } = operation;
        public int Target { get; private set; } = target;
    }

    public class X(int target) : Gate(new SparseMatrix(new Complex[,] { { 0, 1 }, { 1, 0 } }), target)
    {

    }

    public class CX(int control, int target) : Gate(new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 1, 0 } }), target)
    {
        public int Control { get; private set; } = control;
    }
}
