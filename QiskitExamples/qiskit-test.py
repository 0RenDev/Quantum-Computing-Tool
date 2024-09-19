from qiskit.quantum_info import Statevector
from qiskit import QuantumCircuit, QuantumRegister
import numpy as np

u = Statevector([1, 0, 0, 0])
qr = QuantumRegister(2)

qc = QuantumCircuit(qr)
qc.x(0)
qc.h(1)
qc.cx(0, 1)
u = u.evolve(qc)

print(u)