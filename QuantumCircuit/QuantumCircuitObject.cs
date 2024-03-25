using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace QuantumCircuit
{
    public class QuantumCircuitObject
    {
        List<ClassicalLine> classicalLines = new List<ClassicalLine>();
        List<QuantumLine> quantumLines = new List<QuantumLine>();
        String name;

        public QuantumCircuitObject(String name)
        {
            this.name = name;
        }

        public void AddQuantumLine(String qlName)
        {
            if (!QuantumLineExists(qlName))
            {
                quantumLines.Add(new QuantumLine(qlName));
                Console.WriteLine("Quantum line added: " + qlName);
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + qlName + " already exists.");
            }
        }

        private bool QuantumLineExists(string name)
        {
            foreach (QuantumLine line in quantumLines)
            {
                if (line.getName() == name)
                {
                    return true;
                }
            }
            return false;
        }
    

    public void pushBackH(String qlName)
        {
            if(QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        matrix = Operations.Multscaler(matrix, 1 / Math.Sqrt(2));
                        Gate newGate = new Gate("H", matrix);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("H gate added at " + qlName);
                    }
                }
            } else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }

        public void printCircuit()
        {
            Console.WriteLine("Quantum circuit: " + name);
            foreach (var quantumLine in quantumLines)
            {
                Console.Write(quantumLine.getName());
                quantumLine.printGates();
                Console.WriteLine();
            }
        }

        public void printOutput()
        {

        }
    }
}
