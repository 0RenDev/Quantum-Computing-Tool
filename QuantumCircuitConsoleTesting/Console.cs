using LinearAlgebra;
using QuantumCircuits;
using System.Numerics;

class Test
{
    static void Main()
    {
        // build a circuit with one quantum and one classical line
        QuantumCircuitBuilder qc = new QuantumCircuitBuilder(3, 1);

        // half adder

        //input bits
        qc.addGateX(0);
        qc.addGateX(1);

        qc.addGateTOF(2, 1, 0);
        qc.addGateCX(1, 0);
    
        // print out circuit
        Console.WriteLine(qc.ToString());

        CircuitExecution exe = new CircuitExecution(qc);

        // print out execution columns
        Console.WriteLine(exe.ToString());

        // returns the statevector after executing all columns
        Console.WriteLine(exe.ExecuteCircuit());

    }

}