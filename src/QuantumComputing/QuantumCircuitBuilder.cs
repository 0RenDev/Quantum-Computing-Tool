using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;

namespace QuantumComputing.QuantumCircuits
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
        /// </summary>
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
        /// </summary>
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
        public void AddGateX(int target)
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
        public void AddGateY(int target)
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
        public void AddGateZ(int target)
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
        public void AddGateT(int target)
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
        public void AddGateH(int target)
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
        public void AddGateCX(int control, int target)
        {
            ValidateQubits(new int[] { control, target });

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
        public void AddGateSWP(int target1, int target2)
        {
            ValidateQubits(new int[] { target1, target2 });

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

            Gate swp = new SWAP(target2, target1);
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
        public void AddGateTOF(int control1, int control2, int target)
        {
            ValidateQubits(new int[] { control1, control2, target });
    
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
        public void AddGateRX(int target, double theta)
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
        public void AddGateRY(int target, double theta)
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
        public void AddGateRZ(int target, double theta)
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
            // Define the number of lines needed to create a box around each gate
            StringBuilder[] linesTop = new StringBuilder[quantumLines.Length];
            StringBuilder[] linesMiddle = new StringBuilder[quantumLines.Length];
            StringBuilder[] linesBottom = new StringBuilder[quantumLines.Length];

            // Initialize each StringBuilder with the qubit label and starting line
            for (int i = 0; i < quantumLines.Length; i++)
            {
                linesTop[i] = new StringBuilder($"     ");
                linesMiddle[i] = new StringBuilder($"q{i}: ─");
                linesBottom[i] = new StringBuilder("     ");
            }

            // Find the maximum number of gates on any line to ensure uniform width
            int maxGates = quantumLines.Max(line => line.Count);

            // Build the ASCII representation of each gate on each line
            for (int pos = 0; pos < maxGates; pos++)
            {
                for (int q = 0; q < quantumLines.Length; q++)
                {
                    if (pos < quantumLines[q].Count)
                    {
                        string[] gateBox = GetBoxedAsciiSymbol(quantumLines[q][pos]);
                        linesTop[q].Append(gateBox[0]);
                        linesMiddle[q].Append(gateBox[1]);
                        linesBottom[q].Append(gateBox[2]);

                        // Adds spacing between gates
                        linesTop[q].Append(" ");
                        linesMiddle[q].Append("─");
                        linesBottom[q].Append(" ");
                    }
                    else
                    {
                        // Adds spacing if there's no gate in this position
                        linesTop[q].Append("     ");
                        linesMiddle[q].Append("─────");
                        linesBottom[q].Append("     ");
                    }
                }
            }

            // Combine all lines together into the final output
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < quantumLines.Length; i++)
            {
                result.AppendLine(linesTop[i].ToString());
                result.AppendLine(linesMiddle[i].ToString());
                result.AppendLine(linesBottom[i].ToString());
            }

            return result.ToString();
        }

        /// <summary>
        /// Helper method for circuit ToString. Makes an array of strings to represent a gate.
        /// </summary>
        /// <param name="gate">The <see cref="Gate"/> to generate ASCII symbols for.</param>
        /// <returns>
        /// An array of <see cref="System.String"/> array representing the ASCII symbol of the gate.
        /// </returns>
        public string[] GetBoxedAsciiSymbol(Gate gate)
        {
            string[] gateString = new[] {" "," ",};


            if (gate.Type == GateTypes.CXT ||
                gate.Type == GateTypes.TOF ||
                gate.Type == GateTypes.SWP)
            {
                string topLine = " ";
                string middleLine = $"┤ {gate.ToString()} ├";
                string bottomLine = " ";

                if (HasControlAboveOrBelow(gate.Controls, gate.Targets[0], true))
                {
                    topLine = "┌──┴──┐";
                }
                else
                {
                    topLine = "┌─────┐";
                }
                if (HasControlAboveOrBelow(gate.Controls, gate.Targets[0], false))
                {
                    bottomLine = "└──┬──┘";
                }
                else
                {
                    bottomLine = "└─────┘";
                }
                gateString = new[] { topLine, middleLine, bottomLine };
            }
            else if (gate.Type == GateTypes.CXC ||
                gate.Type == GateTypes.TOC ||
                gate.Type == GateTypes.SWT ||
                gate.Type == GateTypes.NOP)
            {
                gateString = new[] {
                    "       ",               // Top line
                   $"──{gate.ToString()}──", // Middle line with control symbol
                    "       " };             // Bottom line
            }
            else
            {
                gateString = new[] {
                    "┌─────┐",                 // Top line
                   $"┤ {gate.ToString()} ├", // Middle line with gate symbol
                    "└─────┘" };                 // Bottom line
            }

            return gateString;
        }

        /// <summary>
        /// Checks if there is a control qubit above or below a target qubit.
        /// </summary>
        /// <param name="controls">Array of control qubit indices.</param>
        /// <param name="target">The target qubit index.</param>
        /// <param name="above">
        /// <c>true</c> to check for controls above; <c>false</c> to check below.
        /// </param>
        /// <returns>
        /// <c>true</c> if a control qubit is found in the specified direction; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasControlAboveOrBelow(int[] controls, int target, bool above)
        {
            if (above)
            {
                foreach (int value in controls)
                {
                    if (value < target)
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (int value in controls)
                {
                    if (value > target)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

