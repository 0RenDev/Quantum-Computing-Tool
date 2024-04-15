using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//-------------------------------------------------------------------------------------------------------------------------------------------------------------
// This is the QuantumLine class representing our implementation of a Quantum Line
// it has the methods addGate and printGate along with standard getters (check each method for more detailed descriptions)
//-------------------------------------------------------------------------------------------------------------------------------------------------------------

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
        public int getLength()
        {
            return gates.Count;
        }
    }
}
