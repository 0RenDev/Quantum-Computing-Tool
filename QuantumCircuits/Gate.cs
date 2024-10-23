using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuits
{
        public enum GateTypes
        {
            XGT, // Not Gate
            YGT,
            ZGT, 
            TGT,
            HGT, // Hadamard Gate
            CXT, // Controlled Not Gate target
            CXC, // Control of CNOT gate
            SWP, // swap gate target one
            SWT, // second target of swap
            TOF, // Toffoli Gate
            TOC, // controls of toffoli gate
            NOP, // No Operation or Identity Gate
        }

    public class Gate(GateTypes type, SparseMatrix operation, int[] controls, int[] targets)
        {
            public GateTypes type { get; private set; } = type;
            public SparseMatrix Operation { get; private set; } = operation;
            public int[] Controls { get; protected set; } = controls;
            public int[] Targets { get; protected set; } = targets;
            override public String ToString()
            {
            return type switch
            {
                GateTypes.XGT => "XGT",
                GateTypes.YGT => "YGT",
                GateTypes.ZGT => "ZGT",
                GateTypes.TGT => "TGT",
                GateTypes.HGT => "HGT",
                GateTypes.CXC => "CNC",
                GateTypes.CXT => " C ",
                GateTypes.SWP => "SWP",
                GateTypes.SWT => " T ",
                GateTypes.TOF => "TOF",
                GateTypes.TOC => " C ",
                GateTypes.NOP => "   ",
                _ => type.ToString()
            };
        }
    }

    public class NOP(int target, GateTypes type) : Gate(type, new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                        { 0, 1 } }), [], [target])
    {

    }
    public class X(int target) : Gate(GateTypes.XGT, new SparseMatrix(new Complex[,] { { 0, 1 },
                                                                        { 1, 0 } }), [], [target])
    {
           
    }

    public class Y(int target) : Gate(GateTypes.YGT, new SparseMatrix(new Complex[,] { { 0, -Complex.ImaginaryOne },
                                                                    { Complex.ImaginaryOne, 0 } }), [], [target])
    {

    }

    public class Z(int target) : Gate(GateTypes.ZGT, new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                    { 0, -1 } }), [], [target])
    {

    }

    public class T(int target) : Gate(GateTypes.TGT, new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                    { 0, Complex.Exp(Complex.ImaginaryOne * Math.PI / 4) } }), [], [target])
    {

    }

    public class H(int target) : Gate(GateTypes.HGT, new SparseMatrix(new Complex[,] { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) },
                                                                    { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } }), [], [target])
    {

    }

    public class CX(int control, int target) : Gate(GateTypes.CXT, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                                                     { 0, 1, 0, 0 },
                                                                                                     { 0, 0, 0, 1 },
                                                                                                     { 0, 0, 1, 0 } }), [control], [target])
    {

    }

    public class SWAP(int target1, int target2) : Gate(GateTypes.SWP, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                                                        { 0, 0, 1, 0 },
                                                                                                        { 0, 1, 0, 0 },
                                                                                                        { 0, 0, 0, 1 } }), [target1], [target2])
    {

    }

    public class Toff(int control1, int control2, int target) : Gate(GateTypes.TOF, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0, 0, 0, 0, 0 },
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


