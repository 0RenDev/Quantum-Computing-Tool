using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    public class SparseMatrix
    {
        // Dictionary to store non-zero values in the matrix.
        // Key is a tuple (row, column) and value is the non-zero element.
        private readonly ConcurrentDictionary<(int, int), Complex> values;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        // Constructor to initialize a sparse matrix of a given size
        public SparseMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            values = new ConcurrentDictionary<(int, int), Complex>();
        }

        public SparseMatrix(Complex[,] matrix)
        {
            Rows = matrix.GetLength(0);
            Cols = matrix.GetLength(1);
            values = new ConcurrentDictionary<(int, int), Complex>();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Complex value = matrix[i, j];
                    if (value != 0.0)
                    {
                        values[(i, j)] = value;
                    }
                }
            }

        }

        public static SparseMatrix FromMatrix(Matrix matrix)
        {
            int rows = matrix.rows;
            int cols = matrix.cols;
            SparseMatrix sparseMatrix = new SparseMatrix(rows, cols);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Complex value = matrix[i, j];
                    if (value != 0.0)
                    {
                        sparseMatrix[i, j] = value;
                    }
                }
            }

            return sparseMatrix;
        }

        // Indexer to get or set matrix values
        public Complex this[int row, int col]
        {
            get => values.TryGetValue((row, col), out var value) ? value : 0.0;
            set => values[(row, col)] = value;
        }

        // Matrix multiplication (SparseMatrix * SparseMatrix)
        public SparseMatrix Multiply(SparseMatrix other)
        {
            if (Cols != other.Rows)
                throw new InvalidOperationException("Matrix dimensions do not match for multiplication.");

            SparseMatrix result = new SparseMatrix(Rows, other.Cols);

            foreach (var entry in values)
            {
                int row = entry.Key.Item1;
                int col = entry.Key.Item2;
                Complex value = entry.Value;

                for (int k = 0; k < other.Cols; k++)
                {
                    Complex otherValue = other[col, k];
                    if (otherValue != 0.0)
                        result[row, k] += value * otherValue;
                }
            }

            return result;
        }

        public SparseMatrix TensorProduct(SparseMatrix other)
        {
            SparseMatrix result = new SparseMatrix(Rows * other.Rows, Cols * other.Cols);

            foreach (var entry in values)
            {
                int row = entry.Key.Item1;
                int col = entry.Key.Item2;
                Complex value = entry.Value;

                foreach (var otherEntry in other.values)
                {
                    int otherRow = otherEntry.Key.Item1;
                    int otherCol = otherEntry.Key.Item2;
                    Complex otherValue = otherEntry.Value;

                    // Compute the new row and column indices in the tensor product matrix
                    int newRow = row * other.Rows + otherRow;
                    int newCol = col * other.Cols + otherCol;

                    // Assign the product of the matrix values to the correct position in the result
                    result[newRow, newCol] = value * otherValue;
                }
            }

            return result;
        }

        public SparseMatrix ParallelTensorProduct(SparseMatrix other)
        {
            SparseMatrix result = new SparseMatrix(Rows * other.Rows, Cols * other.Cols);

            // Local accumulators for each thread
            var threadLocalAccumulators = new ThreadLocal<Dictionary<(int, int), Complex>>(() => new Dictionary<(int, int), Complex>(), true);

            _ = Parallel.ForEach(values, entry =>
            {
                int row = entry.Key.Item1;
                int col = entry.Key.Item2;
                Complex value = entry.Value;

                var localAccumulator = threadLocalAccumulators.Value;

                foreach (var otherEntry in other.values)
                {
                    int otherRow = otherEntry.Key.Item1;
                    int otherCol = otherEntry.Key.Item2;
                    Complex otherValue = otherEntry.Value;

                    // Compute the new row and column indices in the tensor product matrix
                    int newRow = row * other.Rows + otherRow;
                    int newCol = col * other.Cols + otherCol;

                    // Use the thread-local dictionary for accumulation
                    if (localAccumulator.ContainsKey((newRow, newCol)))
                        localAccumulator[(newRow, newCol)] += value * otherValue;
                    else
                        localAccumulator[(newRow, newCol)] = value * otherValue;
                }
            });

            // Merge the thread-local accumulators into the final result
            foreach (var localDict in threadLocalAccumulators.Values)
            {
                foreach (var entry in localDict)
                {
                    if (result.values.ContainsKey(entry.Key))
                        result.values[entry.Key] += entry.Value;
                    else
                        result.values[entry.Key] = entry.Value;
                }
            }

            return result;
        }

        public Complex[] MultiplyWithVector(Complex[] vector)
        {
            if (Cols != vector.Length)
                throw new InvalidOperationException("Matrix column count must match vector length.");

            Complex[] result = new Complex[Rows];

            foreach (var entry in values)
            {
                int row = entry.Key.Item1;
                int col = entry.Key.Item2;
                Complex value = entry.Value;

                // Perform the matrix-vector multiplication and accumulate the result for each row
                result[row] += value * vector[col];
            }

            return result;
        }

        // Method to display the sparse matrix (for debugging purposes)
        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write(this[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static SparseMatrix Identity(int size)
        {
            SparseMatrix identityMatrix = new SparseMatrix(size, size);

            for (int i = 0; i < size; i++)
            {
                identityMatrix[i, i] = 1.0; // Set diagonal elements to 1
            }

            return identityMatrix;
        }
    }
}
