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
        // run specific example
        example.vector_times_matrix();
        //example.mmultThreadedMult();

        Console.ReadKey();  
    }

}
