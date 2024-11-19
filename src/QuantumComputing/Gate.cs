using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using QuantumComputing.LinearAlgebra;

namespace QuantumComputing.QuantumCircuits
{
    /// <summary>
    /// An <see cref="System.Enum"/> of the supported Gate Types
    /// </summary>
    public enum GateTypes
    {
        /// <summary>
        /// Not Gate
        /// </summary>
        XGT, // Not Gate
        /// <summary>
        /// Y Gate
        /// </summary>
        YGT,
        /// <summary>
        /// Z Gate
        /// </summary>
        ZGT, 
        /// <summary>
        /// T Gate
        /// </summary>
        TGT,
        /// <summary>
        /// Hadamard Gate
        /// </summary>
        HGT, // Hadamard Gate
        /// <summary>
        /// Controlled Not Gate Target
        /// </summary>
        CXT, // Controlled Not Gate target
        /// <summary>
        /// Controlled Not Gate Control
        /// </summary>
        CXC, // Control of CNOT gate
        /// <summary>
        /// Swap Gate target
        /// </summary>
        SWP, // swap gate target one
        /// <summary>
        /// Swap Gate target
        /// </summary>
        SWT, // second target of swap
        /// <summary>
        /// Toffoli Gate
        /// </summary>
        TOF, // Toffoli Gate
        /// <summary>
        /// Toffoli Gate controls
        /// </summary>
        TOC, // controls of toffoli gate
        /// <summary>
        /// Rx Gate
        /// </summary>
        RXT, // rotation gates
        /// <summary>
        /// Ry Gate
        /// </summary>
        RYT,
        /// <summary>
        /// Rz Gate
        /// </summary>
        RZT,
        /// <summary>
        /// No operation gate used to fill gaps
        /// </summary>
        NOP, // No Operation or Identity Gate
    }

    /// <summary>
    /// Constuctor for a Quantum Gate
    /// </summary>
    public class Gate
    {
        /// <summary>
        /// The type of gate. See <see cref="GateTypes"/>
        /// </summary>
        public GateTypes Type { get; private set; }

        /// <summary>
        /// The operator matrix of the gate
        /// </summary>
        public SparseMatrix Operation { get; private set; }

        /// <summary>
        /// The control qubits
        /// </summary>
        public int[] Controls { get; protected set; }

        /// <summary>
        /// The target qubits
        /// </summary>
        public int[] Targets { get; protected set; }

        /// <param name="type">The <see cref="GateTypes"/> of the gate</param>
        /// <param name="operation">A <see cref="SparseMatrix"/> of the gate's operator matrix</param>
        /// <param name="controls">The control qubits as an array of qubits (as indexes)</param>
        /// <param name="targets">The target qubits as an array of qubits (as indexes)</param>
        public Gate(GateTypes type, SparseMatrix operation, int[] controls, int[] targets)
        {
            Type = type;
            Operation = operation;
            Controls = controls;
            Targets = targets;
        }

        /// <summary>
        /// Returns the <see cref="System.String"/> representation of the Gate
        /// </summary>
        /// <returns></returns>
        override public String ToString()
        {
            switch (Type)
            {
                case GateTypes.XGT: return " X ";
                case GateTypes.YGT: return " Y ";
                case GateTypes.ZGT: return " Z ";
                case GateTypes.TGT: return " T ";
                case GateTypes.HGT: return " H ";
                case GateTypes.CXT: return "CX ";
                case GateTypes.CXC: return " ■ ";
                case GateTypes.SWP: return "SWP";
                case GateTypes.SWT: return " ■ ";
                case GateTypes.TOF: return "TOF";
                case GateTypes.TOC: return " ■ ";
                case GateTypes.RXT: return "RX ";
                case GateTypes.RYT: return "RY ";
                case GateTypes.RZT: return "RZ ";
                //GateTypes.RXG => $"RX({Theta})", 
                //GateTypes.RYG => $"RY({Theta})", can use if we add theta as an input to the gate later on
                //GateTypes.RZG => $"RZ({Theta})", maybe address later when overhauling visualization process, currently not supported since >3 characters
                case GateTypes.NOP: return "───";
                default: return Type.ToString();
            }
        }
    }

