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

    }
}