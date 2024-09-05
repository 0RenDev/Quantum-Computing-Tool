using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit
{
    internal class QuantumLine
    {
        // Stores list of gates applied to this line
        List<Gate> gates = new List<Gate>();
        private String name;

        public QuantumLine(string name)
        {
            this.name = name;
        }

        public String GetName()
        {
            return name;
        }

        public void AddGate(Gate newGate)
        {
            gates.Add(newGate);
        }

        public string[] GetGates()
        {
            string[] gateTypes = new string[gates.Count];
            int i = 0;
            foreach (Gate gate in gates)
            {
                gateTypes[i] = gate.GetGateType();
            }
            return gateTypes;
        }

        // prints all gates on this line to the console, O(n)
        public void PrintGates()
        {
            foreach (Gate gate in gates)
            {
                Console.Write("--" + gate.getGateType());
            }
        }
        public int GetLength()
        {
            return gates.Count;
        }
    }
}
