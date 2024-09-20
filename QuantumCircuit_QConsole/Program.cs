using System;
using System.Numerics; // Required for Complex numbers
using System.Runtime.Intrinsics;
using QuantumCircuit_Sean;


class Program
{
    // The main is meant to be freely changed to contain any number of showcases. It should not contain any permanent code and is purely for testing the code written in the library
    static void Main()
    {
        for (int i = 0; i < 12; i++)
        {
            int size = 14;
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(size, 0);
            QuantumRegister register = new QuantumRegister(size);
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    circuit.AddGate(new H(k));
                }
            }
            circuit.Execute(register);
        }
    }
}