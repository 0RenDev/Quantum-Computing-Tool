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
        // instance containing examples
        Showcases example = new Showcases();

        int n = 14; 
        QuantumCircuitBuilder qc = new QuantumCircuitBuilder(n, 0);


        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                //input bits
                qc.addGateH(j);
            }
        }

        CircuitExecution exe = new CircuitExecution(qc);

        exe.ExecuteCircuit();

        // run specific examples
        //example.timeTestTensor(44, 41, 96, 82);
        //example.timeTestTensor(106, 12, 90, 16);

        //example.vector_times_matrix();
        //Console.WriteLine("125x1000 Matrix x 1000x350 Matrix Matrix Testing");
        //Showcases.MultThreadedMult();

        /*
          
         These are broken due to the new implementation
          
        //Console.WriteLine("-------------\nQuantum Adder Structure");
        //Showcases.QuantumAdderConstruction();
        //Console.WriteLine("-------------\nTof Test Structure");
        //Showcases.TofGateTest();
        //Console.WriteLine("-------------\nQbit Evolution Demo");
        //Showcases.QbitEvolutionDemo();
        */
        
        example.HalfAdderTest();

        

        Console.ReadKey();  
    }

}
