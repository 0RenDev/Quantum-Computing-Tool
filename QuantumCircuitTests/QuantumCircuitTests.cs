using NUnit.Framework;
using QuantumCircuits;
using LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using System.Data;

namespace QuantumCircuit.Tests
{
    [TestFixture()]
    public class QuantumCircuitTests
    {
        [Test]
        public void QuantumCircuitExecutionTest()
        {
            QuantumCircuitBuilder qc = new(2, 0);

            qc.addGateH(1);
            qc.addGateCX(0, 1);
            qc.addGateX(1);

            CircuitExecution exe = new(qc); 

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            Assert.AreEqual(result.GetState(), new Complex[] { 0, 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2), 0 });
        }

        [Test]//Put them all in one method for the state vector array will break it up for the sake of readability
        public void GateTests(){

            //SingleHGate
            QuantumCircuitBuilder qc = new (1, 0);
            qc.addGateH(0);
            
            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();

            LinearAlgebra.Vector[] resultArr = new LinearAlgebra.Vector[19];

            resultArr[0] = result;


            //SingleXGate
            qc = new (1,0);
            qc.addGateX(0);

            exe = new(qc);

            result = exe.ExecuteCircuit();
            resultArr[1] = result;


            //SingleZGate
            qc = new(1,0);
            
            //TwoHgate
            qc = new(1,0);
            qc.addGateH(0);
            qc.addGateH(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[3] = result;

            //HGateAndXgate
            qc = new(1,0);
            qc.addGateH(0);
            qc.addGateX(0);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[4] = result;

            //TwoQubitTest_H1X2
            qc = new(2,0);
            qc.addGateH(0);
            qc.addGateX(1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[5] = result;

            //TwoQubitTest_H1CX
            qc = new(2,0);
            qc.addGateH(0);
            qc.addGateCX(0, 1);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[6] = result;

            //ThreeQubitTest_H1X2Z3RRX

            //ThreeQubitTest_H
            qc = new(3,0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateH(2);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[8] = result;

            //FourQubitTest_RandomGates
            
            //FourQubitTest_H12X34
            qc = new(4,0);
            qc.addGateH(0);
            qc.addGateH(1);
            qc.addGateX(2);
            qc.addGateX(3);

            exe = new(qc);
            result = exe.ExecuteCircuit();
            resultArr[10] = result;

            /*Assert.AreEqual(result.GetState(), new Complex[] {0*Complex.ImaginaryOne,0*Complex.ImaginaryOne,0*Complex.ImaginaryOne,0.5000000000000001+0*Complex.ImaginaryOne,
                                                              0*Complex.ImaginaryOne,0.5000000000000001+0*Complex.ImaginaryOne, 0*Complex.ImaginaryOne,0*Complex.ImaginaryOne,
                                                              0*Complex.ImaginaryOne,0.5000000000000001+0*Complex.ImaginaryOne,0*Complex.ImaginaryOne,0*Complex.ImaginaryOne,
                                                              0*Complex.ImaginaryOne,0*Complex.ImaginaryOne,0*Complex.ImaginaryOne, 0.5000000000000001+0*Complex.ImaginaryOne} );
            */
        }

        [Test]
        public void SingleXGateTest(){
            QuantumCircuitBuilder qc = new (1,0);
            qc.addGateX(0);

            CircuitExecution exe = new(qc);

            LinearAlgebra.Vector result = exe.ExecuteCircuit();
            //Assert.AreEqual(result.GetState(),new Complex[] { 0.7071067811865476+0*Complex.ImaginaryOne, 0.7071067811865476+0*Complex.ImaginaryOne});

        }

    }
}