namespace QuantumCircuit_Sean
{
    public class QuantumCirucit_Q
    {
        public int Qubits { get; private set; }
        public int ClassicalBits { get; private set; }
        private Queue<Gate> Gates = new Queue<Gate>();

        public QuantumCirucit_Q(int qubits, int classicalBits)
        {
            Qubits = qubits;
            ClassicalBits = classicalBits;
        }

        public void AddGate(Gate gate)
        {
            Gates.Enqueue(gate);
        }

        public void Execute(QuantumRegister register)
        {
            foreach (var gate in Gates)
            {
                register.ApplyGate(gate);
            }
        }
    }
}
