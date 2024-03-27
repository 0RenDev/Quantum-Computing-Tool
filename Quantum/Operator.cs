using System;
using System.Numerics;
using LinearAlgebra;

namespace Quantum
{
    public class Operator
    {

        private readonly Matrix data;

        public Matrix Data { get { return data; } }

        public Operator(Matrix data) { this.data = data;}

        public Operator(Complex[,] data) { this.data = new Matrix(data); }

        public override string ToString()
        {
            return data.ToString();
        }

    }


}
