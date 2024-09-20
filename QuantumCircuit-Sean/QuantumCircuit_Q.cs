namespace QuantumCircuit_Sean
{
    public class QuantumCirucit_Q(int qubits, int classicalBits)
    {
        public int Qubits { get; private set; } = qubits;
        public int ClassicalBits { get; private set; } = classicalBits;
        private Queue<Gate> Gates = new();

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
