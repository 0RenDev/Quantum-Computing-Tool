using LinearAlgebra;
using System.Numerics;

namespace QuantumCircuit_Sean
{
    public class Gate(SparseMatrix operation, int[] controls, int[] targets)
    {
        public SparseMatrix Operation { get; private set; } = operation;
        public int[] Controls { get; protected set; } = controls;
        public int[] Targets { get; protected set; } = targets;
    }

    public class X(int target) : Gate(new SparseMatrix(new Complex[,] { { 0, 1 }, 
                                                                        { 1, 0 } }), [], [target])
    {

    }

    public class Y(int target) : Gate(new SparseMatrix(new Complex[,] { { 0, -Complex.ImaginaryOne },
                                                                        { Complex.ImaginaryOne, 0 } }), [], [target])
    {

    }

    public class Z(int target) : Gate(new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                        { 0, -1 } }), [], [target])
    {

    }

    public class T(int target) : Gate(new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                        { 0, Complex.Exp(Complex.ImaginaryOne * Math.PI / 4) } }), [], [target])
    {

    }

    public class H(int target) : Gate(new SparseMatrix(new Complex[,] { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) }, 
                                                                        { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } }), [], [target])
    {

    }

    public class CX(int control, int target) : Gate(new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                                      { 0, 1, 0, 0 },
                                                                                      { 0, 0, 0, 1 },
                                                                                      { 0, 0, 1, 0 } }), [control], [target])
    {

    }

    public class SWAP(int target1, int target2) : Gate(new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                                         { 0, 0, 1, 0 },
                                                                                         { 0, 1, 0, 0 },
                                                                                         { 0, 0, 0, 1 } }), [target2], [target1])
    {

    }

    public class Toff(int control1, int control2, int target) : Gate(new SparseMatrix(new Complex[,] { { 1, 0, 0, 0, 0, 0, 0, 0 },
                                                                                                       { 0, 1, 0, 0, 0, 0, 0, 0 },
                                                                                                       { 0, 0, 1, 0, 0, 0, 0, 0 },
                                                                                                       { 0, 0, 0, 1, 0, 0, 0, 0 },
                                                                                                       { 0, 0, 0, 0, 1, 0, 0, 0 },
                                                                                                       { 0, 0, 0, 0, 0, 1, 0, 0 },
                                                                                                       { 0, 0, 0, 0, 0, 0, 0, 1 },
                                                                                                       { 0, 0, 0, 0, 0, 0, 1, 0 } }), [control1, control2], [target])
    {

    }
}