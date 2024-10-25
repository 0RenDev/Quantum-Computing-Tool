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
        public void TwoQubitNoControlTest1()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(2, 0);
            QuantumRegister register = new QuantumRegister(2);
            circuit.AddGate(new X(0));
            // circuit.AddGate(new X(1));

            circuit.Execute(register);

            Assert.That(register.State, Is.EqualTo(new Complex[] { 0, 0, 1, 0 }));
        }

        [Test()]
        public void TwoQubitNoControlTest2()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(2, 0);
            QuantumRegister register = new QuantumRegister(2);
            circuit.AddGate(new X(0));
            circuit.AddGate(new H(1));

            circuit.Execute(register);

            Assert.That(register.State, Is.EqualTo(new Complex[] { 0, 0, 1 / Complex.Sqrt(2), 1 / Complex.Sqrt(2) }));
        }

        [Test()]
        public void TwoQubitWithControlTest()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(2, 0);
            QuantumRegister register = new QuantumRegister(2);
            circuit.AddGate(new H(0));
            circuit.AddGate(new X(1));
            circuit.AddGate(new CX(0, 1));

            circuit.Execute(register);

            foreach (Complex number in register.State)
            {
                Console.WriteLine(number);
            }

            foreach(double number in register.ProbabilityVector())
            {
                Console.WriteLine(number);
            }
            Assert.That(register.ProbabilityVector(), Is.EqualTo(new double[] { 0, 0.5, 0.5, 0 }));
        }

        [Test()]
        public void ThreeQubitWithControlTest()
        {
            QuantumCirucit_Q circuit = new QuantumCirucit_Q(3, 0);
            QuantumRegister register = new QuantumRegister(3);
            circuit.AddGate(new H(0));
            circuit.AddGate(new CX(0, 1));
            circuit.AddGate(new X(1));
            circuit.AddGate(new CX(1, 2));

            circuit.Execute(register);

            foreach (Complex number in register.State)
            {
                Console.WriteLine(number);
            }

            foreach (double number in register.ProbabilityVector())
            {
                Console.WriteLine(number);
            }
            Assert.That(register.ProbabilityVector(), Is.EqualTo(new double[] { 0, 0.5, 0, 0, 0, 0, 0.5, 0 }));
        }

        [Test()]
        public void TwelveQubitTest()
        {
            
            for(int i=0; i < 10; i++)
            {
                QuantumCirucit_Q circuit = new QuantumCirucit_Q(i, 0);
                QuantumRegister register = new QuantumRegister(i);
                for (int j = 0; j < i; j++)
                {
                    for(int k = 0; k < i; k++)
                    {
                        circuit.AddGate(new H(k));
                    }
                }
                circuit.Execute(register);
            }
            Assert.Pass();
        }
       
    }
}