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

        public void pushBackY(String qlName)
        {
            if (QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 0, new Complex(1, -1) }, { new Complex(1, 1), 0 } });
                        Gate newGate = new Gate("Y", matrix);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("Y gate added at " + qlName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        public void pushBackX(String qlName)
        {
            if (QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, 1 } });
                        Gate newGate = new Gate("X", matrix);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("X gate added at " + qlName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        public void pushBackZ(String qlName)
        {
            if (QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, -1 } });
                        Gate newGate = new Gate("Z", matrix);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("Z gate added at " + qlName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        public void pushBackCNOT(String qlSource, String qlTarget)
        {
            if (QuantumLineExists(qlSource) && (QuantumLineExists(qlTarget)))
            {
                int sourceIndex = -1;
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlSource)
                    {
                        sourceIndex = quantumLine.getLength();
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        String[] targets = { qlTarget };
                        int[] targetIndex = new int[1];
                        for(int i = 0; i < quantumLines.Count; i++)
                        {
                            if (quantumLines[i].getName() == qlTarget)
                            {
                                targetIndex[0] = quantumLines[i].getLength();
                            }
                        }
                        MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", matrix, targets, targetIndex);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("CNOT gate added at " + qlSource + " with target as " + qlTarget);
                    }
                    
                }
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlTarget)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        String[] targets = { qlTarget };
                        MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT TARG", matrix, qlSource, sourceIndex);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("CNOT gate Target added at " + qlTarget + " with source as " + qlSource);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        public void pushBackTOF(String qlSource, String[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            if (QuantumLineExists(qlSource))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlSource)
                    {
                        sourceIndex = quantumLine.getLength();
                        for(int i = 0; i < qlTargets.Length; i++)
                        {
                            for(int j = 0; j < quantumLines.Count; j++)
                            {
                                if (qlTargets[i] == quantumLines[j].getName())
                                {
                                    indexes[i] = quantumLines[j].getLength();
                                }
                            }
                        }
                        // Need to figure out how this shindig works.
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        MultiLineGateRoot newGate = new MultiLineGateRoot("TOF", matrix, qlTargets, indexes);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("TOF gate added at " + qlSource);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            for(int i = 0; i < qlTargets.Length; i++)
            {
                if (QuantumLineExists(qlTargets[i]))
                {
                    foreach (var quantumLine in quantumLines)
                    {
                        if (quantumLine.getName() == qlTargets[i])
                        {
                            // Need to figure out how this shindig works.
                            Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                            MultiLineGateTarget newGate = new MultiLineGateTarget("TOF", matrix, qlSource, sourceIndex);
                            quantumLine.addGate(newGate);
                            Console.WriteLine("TOF gate added at " + qlSource);
                        }
                    }
                } else
                {
                    Console.WriteLine("Error, quantum line " + qlTargets[i] + " DNE!");
                }
            }
        }

        public void pushBackCNOT(String qlSource, String[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            if (QuantumLineExists(qlSource))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.getName() == qlSource)
                    {
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
                        // Need to figure out how this shindig works.
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", matrix, qlTargets, indexes);
                        quantumLine.addGate(newGate);
                        Console.WriteLine("CNOT gate added at " + qlSource);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }

            for (int i = 0; i < qlTargets.Length; i++)
            {
                if (QuantumLineExists(qlTargets[i]))
                {
                    foreach (var quantumLine in quantumLines)
                    {
                        if (quantumLine.getName() == qlTargets[i])
                        {
                            // Need to figure out how this shindig works.
                            Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                            MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT", matrix, qlSource, sourceIndex);
                            quantumLine.addGate(newGate);
                            Console.WriteLine("CNOT gate added at " + qlSource);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error, quantum line " + qlTargets[i] + " DNE!");
                }
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
