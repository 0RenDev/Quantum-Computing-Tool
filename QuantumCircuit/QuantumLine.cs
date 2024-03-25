using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuit
{
    internal class QuantumLine
    {
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

        public void printGates()
        {
            foreach (Gate gate in gates)
            {
                Console.Write("--" + gate.getGateType());
            }
        }
    }
}
