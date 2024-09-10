using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace QuantumCircuit
{
    public class QuantumCircuitObject
    {
        // List that keeps track of the classical lines in the circuit
        readonly List<ClassicalLine> ClassicalLines = [];
        // List that keeps track of the quantum lines in the circuit
        List<QuantumLine> QuantumLines = [];
        readonly String name;

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
            
            QuantumLines.Add(new QuantumLine(qlName));
            Console.WriteLine("Quantum line added: " + qlName);
        }

        private bool QuantumLineExists(string name)
        {
            foreach (QuantumLine line in QuantumLines) 
            {
                if (line.GetName() == name)
                {
                    return true;
                }
            }
            return false;
        }
    

    public void PushBackIdentity(String qlName)
        {
        // Check if the line is in the circuit, else print error
        if (!QuantumLineExists(qlName))
            {
            Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
        }

        foreach (var quantumLine in QuantumLines)
            {
            if (quantumLine.GetName() == qlName)
                {
                // Define the identity matrix
                Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, 1 } });
                // Create new gate instance and add it to the line
                Gate newGate = new Gate("I", matrix);
                quantumLine.AddGate(newGate);
                Console.WriteLine("Identity gate added at " + qlName);
            }
        }
    }

    public void PushBackH(String qlName)
        {
            // Check if the line is in the circuit, else print error
            if(!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in QuantumLines) // iterate through lines until you find the one with the same unique name as the target
            {
                if (quantumLine.GetName() == qlName)
                {
                    // Define Hadamard gate matrix
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    // Normalize
                    matrix = Operations.Multscaler(matrix, 1 / Math.Sqrt(2));
                    // Create new gate instance and add it to the line
                    Gate newGate = new Gate("H", matrix);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("H gate added at " + qlName);
                }
            }
        }

        public void PushBackY(String qlName)
        {
            // Check if the line is in the circuit, else print error
            if (!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetName() == qlName)
                {
                    // Define Y gate matrix
                    Matrix matrix = new Matrix(new Complex[,] { { 0, new Complex(1, -1) }, { new Complex(1, 1), 0 } });
                    // Create new gate instance and add it to the line
                    Gate newGate = new Gate("Y", matrix);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("Y gate added at " + qlName);
                }
            }
        }

        public void PushBackX(String qlName)
        {
            // Check if the line is in the circuit, else print error
            if (!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetName() == qlName)
                {
                    // Define the X gate matrix (quantum bit flip)
                    Matrix matrix = new Matrix(new Complex[,] { { 0, 1 }, { 1, 0 } });
                    // Create new gate instance and add it to the quantum line
                    Gate newGate = new Gate("X", matrix);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("X gate added at " + qlName);
                }
            }
        }

        public void PushBackZ(String qlName)
        {
            // Check if the line is in the circuit, if not print error
            if (!QuantumLineExists(qlName))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetName() == qlName)
                {
                    // Define the Z gate matrix (phase shift)
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, -1 } });
                    // Create new gate instance and add it to the line
                    Gate newGate = new Gate("Z", matrix);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("Z gate added at " + qlName);
                }
            }
        }

        public void PushBackCNOT(String qlSource, String qlTarget)
        {
            // Check if the source and target lines are in the circuit, if not, print error
            if (!QuantumLineExists(qlSource) || !QuantumLineExists(qlTarget))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
                return;
            }
            
            int sourceIndex = -1;
            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetName() == qlSource)
                {
                    sourceIndex = quantumLine.GetLength();
                    // TODO: Initialize CNOT gate
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    String[] targets = { qlTarget };
                    int[] targetIndex = new int[1];
                    // locate the target quantum line to get its gate count
                    for (int i = 0; i < QuantumLines.Count; i++)
                    {
                        if (QuantumLines[i].GetName() == qlTarget)
                        {
                            targetIndex[0] = QuantumLines[i].GetLength();
                        }
                    }
                    // Create and add root part of CNOT gaste to source line
                    MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", matrix, targets, targetIndex);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("CNOT gate added at " + qlSource + " with target as " + qlTarget);
                }

            }

            // After setting the root part, add target
            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetName() == qlTarget)
                {
                    // TODO: Initalize CNOT gate
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    String[] targets = { qlTarget };
                    // Create and add the target part of CNOT to target line
                    MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT TARG", matrix, [qlSource], [sourceIndex]);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("CNOT gate Target added at " + qlTarget + " with source as " + qlSource);
                }
            }
        }

        public void PushBackTOF(String[] qlSources, String qlTarget)
        {
            int targetIndex = -1;
            int[] indexes = new int[qlSources.Length];
            // Check if the line is in the circuit, if not, print error
            if (!QuantumLineExists(qlTarget))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
                return;
            }

            foreach (var quantumLine in QuantumLines)
            {
                // If the quantum line's name is not the source, skip to the next line
                if (!(quantumLine.GetName() == qlTarget)) continue;

                targetIndex = quantumLine.GetLength();
                // For each target name, find corresponding quantum line and track its length
                for (int i = 0; i < qlSources.Length; i++)
                {
                    for (int j = 0; j < QuantumLines.Count; j++)
                    {
                        if (qlSources[i] == QuantumLines[j].GetName())
                        {
                            indexes[i] = QuantumLines[j].GetLength();
                        }
                    }
                }
                // TODO: Initalize Toffoli gate
                Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                // Create a TOF gate as a root gate on the source line
                MultiLineGateTarget newGate = new MultiLineGateTarget("TOF", matrix, qlSources, indexes);
                quantumLine.AddGate(newGate);
                Console.WriteLine("TOF gate added at " + qlTarget);

            }


            for (int i = 0; i < qlSources.Length; i++)
            {
                // Check if the target lines are in the circuit, else print error
                if (!QuantumLineExists(qlSources[i]))
                {
                    Console.WriteLine("Error, quantum line " + qlSources[i] + " DNE!");
                    return;
                }

                foreach (var quantumLine in QuantumLines)
                {
                    // If line's name is not in target, skip to the next line
                    if (!(quantumLine.GetName() == qlSources[i])) continue;

                    // TODO: Initalize Toffoli gate
                    Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                    // Create a TOF gate as a target gate on the target line
                    MultiLineGateRoot newGate = new MultiLineGateRoot("TOF", matrix, [qlTarget], [targetIndex]);
                    quantumLine.AddGate(newGate);
                    Console.WriteLine("TOF gate added at " + qlTarget);

                }
            }
        }

        public void PushBackCNOT(String qlSource, String[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            // Check if the line is in the circuit
            if (!QuantumLineExists(qlSource))
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
                return;
            }
            
            foreach (var quantumLine in QuantumLines)
            {
                // if line is not the source, skip to the next line
                if (!(quantumLine.GetName() == qlSource)) continue;

                sourceIndex = quantumLine.GetLength();
                for (int i = 0; i < qlTargets.Length; i++)
                {
                    for (int j = 0; j < QuantumLines.Count; j++)
                    {
                        if (qlTargets[i] == QuantumLines[j].GetName())
                        {
                            indexes[i] = QuantumLines[j].GetLength();
                        }
                    }
                }
                // TODO: Initailize CNOT gate
                Matrix matrix = new(new Complex[,] { { 1, 1 }, { 1, -1 } });
                // Create a CNOT gate as a root gate on the source line
                MultiLineGateRoot newGate = new("CNOT", matrix, qlTargets, indexes);
                quantumLine.AddGate(newGate);
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
                
                foreach (var quantumLine in QuantumLines)
                {
                    if (quantumLine.GetName() == qlTargets[i])
                    {
                        // TODO: Initailize CNOT gate
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        // Create a CNOT gate as the target gate on the target quantum line
                        MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT", matrix, [qlSource], [sourceIndex]);
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("CNOT gate added at " + qlSource);
                    }
                }
            }
        }
        
        // prints information about each quantumLine in the circuit to the console, O(n)
        public void PrintCircuit()
        {
            Console.WriteLine("Quantum circuit: " + name);
            foreach (var quantumLine in QuantumLines)
            {
                Console.Write(quantumLine.GetName());
                quantumLine.PrintGates();
                Console.WriteLine();
            }
        }

        public void PrintOutput()
        {

        }

        public int QuantumLinesCount()
        {
            return QuantumLines.Count;
        }

        public string[] GetQuantumLineGates(string quantumLineName)
        {
            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetName() == quantumLineName)
                {
                    return quantumLine.GetGates();
                }
            }

            return [];
        }

        public void Execute()
        {
            // Convert the quantum circuit to a quantum register
            // Convert the quantum lines into a matrix of gates
            // For each column of the matrix, apply nonentangling gates to the quantum register, then apply entangling gates

            QuantumRegister quantumRegister = new(QuantumLinesCount());

            int maxGates = 0;
            foreach (var quantumLine in QuantumLines)
            {
                if (quantumLine.GetLength() > maxGates)
                {
                    maxGates = quantumLine.GetLength();
                }
            }
        }
    }
}
