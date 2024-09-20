from qiskit.quantum_info import Statevector
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
import numpy as np

u = Statevector([1, 0, 0, 0])
qreg_q = QuantumRegister(2, 'q')
circuit = QuantumCircuit(qreg_q)

circuit.x(qreg_q[0])
u = u.evolve(circuit)

print(u)
print(u.probabilities())