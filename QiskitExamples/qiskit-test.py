from qiskit.quantum_info import Statevector
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
import numpy as np

u = Statevector([1, 0, 0, 0, 0, 0, 0, 0])
qreg_q = QuantumRegister(3, 'q')
circuit = QuantumCircuit(qreg_q)

circuit.x(qreg_q[0])
circuit.cx(qreg_q[0], qreg_q[1])
circuit.h(qreg_q[1])
circuit.cx(qreg_q[1], qreg_q[2])
u = u.evolve(circuit)

print(u)
print(u.probabilities())