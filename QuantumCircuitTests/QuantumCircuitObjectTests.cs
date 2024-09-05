using NUnit.Framework;
using QuantumCircuit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace QuantumCircuit.Tests
{
    [TestFixture()]
    public class QuantumCircuitObjectTests
    {
        [Test()]
        public void QuantumCircuitObjectTest()
        {
            // Write a Unit Test to check if the QuantumCircuitObject is created
            QuantumCircuitObject qco = new QuantumCircuitObject("test");
            Assert.IsNotNull(qco);
        }

        [Test()]
        public void AddQuantumLineTest()
        {
            // Write a Unit Test to check if the QuantumLine is added to the QuantumCircuitObject
            QuantumCircuitObject qco = new QuantumCircuitObject("test");
            qco.AddQuantumLine("test");
            Assert.AreEqual(1, qco.QuantumLinesCount());

        }

        [Test()]
        public void PushBackHTest()
        {
            // Write a Unit Test to check if the Hadamard gate is added to the QuantumLine
            QuantumCircuitObject qco = new("test");
            qco.AddQuantumLine("test");
            qco.PushBackH("test");
            string[] gates = qco.GetQuantumLineGates("test");
            Assert.AreEqual(1, gates.Length);
            Assert.AreEqual("H", gates[0]);
        }

        [Test()]
        public void PushBackYTest()
        {
            // Write a Unit Test to check if the Y gate is added to the QuantumLine
            QuantumCircuitObject qco = new("test");
            qco.AddQuantumLine("test");
            qco.PushBackY("test");
            string[] gates = qco.GetQuantumLineGates("test");
            Assert.AreEqual(1, gates.Length);
            Assert.AreEqual("Y", gates[0]);
        }

        [Test()]
        public void PushBackXTest()
        {
            // Write a Unit Test to check if the X gate is added to the QuantumLine
            QuantumCircuitObject qco = new("test");
            qco.AddQuantumLine("test");
            qco.PushBackX("test");
            string[] gates = qco.GetQuantumLineGates("test");
            Assert.AreEqual(1, gates.Length);
            Assert.AreEqual("X", gates[0]);

        }

        [Test()]
        public void PushBackZTest()
        {
            // Write a Unit Test to check if the Z gate is added to the QuantumLine
            QuantumCircuitObject qco = new("test");
            qco.AddQuantumLine("test");
            qco.PushBackZ("test");
            string[] gates = qco.GetQuantumLineGates("test");
            Assert.AreEqual(1, gates.Length);
            Assert.AreEqual("Z", gates[0]);

        }

        [Test()]
        public void PushBackCNOTTest()
        {
            // Write a Unit Test to check if the CNOT gate is added to the QuantumLine
            QuantumCircuitObject qco = new("test");
            qco.AddQuantumLine("test1");
            qco.AddQuantumLine("test2");
            qco.PushBackCNOT("test1", "test2");
            
            string[] gates1 = qco.GetQuantumLineGates("test1");
            
            Assert.AreEqual(1, gates1.Length);
            Assert.AreEqual("[CNOT targetting at: test2:0]", gates1[0]);
            
        }

        [Test()]
        public void PushBackTOFTest()
        {
            // Write a Unit Test to check if the TOF gate is added to the QuantumLine
            QuantumCircuitObject qco = new("test");

            qco.AddQuantumLine("test1");
            qco.AddQuantumLine("test2");
            qco.AddQuantumLine("test3");

            string[] targets = { "test2", "test3" };
            qco.PushBackTOF("test1", targets);
            string[] gates = qco.GetQuantumLineGates("test1");
            Assert.AreEqual(1, gates.Length);
            Assert.AreEqual("[TOF targetting at: test2:0 test3:0]", gates[0]);
        }

    }
}