    /// <summary>
    /// Cosntructor for a NOP Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class NOP : Gate
    {
        /// <summary>
        /// Constructs a NOP Gate which acts as an empty space
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public NOP(int target, GateTypes type)
            : base(type, new SparseMatrix(new Complex[,] { { 1, 0 }, { 0, 1 } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for an X Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class X : Gate
    {
        /// <summary>
        /// Constructs an X Gate
        /// </summary>
        /// <param name="target"></param>
        public X(int target)
            : base(GateTypes.XGT, new SparseMatrix(new Complex[,] { { 0, 1 }, { 1, 0 } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Y Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class Y : Gate
    {
        /// <summary>
        /// Constructs a Y Gate
        /// </summary>
        /// <param name="target"></param>
        public Y(int target)
            : base(GateTypes.YGT, new SparseMatrix(new Complex[,] { { 0, -Complex.ImaginaryOne }, { Complex.ImaginaryOne, 0 } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Z Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class Z : Gate
    {
        /// <summary>
        /// Constructs an Z Gate
        /// </summary>
        /// <param name="target"></param>
        public Z(int target)
            : base(GateTypes.ZGT, new SparseMatrix(new Complex[,] { { 1, 0 }, { 0, -1 } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a T Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class T : Gate
    {
        /// <summary>
        /// Constructs a T Gate
        /// </summary>
        /// <param name="target"></param>
        public T(int target)
            : base(GateTypes.TGT, new SparseMatrix(new Complex[,] { { 1, 0 }, { 0, Complex.Exp(Complex.ImaginaryOne * Math.PI / 4) } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Hadamard Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class H : Gate
    {
        /// <summary>
        /// Constructs a Hadamard Gate
        /// </summary>
        /// <param name="target"></param>
        public H(int target)
            : base(GateTypes.HGT, new SparseMatrix(new Complex[,] { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) },
                                                                    { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Controlled Not (CX) Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class CX : Gate
    {
        /// <summary>
        /// Constructs an Controlled Not (CX) Gate
        /// </summary>
        /// <param name="control"></param>
        /// <param name="target"></param>
        public CX(int control, int target)
            : base(GateTypes.CXT, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                    { 0, 1, 0, 0 },
                                                                    { 0, 0, 0, 1 },
                                                                    { 0, 0, 1, 0 } }), new[] { control }, new[] { target })
        {
        }
    }

    /// <summary>
    /// Contsructor for a SWAP Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class SWAP : Gate
    {
        /// <summary>
        /// Constructs a SWAP Gate (SWP)
        /// </summary>
        /// <param name="target1"></param>
        /// <param name="target2"></param>
        public SWAP(int target1, int target2)
            : base(GateTypes.SWP, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                    { 0, 0, 1, 0 },
                                                                    { 0, 1, 0, 0 },
                                                                    { 0, 0, 0, 1 } }), new[] { target1 }, new[] { target2 })
        {
        }
    }

    /// <summary>
    /// Constructor for a Toffoli Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class Toff : Gate
    {
        /// <summary>
        /// Constructs a Toffoli Gate (Toff/TOF)
        /// </summary>
        /// <param name="control1"></param>
        /// <param name="control2"></param>
        /// <param name="target"></param>
        public Toff(int control1, int control2, int target)
            : base(GateTypes.TOF, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0, 0, 0, 0, 0 },
                                                                    { 0, 1, 0, 0, 0, 0, 0, 0 },
                                                                    { 0, 0, 1, 0, 0, 0, 0, 0 },
                                                                    { 0, 0, 0, 1, 0, 0, 0, 0 },
                                                                    { 0, 0, 0, 0, 1, 0, 0, 0 },
                                                                    { 0, 0, 0, 0, 0, 1, 0, 0 },
                                                                    { 0, 0, 0, 0, 0, 0, 0, 1 },
                                                                    { 0, 0, 0, 0, 0, 0, 1, 0 } }), new[] { control1, control2 }, new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Rotate X Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class RX : Gate
    {
        /// <summary>
        /// Constructs a Rotate X Gate (RX)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="theta"></param>
        public RX(int target, double theta)
            : base(GateTypes.RXT, new SparseMatrix(new Complex[,] { { Math.Cos(theta / 2), -Complex.ImaginaryOne * Math.Sin(theta / 2) },
                                                                    { -Complex.ImaginaryOne * Math.Sin(theta / 2), Math.Cos(theta / 2) } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Rotate Y Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class RY : Gate
    {
        /// <summary>
        /// Constructs a Rotate Y Gate (RY)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="theta"></param>
        public RY(int target, double theta)
            : base(GateTypes.RYT, new SparseMatrix(new Complex[,] { { Math.Cos(theta / 2), -Math.Sin(theta / 2) },
                                                                    { Math.Sin(theta / 2), Math.Cos(theta / 2) } }), new int[0], new[] { target })
        {
        }
    }

    /// <summary>
    /// Constructor for a Rotate Z Gate
    /// </summary>
    /// <seealso cref="QuantumCircuits.Gate" />
    public class RZ : Gate
    {
        /// <summary>
        /// Constructs a Rotate Z Gate (RZ)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="theta"></param>
        public RZ(int target, double theta)
            : base(GateTypes.RZT, new SparseMatrix(new Complex[,] { { Complex.Exp(-Complex.ImaginaryOne * theta / 2), 0 },
                                                                    { 0, Complex.Exp(Complex.ImaginaryOne * theta / 2) } }), new int[0], new[] { target })
        {
        }
    }

}


