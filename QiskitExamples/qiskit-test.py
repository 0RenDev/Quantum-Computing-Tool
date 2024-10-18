from qiskit.quantum_info import Statevector
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
import numpy as np

u = Statevector([1, 0, 0, 0])
qreg_q = QuantumRegister(2, 'q')
circuit = QuantumCircuit(qreg_q)

circuit.h(qreg_q[0])
circuit.cx(0, 1)
circuit.x(1)
u = u.evolve(circuit)

print(u)
print(u.probabilities())