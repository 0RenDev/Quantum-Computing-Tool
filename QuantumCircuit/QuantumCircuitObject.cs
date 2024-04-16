using LinearAlgebra;
using Quantum;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace QuantumCircuit
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class QuantumCircuitObject
    {
        /// <summary>
        /// The classical lines
        /// </summary>
        private readonly List<ClassicalLine> classicalLines = [];
        /// <summary>
        /// The quantum lines
        /// </summary>
        private readonly List<QuantumLine> quantumLines = [];
        /// <summary>
        /// The name
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantumCircuitObject"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public QuantumCircuitObject(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Adds the quantum line.
        /// </summary>
        /// <param name="qlName">Name of the ql.</param>
        /// <returns></returns>
        public void AddQuantumLine(string qlName)
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

        /// <summary>
        /// Quantums the line exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private bool QuantumLineExists(string name)
        {
            foreach (QuantumLine line in quantumLines)
            {
                if (line.GetName() == name)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Pushes the back h.
        /// </summary>
        /// <param name="qlName">Name of the ql.</param>
        /// <returns></returns>
        public void pushBackH(string qlName)
        {
            if(QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        matrix = Operations.Multscaler(matrix, 1 / Math.Sqrt(2));
                        Gate newGate = new Gate("H", new Operator(matrix));
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("H gate added at " + qlName);
                        break;
                    }
                }
            } else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }

        /// <summary>
        /// Pushes the back y.
        /// </summary>
        /// <param name="qlName">Name of the ql.</param>
        /// <returns></returns>
        public void pushBackY(string qlName)
        {
            if (QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 0, new Complex(1, -1) }, { new Complex(1, 1), 0 } });
                        Gate newGate = new Gate("Y", new Operator(matrix));
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("Y gate added at " + qlName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        /// <summary>
        /// Pushes the back x.
        /// </summary>
        /// <param name="qlName">Name of the ql.</param>
        /// <returns></returns>
        public void pushBackX(string qlName)
        {
            if (QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, 1 } });
                        Gate newGate = new Gate("X", new Operator(matrix));
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("X gate added at " + qlName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        /// <summary>
        /// Pushes the back z.
        /// </summary>
        /// <param name="qlName">Name of the ql.</param>
        /// <returns></returns>
        public void PushBackZ(string qlName)
        {
            if (QuantumLineExists(qlName))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlName)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 0 }, { 0, -1 } });
                        Gate newGate = new Gate("Z", new Operator(matrix));
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("Z gate added at " + qlName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        /// <summary>
        /// Pushes the back cnot.
        /// </summary>
        /// <param name="qlSource">The ql source.</param>
        /// <param name="qlTarget">The ql target.</param>
        /// <returns></returns>
        public void PushBackCNOT(string qlSource, string qlTarget)
        {
            if (QuantumLineExists(qlSource) && (QuantumLineExists(qlTarget)))
            {
                int sourceIndex = -1;
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlSource)
                    {
                        sourceIndex = quantumLine.GetLength();
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        string[] targets = { qlTarget };
                        int[] targetIndex = new int[1];
                        for(int i = 0; i < quantumLines.Count; i++)
                        {
                            if (quantumLines[i].GetName() == qlTarget)
                            {
                                targetIndex[0] = quantumLines[i].GetLength();
                            }
                        }
                        MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", new Operator(matrix), targets, targetIndex);
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("CNOT gate added at " + qlSource + " with target as " + qlTarget);
                    }
                    
                }
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlTarget)
                    {
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        string[] targets = { qlTarget };
                        MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT TARG", new Operator(matrix), qlSource, sourceIndex);
                        quantumLine.AddGate(newGate);
                        Console.WriteLine("CNOT gate Target added at " + qlTarget + " with source as " + qlSource);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Quantum line with name " + name + " does not exist.");
            }
        }
        /// <summary>
        /// Pushes the back tof.
        /// </summary>
        /// <param name="qlSource">The ql source.</param>
        /// <param name="qlTargets">The ql targets.</param>
        /// <returns></returns>
        public void pushBackTOF(string qlSource, string[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            if (QuantumLineExists(qlSource))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlSource)
                    {
                        sourceIndex = quantumLine.GetLength();
                        for(int i = 0; i < qlTargets.Length; i++)
                        {
                            for(int j = 0; j < quantumLines.Count; j++)
                            {
                                if (qlTargets[i] == quantumLines[j].GetName())
                                {
                                    indexes[i] = quantumLines[j].GetLength();
                                }
                            }
                        }
                        // Need to figure out how this shindig works.
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        MultiLineGateRoot newGate = new MultiLineGateRoot("TOF", new Operator(matrix), qlTargets, indexes);
                        quantumLine.AddGate(newGate);
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
                        if (quantumLine.GetName() == qlTargets[i])
                        {
                            // Need to figure out how this shindig works.
                            Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                            MultiLineGateTarget newGate = new MultiLineGateTarget("TOF", new Operator(matrix), qlSource, sourceIndex);
                            quantumLine.AddGate(newGate);
                            Console.WriteLine("TOF gate added at " + qlSource);
                        }
                    }
                } else
                {
                    Console.WriteLine("Error, quantum line " + qlTargets[i] + " DNE!");
                }
            }
        }

        /// <summary>
        /// Pushes the back cnot.
        /// </summary>
        /// <param name="qlSource">The ql source.</param>
        /// <param name="qlTargets">The ql targets.</param>
        /// <returns></returns>
        public void PushBackCNOT(string qlSource, string[] qlTargets)
        {
            int sourceIndex = -1;
            int[] indexes = new int[qlTargets.Length];
            if (QuantumLineExists(qlSource))
            {
                foreach (var quantumLine in quantumLines)
                {
                    if (quantumLine.GetName() == qlSource)
                    {
                        sourceIndex = quantumLine.GetLength();
                        for (int i = 0; i < qlTargets.Length; i++)
                        {
                            for (int j = 0; j < quantumLines.Count; j++)
                            {
                                if (qlTargets[i] == quantumLines[j].GetName())
                                {
                                    indexes[i] = quantumLines[j].GetLength();
                                }
                            }
                        }
                        // Need to figure out how this shindig works.
                        Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                        MultiLineGateRoot newGate = new MultiLineGateRoot("CNOT", new Operator(matrix), qlTargets, indexes);
                        quantumLine.AddGate(newGate);
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
                        if (quantumLine.GetName() == qlTargets[i])
                        {
                            // Need to figure out how this shindig works.
                            Matrix matrix = new Matrix(new Complex[,] { { 1, 1 }, { 1, -1 } });
                            MultiLineGateTarget newGate = new MultiLineGateTarget("CNOT", new Operator(matrix), qlSource, sourceIndex);
                            quantumLine.AddGate(newGate);
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
        /// <summary>
        /// Prints the circuit.
        /// </summary>
        /// <returns></returns>
        public void PrintCircuit()
        {
            Console.WriteLine("Quantum circuit: " + name);
            foreach (var quantumLine in quantumLines)
            {
                Console.Write(quantumLine.GetName());
                quantumLine.PrintGates();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <returns></returns>
        public void PrintOutput()
        {
            throw new NotImplementedException();
        }

        public int QuantumLinesCount()
        {
            return quantumLines.Count;
        }

        public string[] GetQuantumLineGates(string quantumLineName)
        {
            foreach (var quantumLine in quantumLines)
            {
                if (quantumLine.GetName() == quantumLineName)
                {
                    return quantumLine.GetGates();
                }
            }

            return [];
        }
        
    }
}
