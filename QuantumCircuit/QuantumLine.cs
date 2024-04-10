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

        public String getName()
        {
            return name;
        }

        public void addGate(Gate newGate)
        {
            gates.Add(newGate);
        }

        // prints all gates on this line to the console, O(n)
        public void printGates()
        {
            foreach (Gate gate in gates)
            {
                Console.Write("--" + gate.getGateType());
            }
        }
        public int getLength()
        {
            return gates.Count;
        }
    }
}
