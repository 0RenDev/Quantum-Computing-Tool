using LinearAlgebra;
using System;
using System.Numerics;
using System.Text;

namespace QuantumCircuits
{
    /// <summary>
    /// This class is used to build a quantum circuit by adding gates to the circuit lines.
    /// </summary>
    public class QuantumCircuitBuilder
    {
        // Declare circuit lines        
        /// <summary>
        /// The quantum lines represent the series of quantum gates that are applied to a qubit.
        /// </summary>
        public List<Gate>[] quantumLines;

        /// <summary>
        /// The classical lines represent the series of classical gates that are applied to a classical bit.
        /// </summary>
        public List<Gate>[] classicalLines;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantumCircuitBuilder"/> class.
        /// </summary>
        /// <param name="numQuantumLines">The number quantum lines.</param>
        /// <param name="numClassicalLines">The number classical lines.</param>
        public QuantumCircuitBuilder(int numQuantumLines, int numClassicalLines)
        {
            // should check of input is > 0

            quantumLines = new List<Gate>[numQuantumLines];
            classicalLines = new List<Gate>[numClassicalLines];

            // Initialize all lists
            for (int i = 0; i < quantumLines.Length; i++)
            {
                quantumLines[i] = new List<Gate>(); // Initialize as empty
            }
            for (int i = 0; i < classicalLines.Length; i++)
            {
                classicalLines[i] = new List<Gate>(); // Initialize as empty
            }


        }

        /// <summary>
        /// Utility function to check for target being out of bounds.
        /// <summary>
        /// <param name="target">The target qubit.</param>
        /// <param name="type">Optional parameter to specify the type of the qubit (control or target).</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        private void ValidateTarget(int target, string type = "Target")
        {
            if (target < 0 || target >= quantumLines.Length)
            {
                throw new ArgumentException($"{type} outside of circuit bounds");
            }
        }

        /// <summary>
        /// Utility function to check for qubit equality (each should be distinct).
        /// <summary>
        /// <param name="qubits">Array of input qubits to check equality for</param>
        /// <exception cref="System.ArgumentException"> qubits not distinct</exception>
        private void ValidateQubits(int[] qubits)
        {
            var seenQubits = new HashSet<int>();

            foreach (var qubit in qubits)
            {
                // add returns false if it is already in the hashset
                if (!seenQubits.Add(qubit))
                {
                    throw new ArgumentException("Input qubits must be distinct, duplicate found.");
                }
            }
        }


        /// <summary>
        /// Adds the gate X.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateX(int target)
        {
            ValidateTarget(target);

            Gate x = new X(target);

            quantumLines[target].Add(x);
        }

        /// <summary>
        /// Adds the gate Y.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateY(int target)
        {
            ValidateTarget(target);

            Gate y = new Y(target);

            quantumLines[target].Add(y);
        }

        /// <summary>
        /// Adds the gate Z.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateZ(int target)
        {
            ValidateTarget(target);

            Gate z = new Z(target);

            quantumLines[target].Add(z);
        }

        /// <summary>
        /// Adds the gate T.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateT(int target)
        {
            ValidateTarget(target);

            Gate t = new T(target);

            quantumLines[target].Add(t);
        }

        /// <summary>
        /// Adds the gate H.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateH(int target)
        {
            ValidateTarget(target);

            Gate h = new H(target);

            quantumLines[target].Add(h);
        }

        /// <summary>
        /// Adds the gate CX (Controlled Not).
        /// </summary>
        /// <param name="control">The control qubit.</param>
        /// <param name="target">The target qubit.</param>
        /// <exception cref="System.ArgumentException">
        /// target and control cannot be the same value
        /// or
        /// target or control outside of circuit bounds
        /// </exception>
        public void addGateCX(int control, int target)
        {
            ValidateQubits([control, target]);

            ValidateTarget(target);
            ValidateTarget(control, "Control");

            while (quantumLines[control].Count != quantumLines[target].Count)
            {
                if (quantumLines[control].Count < quantumLines[target].Count)
                {
                    Gate conNop = new NOP(control, GateTypes.NOP);
                    quantumLines[control].Add(conNop);
                }
                else
                {
                    Gate tarNop = new NOP(target, GateTypes.NOP);
                    quantumLines[target].Add(tarNop);
                }
            }

            Gate cx = new CX(control, target);
            Gate con = new NOP(control, GateTypes.CXC);

            quantumLines[target].Add(cx);
            quantumLines[control].Add(con);
        }

