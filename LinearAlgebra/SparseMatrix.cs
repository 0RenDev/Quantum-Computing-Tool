using System;
using System.Collections.Generic;
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
        private Dictionary<(int, int), Complex> values;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        // Constructor to initialize a sparse matrix of a given size
        public SparseMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            values = new Dictionary<(int, int), Complex>();
        }

        public SparseMatrix(Complex[,] matrix)
        {
            Rows = matrix.GetLength(0);
            Cols = matrix.GetLength(1);
            values = new Dictionary<(int, int), Complex>();

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
            get
            {
                if (values.ContainsKey((row, col)))
                    return values[(row, col)];
                return 0.0;
            }
            set
            {
                if (value != 0.0)
                    values[(row, col)] = value;
                else
                    values.Remove((row, col));  // Ensure zero values are removed from dictionary
            }
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

        // Tensor product (Kronecker product)
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

                    result[row * other.Rows + otherRow, col * other.Cols + otherCol] = value * otherValue;
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
