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
    }
}