        /// <summary>
        /// Adds the gate SWP.
        /// </summary>
        /// <param name="target1">The first target qubit.</param>
        /// <param name="target2">The second target qubit.</param>
        /// <exception cref="System.ArgumentException">
        /// target and control cannot be the same value
        /// or
        /// target or control outside of circuit bounds
        /// </exception>
        public void addGateSWP(int target1, int target2)
        {
            ValidateQubits([target1, target2]);

            ValidateTarget(target1, "Target1");
            ValidateTarget(target2, "Target2");

            while (quantumLines[target1].Count != quantumLines[target2].Count)
            {
                if (quantumLines[target1].Count < quantumLines[target2].Count)
                {
                    Gate tar1Nop = new NOP(target1, GateTypes.NOP);
                    quantumLines[target1].Add(tar1Nop);
                }
                else
                {
                    Gate tar2Nop = new NOP(target2, GateTypes.NOP);
                    quantumLines[target2].Add(tar2Nop);
                }
            }

            Gate swp = new SWAP(target1, target2);
            Gate tar = new NOP(target2, GateTypes.SWT);

            quantumLines[target1].Add(swp);
            quantumLines[target2].Add(tar);
        }

        /// <summary>
        /// Adds the gate TOF.
        /// </summary>
        /// <param name="control1">The first control qubit.</param>
        /// <param name="control2">The second control qubit.</param>
        /// <param name="target">The target qubit.</param>
        /// <exception cref="System.ArgumentException">
        /// target or controls can be the same value
        /// or
        /// target or control outside of circuit bounds
        /// </exception>
        public void addGateTOF(int control1, int control2, int target)
        {
            ValidateQubits([control1, control2, target]);
    
            ValidateTarget(control1, "Control1");
            ValidateTarget(control2, "Control2");
            ValidateTarget(target);

            // add in spacer nops to ensure correct order of execution
            while (quantumLines[control1].Count != quantumLines[target].Count || quantumLines[control2].Count != quantumLines[target].Count)
            {
                if (quantumLines[control1].Count < quantumLines[target].Count)
                {
                    Gate conNop = new NOP(control1, GateTypes.NOP);
                    quantumLines[control1].Add(conNop);
                }
                else if (quantumLines[control2].Count < quantumLines[target].Count)
                {
                    Gate conNop = new NOP(control2, GateTypes.NOP);
                    quantumLines[control2].Add(conNop);
                }
                else
                {
                    Gate tarNop = new NOP(target, GateTypes.NOP);
                    quantumLines[target].Add(tarNop);
                }
            }

            Gate toft = new Toff(control1, control2, target);
            Gate con1 = new NOP(control1, GateTypes.TOC);
            Gate con2 = new NOP(control2, GateTypes.TOC);

            quantumLines[control1].Add(con1);
            quantumLines[control2].Add(con2);
            quantumLines[target].Add(toft);

        }

        /// <summary>
        /// Adds the gate RX.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <param name="theta">Angle of rotation in radians</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateRX(int target, double theta)
        {
            ValidateTarget(target);

            Gate rx = new RX(target, theta);

            quantumLines[target].Add(rx);
        }

        /// <summary>
        /// Adds the gate RY.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <param name="theta">Angle of rotation in radians</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateRY(int target, double theta)
        {
            ValidateTarget(target);

            Gate ry = new RY(target, theta);

            quantumLines[target].Add(ry);
        }

        /// <summary>
        /// Adds the gate RZ.
        /// </summary>
        /// <param name="target">The target qubit</param>
        /// <param name="theta">Angle of rotation in radians</param>
        /// <exception cref="System.ArgumentException">target outside of circuit bounds</exception>
        public void addGateRZ(int target, double theta)
        {
            ValidateTarget(target);

            Gate rz = new RZ(target, theta);

            quantumLines[target].Add(rz);
        }


        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Quantum Lines:\n");

            for (int i = 0; i < quantumLines.Length; i++)
            {
                result.Append($"Row {i}: ");

                if (quantumLines[i].Count > 0)
                {
                    for (int j = 0; j < quantumLines[i].Count; j++)
                    {
                        result.Append(quantumLines[i][j].ToString());
                        if (j < quantumLines[i].Count - 1)
                        {
                            result.Append(" - "); // Add hyphen between elements
                        }
                    }
                }
                else
                {
                    result.Append("Empty");
                }
                result.AppendLine(); 
            }

            result.Append("Classical Lines:\n");

            for (int i = 0; i < classicalLines.Length; i++)
            {
                result.Append($"Row {i}: ");

                if (classicalLines[i].Count > 0)
                {
                    for (int j = 0; j < classicalLines[i].Count; j++)
                    {
                        result.Append(classicalLines[i][j]);
                        if (j < classicalLines[i].Count - 1)
                        {
                            result.Append(",\t"); // Add comma between elements
                        }
                    }
                }
                else
                {
                    result.Append("Empty");
                }
                result.AppendLine(); 
            }
            return result.ToString();
        }
    }
}
