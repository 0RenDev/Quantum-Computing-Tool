from qiskit.quantum_info import Statevector
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
import numpy as np

u = Statevector([1, 0, 0, 0])
qreg_q = QuantumRegister(2, 'q')
circuit = QuantumCircuit(2)

circuit.h(1)
circuit.cx(1, 0)
circuit.x(1)
u = u.evolve(circuit)

print(u)
print(u.probabilities())