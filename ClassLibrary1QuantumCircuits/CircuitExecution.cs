using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuantumCircuits
{
    public class CircuitExecution
    {
        // matrices for defined gates

        private Matrix nopgate = new Matrix(new Complex[,] { { 1, 0 },
                                                             { 0, 1 } });

        private Matrix xgate = new Matrix(new Complex[,] { { 0, 1 },
                                                           { 1, 0 } });

        private Matrix hgate = new Matrix(new Complex[,] { { 1/(Math.Sqrt(2)) , 1/(Math.Sqrt(2)) },
                                                           { 1/(Math.Sqrt(2)),  -1/(Math.Sqrt(2)) } });

        private Matrix cxgate = new Matrix(new Complex[,] { { 1, 0 ,0 ,0 },
                                                            { 0, 1, 0, 0 },
                                                            { 0, 0 ,0 ,1 },
                                                            { 0, 0, 1, 0 } });

        private Matrix cxgateflipped = new Matrix(new Complex[,] { { 1, 0 ,0 ,0 },
                                                                   { 0, 0, 0, 1 },
                                                                   { 0, 0 ,1 ,0 },
                                                                   { 0, 1, 0, 0 } });

        private Matrix swapgate = new Matrix(new Complex[,] { {1, 0, 0, 0},
                                                              {0, 0, 1, 0},
                                                              {0, 1, 0, 0},
                                                              {0, 0, 0, 1} });
        private Matrix toffoli = new Matrix(new Complex[,] { { 1, 0 ,0 ,0, 0, 0 ,0 ,0},
                                                             { 0, 1 ,0 ,0, 0, 0 ,0 ,0},
                                                             { 0, 0 ,1 ,0, 0, 0 ,0 ,0},
                                                             { 0, 0 ,0 ,1, 0, 0 ,0 ,0},
                                                             { 0, 0 ,0 ,0, 1, 0 ,0 ,0},
                                                             { 0, 0 ,0 ,0, 0, 1 ,0 ,0},
                                                             { 0, 0 ,0 ,0, 0, 0 ,0 ,1},
                                                             { 0, 0 ,0 ,0, 0, 0 ,1 ,0},});

        private Matrix toffoliflipped = new Matrix(new Complex[,] { { 1, 0 ,0 ,0, 0, 0 ,0 ,0},
                                                                    { 0, 1 ,0 ,0, 0, 0 ,0 ,0},
                                                                    { 0, 0 ,1 ,0, 0, 0 ,0 ,0},
                                                                    { 0, 0 ,0 ,0, 0, 0 ,0 ,1},
                                                                    { 0, 0 ,0 ,0, 1, 0 ,0 ,0},
                                                                    { 0, 0 ,0 ,0, 0, 1 ,0 ,0},
                                                                    { 0, 0 ,0 ,0, 0, 0 ,1 ,0},
                                                                    { 0, 0 ,0 ,1, 0, 0 ,0 ,0},});

        private Matrix zgate = new Matrix(new Complex[,] { { 1, 0 },
                                                   { 0, -1 } });





        QuantumCircuitBuilder circuit;

        List<List<Gate>> executionColumns = new List<List<Gate>>();

        LinearAlgebra.Vector stateVector = new LinearAlgebra.Vector(new Complex[] { 1, 0 });

        public CircuitExecution(QuantumCircuitBuilder inputcircuit)
        {
            circuit = inputcircuit;

            int longestLine = 0;

            List<LinearAlgebra.Vector> tempVectorList = new List<LinearAlgebra.Vector>();

            for( int i = 0; i < circuit.quantumLines.Length; i++)
            {
                if(longestLine < circuit.quantumLines[i].Count)
                {
                    longestLine = circuit.quantumLines[i].Count;
                }

                tempVectorList.Add(new LinearAlgebra.Vector(new Complex[] { 1, 0 }));
            }

            for(int i = 1; i < tempVectorList.Count; i++)
            {
                stateVector = Operations.TensorProductofVectors(stateVector, tempVectorList[i]);
            }


            for (int i = 0; i < longestLine; i++)
            {
                List<Gate> columnOfGates = new List<Gate>();
                for( int j = 0; j < circuit.quantumLines.Length; j++ )
                {
                    if (circuit.quantumLines[j].Count > 0)
                    {
                        columnOfGates.Add(circuit.quantumLines[j][0]);
                        circuit.quantumLines[j].RemoveAt(0);
                    }
                    else
                    {
                        Gate spacerNoOp = new Gate(GateTypes.NOP, j, new List<int> { }, i);
                        columnOfGates.Add(spacerNoOp);
                    }
                }
                executionColumns.Add(columnOfGates);
            }
        }


        public Matrix BuildNextColumnMatrix()
        {
            List<Gate> currentColumn = executionColumns[0];

            Matrix combinedMatrix = new Matrix(new Complex[,] { { 1 } } );

            while (currentColumn.Count > 0)
            {
                int index = currentColumn.Count - 1;

                switch (currentColumn[index].type)
                {

                    case GateTypes.CXC:
                        if (currentColumn[index - 1].type != GateTypes.CXT)
                        {
                            throw new Exception("Incomplete CNOT gate");
                        }
                        else
                        {
                            index = index - 1;
                            currentColumn.RemoveAt(index);
                            combinedMatrix = Operations.TensorProduct(combinedMatrix, GateToMatrix(currentColumn[index]));
                            currentColumn.RemoveAt(index);
                        }
                        break;

                    case GateTypes.CXT:
                        if (currentColumn[index - 1].type != GateTypes.CXC)
                        {
                            throw new Exception("Incomplete CNOT gate");
                        }
                        else
                        {
                            index = index - 1;
                            currentColumn.RemoveAt(index);
                            combinedMatrix = Operations.TensorProduct(combinedMatrix, GateToMatrix(currentColumn[index]));
                            currentColumn.RemoveAt(index);
                        }
                        break;

                    case GateTypes.SWAP:
                        

                    case GateTypes.TOF:
                        if (currentColumn[index - 1].type != GateTypes.TOC && currentColumn[index - 2].type != GateTypes.TOC )
                        {
                            throw new Exception("Incomplete TOF gate");
                        }
                        else
                        {
                            index = index - 1;
                            currentColumn.RemoveAt(index);
                            index = index - 1;
                            currentColumn.RemoveAt(index);
                            combinedMatrix = Operations.TensorProduct(combinedMatrix, GateToMatrix(currentColumn[index]));
                            currentColumn.RemoveAt(index);
                        }
                        break;

                    case GateTypes.TOC:
                        if (currentColumn[index - 1].type != GateTypes.TOC && currentColumn[index - 2].type != GateTypes.TOF )
                        {
                            throw new Exception("Incomplete TOF gate");
                        }
                        else
                        {
                            index = index - 1;
                            currentColumn.RemoveAt(index);
                            index = index - 1;
                            currentColumn.RemoveAt(index);
                            combinedMatrix = Operations.TensorProduct(combinedMatrix, GateToMatrix(currentColumn[index]));
                            currentColumn.RemoveAt(index);
                        }
                        break;

                    default:
                        combinedMatrix = Operations.TensorProduct(combinedMatrix, GateToMatrix(currentColumn[index]));
                        currentColumn.RemoveAt(index);
                        break;
                }
            }
            return combinedMatrix;
        }


        public LinearAlgebra.Vector ExecuteCircuit()
        {
            while(executionColumns.Count > 0) 
            {
                Matrix combinedoperation = BuildNextColumnMatrix();
                stateVector = Operations.MatrixVectorMult(combinedoperation,stateVector);
                executionColumns.RemoveAt(0);
            }
            return stateVector;
        }




        private Matrix GateToMatrix(Gate gate)
        {
            switch (gate.type)
            {
                case GateTypes.NOP:
                    return nopgate;
                case GateTypes.XGT:
                    return xgate;
                case GateTypes.HGT:
                    return hgate;
                case GateTypes.CXC:
                    return cxgate;
                case GateTypes.CXT:
                    return cxgateflipped;
                case GateTypes.SWAP:
                    return swapgate; 
                case GateTypes.TOF:
                    return toffoliflipped;
                case GateTypes.TOC:
                    return toffoli;
                case GateTypes.ZGT:
                    return zgate;
                default:
                    return nopgate;
            }
        }















        /*
         * 
         * Full measurement operations
         * 
         * 
          public int MEQGateOp(LinearAlgebra.Vector input)
        {
            // get probabilites vector
            double[] probabilites = StatevectorProbabilities(input.elements);

            return MeasureProbabilities(probabilites); ;
        }
        

        public static double[] StatevectorProbabilities(Complex[] statevector)
        {
            // Check if the input vector is a valid statevector
            if (!IsValidStatevector(statevector))
            {
                throw new ArgumentException("Invalid statevector. The norm (magnitude) of the statevector must equal 1.");
            }

            // Calculate unnormalized probabilities
            double[] unnormalizedProbabilities = new double[statevector.Length];
            for (int i = 0; i < statevector.Length; i++)
            {
                unnormalizedProbabilities[i] = Math.Pow(statevector[i].Magnitude, 2);
            }

            // Calculate normalization factor
            double normalizationFactor = 0;
            for (int i = 0; i < unnormalizedProbabilities.Length; i++)
            {
                normalizationFactor += unnormalizedProbabilities[i];
            }

            // Normalize probabilities
            double[] probabilities = new double[statevector.Length];
            for (int i = 0; i < probabilities.Length; i++)
            {
                probabilities[i] = unnormalizedProbabilities[i] / normalizationFactor;
            }

            return probabilities;
        }

        private int MeasureProbabilities(double[] probabilities)
        {
            // Perform measurement
            Random random = new Random();
            double rand = random.NextDouble();
            double cumulativeProbability = 0;
            int measurementOutcome = -1;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (rand < cumulativeProbability)
                {
                    measurementOutcome = i;
                    break;
                }
            }

            return measurementOutcome;
        }

        public static bool IsValidStatevector(Complex[] statevector)
        {
            // Calculate the norm (magnitude) of the statevector
            double norm = 0;
            for (int i = 0; i < statevector.Length; i++)
            {
                norm += Math.Pow(statevector[i].Magnitude, 2);
            }

            // Check if the norm equals 1 (within a small tolerance)
            return Math.Abs(norm - 1) < 1e-10;
        }
         
         */






        // Override the toString method
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Circuit Execution:");

            // Append qubit states
            sb.AppendLine("Qubits StateVector: ");

            sb.Append(stateVector.ToString());

            sb.Append("\n");

            for (int i = 0; i < executionColumns.Count; i++)
            {
                sb.Append($"Column {i + 1}:\n");
                foreach (Gate gate in executionColumns[i])
                {
                    sb.Append(gate.ToString() + "\t");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

    }
}
