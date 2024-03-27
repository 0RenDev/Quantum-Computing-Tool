using System;
using System.Numerics; // Required for Complex numbers
using System.Runtime.Intrinsics;
using Console_Testing;
using LinearAlgebra;

class Program
{
    // The main is meant to be freely changed to contain any number of showcases. It should not contain any permanent code and is purely for testing the code written in the library
    static void Main()
    {
        // instance containong examples
        Showcases example = new Showcases();

        // run specific examples
        //example.timeTestTensor(44, 41, 96, 82);
        //example.timeTestTensor(106, 12, 90, 16);

        //QuantumCircuitObject cs = new QuantumCircuitObject("Test");
        //cs.AddQuantumLine("X");
        //cs.pushBackH("X");
        //cs.pushBackH("X");
        //cs.pushBackH("X");

        //cs.AddQuantumLine("Y");
        //cs.pushBackH("Y");
        //cs.printCircuit();

        //example.vector_times_matrix();
        //example.mmultThreadedMult();

        Showcases.QbitEvolutionDemo();   


        Console.ReadKey();  
    }

}
