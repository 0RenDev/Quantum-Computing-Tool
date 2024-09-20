using LinearAlgebra;
using System;
using System.Numerics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuantumCircuits
{
    public class QuantumCircuitBuilder
    {
        // Declare circuit lines
        public List<Gate>[] quantumLines;
        public List<Gate>[] classicalLines;

        // Constructor to define the array's length
        public QuantumCircuitBuilder(int numQuantumLines, int numClassicalLines)
        {
            // should check of input is > 0

            quantumLines = new List<Gate>[numQuantumLines];
            classicalLines = new List<Gate>[numClassicalLines];

            // Initialize all lists
            for (int i = 0; i < quantumLines.Length; i++)
            {
                quantumLines[i] = new List<Gate>(); // Initialize as empty lists
            }
            for (int i = 0; i < classicalLines.Length; i++)
            {
                classicalLines[i] = new List<Gate>(); // Initialize as empty lists
            }


        }

        public void addGateX(int target)
        {
            if(target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate x = new Gate(GateTypes.XGT, target, new List<int> { }, quantumLines[target].Count);

            quantumLines[target].Add(x);
        }

        
        public void addGateH(int target)
        {
            if (target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate h = new Gate(GateTypes.HGT, target, new List<int> { }, quantumLines[target].Count);

            quantumLines[target].Add(h);
        }

        public void addGateCX(int target, int control)
        {
            if (target == control)
            {
                throw new ArgumentException("target and control cannot be the same value");
            }

            if ( target > quantumLines.Length || control > quantumLines.Length)
            {
                throw new ArgumentException("target or control outside of circuit bounds");
            }

            int diff = target - control;
            if(diff != 1 && diff != -1)
            {
                throw new ArgumentException("Line difference of target and control must be 1");
            }


            while(quantumLines[control].Count != quantumLines[target].Count)
            {
                if (quantumLines[control].Count < quantumLines[target].Count)
                {
                    Gate conNop = new Gate(GateTypes.NOP, control, new List<int> { }, quantumLines[control].Count);
                    quantumLines[control].Add(conNop);
                }
                else
                {
                    Gate tarNop = new Gate(GateTypes.NOP, target, new List<int> { }, quantumLines[target].Count);
                    quantumLines[target].Add(tarNop);
                }
            }

            Gate cxt = new Gate(GateTypes.CXT, target, new List<int> { control }, quantumLines[target].Count);
            Gate cxc = new Gate(GateTypes.CXC, target, new List<int> { }, quantumLines[control].Count);

            quantumLines[target].Add(cxt);
            quantumLines[control].Add(cxc);
        }



        public void addGateTOF(int target, int control1, int control2)
        {
            if (target == control1 ||  target == control2)
            {
                throw new ArgumentException("target or controls can be the same value");
            }
    
            if (target > quantumLines.Length || control1 > quantumLines.Length || control2 > quantumLines.Length)
            {
                throw new ArgumentException("target or control outside of circuit bounds");
            }



            // target and controls must be a direct sequence like: 0,1,2
            // need to modify so that the case of control target control doesn't count as valid
            int min = Math.Min(Math.Min(target, control1), control2);
            int max = Math.Max(Math.Max(target, control1), control2);
            int middle = target + control1 + control2 - min - max;

            if((max - middle != 1) && (middle - min != 1))
            {
                throw new ArgumentException("Line difference of target and controls must be 1");
            }

            while (quantumLines[control1].Count != quantumLines[target].Count || quantumLines[control2].Count != quantumLines[target].Count)
            {
                if (quantumLines[control1].Count < quantumLines[target].Count)
                {
                    Gate conNop = new Gate(GateTypes.NOP, control1, new List<int> { }, quantumLines[control1].Count);
                    quantumLines[control1].Add(conNop);
                }
                else if (quantumLines[control2].Count < quantumLines[target].Count)
                {
                    Gate conNop = new Gate(GateTypes.NOP, control2, new List<int> { }, quantumLines[control2].Count);
                    quantumLines[control2].Add(conNop);
                }
                else
                {
                    Gate tarNop = new Gate(GateTypes.NOP, target, new List<int> { }, quantumLines[target].Count);
                    quantumLines[target].Add(tarNop);
                }
            }

            Gate toft = new Gate(GateTypes.TOF, target, new List<int> { control1, control2 }, quantumLines[target].Count);
            Gate tofc1 = new Gate(GateTypes.TOC, target, new List<int> { }, quantumLines[control1].Count);
            Gate tofc2 = new Gate(GateTypes.TOC, target, new List<int> { }, quantumLines[control2].Count);

            quantumLines[target].Add(toft);
            quantumLines[control1].Add(tofc1);
            quantumLines[control2].Add(tofc2);

        }
        

        // add more gates here


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
                        result.Append(quantumLines[i][j]);
                        if (j < quantumLines[i].Count - 1)
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
