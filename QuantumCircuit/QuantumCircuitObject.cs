using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace QuantumCircuit
{
    public class QuantumCircuitObject
    {
        // List that keeps track of the classical lines in the circuit
        List<ClassicalLine> classicalLines = new List<ClassicalLine>();
        // List that keeps track of the quantum lines in the circuit
        List<QuantumLine> quantumLines = new List<QuantumLine>();
        String name;

        public QuantumCircuitObject(String name)
        {
            this.name = name;
        }

        public void AddQuantumLine(String qlName)
        {
            // Attempts to add a new quantum line to the circuit
            if (QuantumLineExists(qlName)) // If name already exists, print error
            {
                Console.WriteLine("Error: Quantum line with name " + qlName + " already exists.");
                return;
            }
            
            quantumLines.Add(new QuantumLine(qlName));
            Console.WriteLine("Quantum line added: " + qlName);
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
            // Check if the line is in the circuit, else print error
            if(!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in quantumLines) // iterate through lines until you find the one with the same unique name as the target
            {
                if (quantumLine.getName() == qlName)
                {
                    // Define Hadamard gate matrix
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    // Normalize
                    matrix = Operations.Multscaler(matrix, 1 / Math.Sqrt(2));
                    // Create new gate instance and add it to the line
                    Gate newGate = new Gate("H", matrix);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("H gate added at " + qlName);
                }
            }
        }

        public void pushBackY(String qlName)
        {
            // Check if the line is in the circuit, else print error
            if (!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in quantumLines)
            {
                if (quantumLine.getName() == qlName)
                {
                    // Define Y gate matrix
                    Matrix matrix = new Matrix(new Complex[,] { { 0, new Complex(1, -1) }, { new Complex(1, 1), 0 } });
                    // Create new gate instance and add it to the line
                    Gate newGate = new Gate("Y", matrix);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("Y gate added at " + qlName);
                }
            }
        }

        public void pushBackX(String qlName)
        {
            // Check if the line is in the circuit, else print error
            if (!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in quantumLines)
            {
                if (quantumLine.getName() == qlName)
                {
                    // Define the X gate matrix (quantum bit flip)
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, 1 } });
                    // Create new gate instance and add it to the quantum line
                    Gate newGate = new Gate("X", matrix);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("X gate added at " + qlName);
                }
            }
        }

        public void pushBackZ(String qlName)
        {
            // Check if the line is in the circuit, if not print error
            if (!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in quantumLines)
            {
                if (quantumLine.getName() == qlName)
                {
                    // Define the Z gate matrix (phase shift)
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, -1 } });
                    // Create new gate instance and add it to the line
                    Gate newGate = new Gate("Z", matrix);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("Z gate added at " + qlName);
                }
            }
        }

        public void pushBackCNOT(String qlSource, String qlTarget)
        {
            // Check if the source and target lines are in the circuit, if not, print error
            if (!QuantumLineExists(qlSource) || !QuantumLineExists(qlTarget))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
                return;
            }
            
            int sourceIndex = -1;
            foreach (var quantumLine in quantumLines)
            {
                if (quantumLine.getName() == qlSource)
                {
                    sourceIndex = quantumLine.getLength();
                    // TODO: Initialize CNOT gate
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    String[] targets = { qlTarget };
                    int[] targetIndex = new int[1];
                    // locate the target quantum line to get its gate count
                    for (int i = 0; i < quantumLines.Count; i++)
                    {
                        if (quantumLines[i].getName() == qlTarget)
                        {
                            targetIndex[0] = quantumLines[i].getLength();
                        }
                    }
                    // Create and add root part of CNOT gaste to source line
                    MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", matrix, targets, targetIndex);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("CNOT gate added at " + qlSource + " with target as " + qlTarget);
                }

            }

            // After setting the root part, add target
            foreach (var quantumLine in quantumLines)
            {
                if (quantumLine.getName() == qlTarget)
                {
                    // TODO: Initalize CNOT gate
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    String[] targets = { qlTarget };
                    // Create and add the target part of CNOT to target line
                    MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT TARG", matrix, qlSource, sourceIndex);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("CNOT gate Target added at " + qlTarget + " with source as " + qlSource);
                }
            }
        }

        public void pushBackTOF(String qlSource, String[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            // Check if the line is in the circuit, if not, print error
            if (!QuantumLineExists(qlSource))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
                return;
            }
            
            foreach (var quantumLine in quantumLines)
            {
                // If the quantum line's name is not the source, skip to the next line
                if (!(quantumLine.getName() == qlSource)) continue;

                sourceIndex = quantumLine.getLength();
                // For each target name, find corresponding quantum line and track its length
                for (int i = 0; i < qlTargets.Length; i++)
                {
                    for (int j = 0; j < quantumLines.Count; j++)
                    {
                        if (qlTargets[i] == quantumLines[j].getName())
                        {
                            indexes[i] = quantumLines[j].getLength();
                        }
                    }
                }
                // TODO: Initalize Toffoli gate
                Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                // Create a TOF gate as a root gate on the source line
                MultiLineGateRoot newGate = new MultiLineGateRoot("TOF", matrix, qlTargets, indexes);
                quantumLine.addGate(newGate);
                Console.WriteLine("TOF gate added at " + qlSource);

            }
            

            for(int i = 0; i < qlTargets.Length; i++)
            {
                // Check if the target lines are in the circuit, else print error
                if (!QuantumLineExists(qlTargets[i]))
                {
                    Console.WriteLine("Error, quantum line " + qlTargets[i] + " DNE!");
                    return;
                }
                
                foreach (var quantumLine in quantumLines)
                {
                    // If line's name is not in target, skip to the next line
                    if (!(quantumLine.getName() == qlTargets[i])) continue;
                    
                    // TODO: Initalize Toffoli gate
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    // Create a TOF gate as a target gate on the target line
                    MultiLineGateTarget newGate = new MultiLineGateTarget("TOF", matrix, qlSource, sourceIndex);
                    quantumLine.addGate(newGate);
                    Console.WriteLine("TOF gate added at " + qlSource);

                }
            }
        }

        public void pushBackCNOT(String qlSource, String[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            // Check if the line is in the circuit
            if (!QuantumLineExists(qlSource))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
                return;
            }
            
            foreach (var quantumLine in quantumLines)
            {
                // if line is not the source, skip to the next line
                if (!(quantumLine.getName() == qlSource)) continue;

                sourceIndex = quantumLine.getLength();
                for (int i = 0; i < qlTargets.Length; i++)
                {
                    for (int j = 0; j < quantumLines.Count; j++)
                    {
                        if (qlTargets[i] == quantumLines[j].getName())
                        {
                            indexes[i] = quantumLines[j].getLength();
                        }
                    }
                }
                // TODO: Initailize CNOT gate
                Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                // Create a CNOT gate as a root gate on the source line
                MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", matrix, qlTargets, indexes);
                quantumLine.addGate(newGate);
                Console.WriteLine("CNOT gate added at " + qlSource);
            }

            for (int i = 0; i < qlTargets.Length; i++)
            {
                // Check if the target lines are in the circuit, if not, print error
                if (!QuantumLineExists(qlTargets[i]))
                {
                    Console.WriteLine("Error, quantum line " + qlTargets[i] + " DNE!");
                    return;
                }
                
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlTargets[i])
                    {
                        // TODO: Initailize CNOT gate
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        // Create a CNOT gate as the target gate on the target quantum line
                        MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT", matrix, qlSource, sourceIndex);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("CNOT gate added at " + qlSource);
                    }
                }
            }
        }
        
        // prints information about each quantumLine in the circuit to the console, O(n)
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
