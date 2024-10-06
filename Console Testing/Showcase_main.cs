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


        //Attempted testing for dictionary I'm not super sure if I did this right but regardless I'm leaving it here as a record
        /*
        int n = 14; 
        QuantumCircuitBuilder qc = new QuantumCircuitBuilder(n, 0);

        qc = new(12, 0);
        qc.addGateH(0);
        qc.addGateH(1);
        qc.addGateH(2);
        qc.addGateH(3);
        qc.addGateH(4);
        qc.addGateH(5);
        qc.addGateH(6);
        qc.addGateH(7);
        qc.addGateH(8);
        qc.addGateH(9);
        qc.addGateH(10);
        qc.addGateH(11);
        //qc.addGateH(12);
        //qc.addGateH(13);
        CircuitExecution exe = new CircuitExecution(qc);

        exe.ExecuteCircuit();
        */


        // testing for 2D array, not important will delete before merge

        /*
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
        */

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
