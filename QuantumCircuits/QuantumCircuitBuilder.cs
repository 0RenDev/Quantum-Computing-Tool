using LinearAlgebra;
using System;
using System.Numerics;
using System.Text;

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
                quantumLines[i] = new List<Gate>(); // Initialize as empty
            }
            for (int i = 0; i < classicalLines.Length; i++)
            {
                classicalLines[i] = new List<Gate>(); // Initialize as empty
            }


        }

        public void addGateX(int target)
        {
            if(target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate x = new X(target);

            quantumLines[target].Add(x);
        }

        public void addGateY(int target)
        {
            if (target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate y = new Y(target);

            quantumLines[target].Add(y);
        }

        public void addGateZ(int target)
        {
            if (target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate z = new Z(target);

            quantumLines[target].Add(z);
        }

        public void addGateT(int target)
        {
            if (target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate t = new T(target);

            quantumLines[target].Add(t);
        }


        public void addGateH(int target)
        {
            if (target > quantumLines.Length)
            {
                throw new ArgumentException("target outside of circuit bounds");
            }

            Gate h = new H(target);

            quantumLines[target].Add(h);
        }

        public void addGateCX(int control, int target)
        {
            if (target == control)
            {
                throw new ArgumentException("target and control cannot be the same value");
            }

            if ( target > quantumLines.Length || control > quantumLines.Length)
            {
                throw new ArgumentException("target or control outside of circuit bounds");
            }

            while(quantumLines[control].Count != quantumLines[target].Count)
            {
                if (quantumLines[control].Count < quantumLines[target].Count)
                {
                    Gate conNop = new NOP(control);
                    quantumLines[control].Add(conNop);
                }
                else
                {
                    Gate tarNop = new NOP(target);
                    quantumLines[target].Add(tarNop);
                }
            }

            Gate cx = new CX(control, target);
            Gate con = new NOP(control);

            quantumLines[target].Add(cx);
            quantumLines[control].Add(con);
        }

        // Add SWAP gate
        // should be similar to CX

        
        public void addGateTOF(int control1, int control2, int target)
        {
            if (target == control1 ||  target == control2)
            {
                throw new ArgumentException("target or controls can be the same value");
            }
    
            if (target > quantumLines.Length || control1 > quantumLines.Length || control2 > quantumLines.Length)
            {
                throw new ArgumentException("target or control outside of circuit bounds");
            }

            // add in spacer nops to ensure correct order of execution
            while (quantumLines[control1].Count != quantumLines[target].Count || quantumLines[control2].Count != quantumLines[target].Count)
            {
                if (quantumLines[control1].Count < quantumLines[target].Count)
                {
                    Gate conNop = new NOP(control1);
                    quantumLines[control1].Add(conNop);
                }
                else if (quantumLines[control2].Count < quantumLines[target].Count)
                {
                    Gate conNop = new NOP(control2);
                    quantumLines[control2].Add(conNop);
                }
                else
                {
                    Gate tarNop = new NOP(target);
                    quantumLines[target].Add(tarNop);
                }
            }

            Gate toft = new Toff(control1, control2, target);
            Gate con1 = new NOP(control1);
            Gate con2 = new NOP(control2);

            quantumLines[control1].Add(con1);
            quantumLines[control2].Add(con2);
            quantumLines[target].Add(toft);

        }
        


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
