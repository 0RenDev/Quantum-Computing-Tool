using NUnit.Framework;
using QuantumCircuit_Sean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace QuantumCircuit_Sean.Tests
{
    [TestFixture()]
    public class QuantumCirucit_QTests
    {
        [Test()]
        public void OneQubitTest()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(1, 0);
            QuantumRegister register = new QuantumRegister(1);
            circuit.AddGate(new X(0));

            circuit.Execute(register);

            Assert.That(register.State, Is.EqualTo(new Complex[] { 0, 1 }));
        }

        [Test()]
        public void TwoQubitNoControlTest()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(2, 0);
            QuantumRegister register = new QuantumRegister(2);
            circuit.AddGate(new X(0));
            circuit.AddGate(new X(1));

            circuit.Execute(register);

            Assert.That(register.State, Is.EqualTo(new Complex[] { 0, 0, 0, 1 }));
        }

        [Test()]
        public void TwoQubitWithControlTest()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(2, 0);
            QuantumRegister register = new QuantumRegister(2);
            circuit.AddGate(new X(0));
            circuit.AddGate(new H(1));
            circuit.AddGate(new CX(0, 1));

            circuit.Execute(register);

            foreach (Complex number in register.State)
            {
                Console.WriteLine(number);
            }
            Assert.That(register.State, Is.EqualTo(new Complex[] { 0, 1, 0, 0 }));
        }
    }
}