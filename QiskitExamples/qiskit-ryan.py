from qiskit import QuantumCircuit
from qiskit.quantum_info import Statevector 
from qiskit.visualization import plot_histogram, plot_bloch_multivector
import matplotlib.pyplot as plt
import numpy as n
import qiskit.quantum_info as qi

# Create the quantum circuit with two qbits = 0, 1
qc = QuantumCircuit(2)

# Add h-gate to qbit 0
qc.h(0) 

# perform a CZGate on qbit 1, controlled by qbit 0
qc.cz(0, 1) 

# convert the final product into a state vector to be printed
sv = qi.Statevector.from_instruction(qc)

print('Final state vector : ', sv)

