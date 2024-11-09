using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuits
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
    /// <param name="type">The <see cref="GateTypes"/> of the gate</param>
    /// <param name="operation">A <see cref="SparseMatrix"/> of the gate's operator matrix</param>
    /// <param name="controls">The control qubits as an array of qubits (as indexes)</param>
    /// <param name="targets">The target qubits as an array of qubits (as indexes)</param>
    public class Gate(GateTypes type, SparseMatrix operation, int[] controls, int[] targets)
    {
        /// <summary>
        /// The type of gate. See <see cref="GateTypes"/>
        /// </summary>
        public GateTypes Type { get; private set; } = type;

        /// <summary>
        /// The operator matrix of the gate
        /// </summary>
        public SparseMatrix Operation { get; private set; } = operation;

        /// <summary>
        /// The control qubits
        /// </summary>
        public int[] Controls { get; protected set; } = controls;

        /// <summary>
        /// The target qubits
        /// </summary>
        public int[] Targets { get; protected set; } = targets;

        /// <summary>
        /// Returns the <see cref="System.String"/> representation of the Gate
        /// </summary>
        /// <returns></returns>
        override public String ToString()
        {
            return Type switch
            {
                 GateTypes.XGT => " X ",
                 GateTypes.YGT => " Y ",
                 GateTypes.ZGT => " Z ",
                 GateTypes.TGT => " T ",
                 GateTypes.HGT => " H ",
                 GateTypes.CXT => "CX ",
                 GateTypes.CXC => " ■ ",
                 GateTypes.SWP => "SWP",
                 GateTypes.SWT => " ■ ",
                 GateTypes.TOF => "TOF",
                 GateTypes.TOC => " ■ ",
                 GateTypes.RXT => "RX ",
                 GateTypes.RYT => "RY ",
                 GateTypes.RZT => "RZ ",
                //GateTypes.RXG => $"RX({Theta})", 
                //GateTypes.RYG => $"RY({Theta})", can use if we add theta as an input to the gate later on
                //GateTypes.RZG => $"RZ({Theta})", maybe address later when overhauling visualization process, currently not supported since >3 characters
                GateTypes.NOP => "───",
                _ => Type.ToString()
            };
        }
    }

    /// <summary>
    /// Constructs a NOP Gate which acts as an empty space
    /// </summary>
    /// <param name="target"></param>
    /// <param name="type"></param>
    public class NOP(int target, GateTypes type) : Gate(type, new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                                                { 0, 1 } }), [], [target])
    {

    }

    /// <summary>
    /// Constructs an X Gate
    /// </summary>
    /// <param name="target"></param>
    public class X(int target) : Gate(GateTypes.XGT, new SparseMatrix(new Complex[,] { { 0, 1 },
                                                                                       { 1, 0 } }), [], [target])
    {
           
    }

    /// <summary>
    /// Constructs a Y Gate
    /// </summary>
    /// <param name="target"></param>
    public class Y(int target) : Gate(GateTypes.YGT, new SparseMatrix(new Complex[,] { { 0, -Complex.ImaginaryOne },
                                                                                       { Complex.ImaginaryOne, 0 } }), [], [target])
    {

    }

    /// <summary>
    /// Constructs an Z Gate
    /// </summary>
    /// <param name="target"></param>
    public class Z(int target) : Gate(GateTypes.ZGT, new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                                       { 0, -1 } }), [], [target])
    {

    }

    /// <summary>
    /// Constructs a T Gate
    /// </summary>
    /// <param name="target"></param>
    public class T(int target) : Gate(GateTypes.TGT, new SparseMatrix(new Complex[,] { { 1, 0 },
                                                                                       { 0, Complex.Exp(Complex.ImaginaryOne * Math.PI / 4) } }), [], [target])
    {

    }
    
    /// <summary>
    /// Constructs a Hadamard Gate
    /// </summary>
    /// <param name="target"></param>
    public class H(int target) : Gate(GateTypes.HGT, new SparseMatrix(new Complex[,] { { 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) },
                                                                                       { 1 / Complex.Sqrt(2), -1 / Complex.Sqrt(2) } }), [], [target])
    {

    }

    /// <summary>
    /// Constructs an Controlled Not (CX) Gate
    /// </summary>
    /// <param name="control"></param>
    /// <param name="target"></param>
    public class CX(int control, int target) : Gate(GateTypes.CXT, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                                                     { 0, 1, 0, 0 },
                                                                                                     { 0, 0, 0, 1 },
                                                                                                     { 0, 0, 1, 0 } }), [control], [target])
    {

    }

    /// <summary>
    /// Constructs a SWAP Gate (SWP)
    /// </summary>
    /// <param name="target1"></param>
    /// <param name="target2"></param>
    public class SWAP(int target1, int target2) : Gate(GateTypes.SWP, new SparseMatrix(new Complex[,] { { 1, 0, 0, 0 },
                                                                                                        { 0, 0, 1, 0 },
                                                                                                        { 0, 1, 0, 0 },
                                                                                                        { 0, 0, 0, 1 } }), [target1], [target2])
    {

    }

    /// <summary>
    /// Constructs a Toffoli Gate (Toff/TOF)
    /// </summary>
    /// <param name="control1"></param>
    /// <param name="control2"></param>
    /// <param name="target"></param>
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

    /// <summary>
    /// Constructs a Rotate X Gate (RX)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="theta"></param>
    public class RX(int target, double theta) : Gate(GateTypes.RXT, new SparseMatrix(new Complex[,] { { new Complex(Math.Cos(theta / 2), 0), new Complex(0, -Math.Sin(theta / 2)) },
                                                                                                      { new Complex(0, -Math.Sin(theta / 2)), new Complex(Math.Cos(theta / 2), 0) }}), [], [target])
    {

    }

    /// <summary>
    /// Constructs a Rotate Y Gate (RY)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="theta"></param>
    public class RY(int target, double theta) : Gate(GateTypes.RYT, new SparseMatrix(new Complex[,] { { new Complex(Math.Cos(theta / 2), 0), new Complex(-Math.Sin(theta / 2), 0) },
                                                                                                      { new Complex(Math.Sin(theta / 2), 0), new Complex(Math.Cos(theta / 2), 0) }}), [], [target])
    {

    }

    /// <summary>
    /// Constructs a Rotate Z Gate (RZ)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="theta"></param>
    public class RZ(int target, double theta) : Gate(GateTypes.RZT, new SparseMatrix(new Complex[,] { { Complex.Exp(-Complex.ImaginaryOne * theta / 2), Complex.Zero },
                                                                                                      { Complex.Zero, Complex.Exp(Complex.ImaginaryOne * theta / 2) }}), [], [target])
    {

    }

}


