from qiskit.quantum_info import Statevector, Operator
from qiskit import QuantumCircuit
from qiskit.visualization import plot_histogram
import matplotlib.pyplot as plt
import numpy as np

# We can model state vectors using the Statevector class
u = Statevector([1/np.sqrt(2), 1/np.sqrt(2)])
v = Statevector([-1/2 + 1j/np.sqrt(2), -1/2 - 1j/np.sqrt(2)])
print("U = " + str(u.draw('text')))
print("V = " + str(v.draw('text')))

# We can check if they are valid states as follows 
print('U is valid: ' + str(u.is_valid()))
print('V is valid: ' + str(v.is_valid()))

# Measuring states corresponds to the probability of each state
print("Result of measuring U: ")
print(u.measure())

print("Result of sampling U 1000 times: ")
measurements = u.sample_counts(1000)
print(measurements)
histogram = plot_histogram(measurements)
plt.show()

# We can use Operators to perform a series of operations on a state
X = Operator([[0, 1], [1, 0]])
Y = Operator([[0, -1.0j], [1.0j, 0]])
Z = Operator([[1, 0], [0, -1]])

u_out = u.evolve(X)
u_out = u_out.evolve(Y)
u_out = u_out.evolve(Z)

print("Result of ZYXU = " + str(u_out.draw("text")))

# We can also represent the series of operations as a quantum circuit
circuit = QuantumCircuit(1) # A Quantum Cirucit with 1 qubit
circuit.x(0)
circuit.y(0)
circuit.z(0)
print(circuit.draw())

u_out = u.evolve(circuit)
print("Result of ZYXU circuit = " + str(u_out.draw("text")))

# Representing tensor products
one = Statevector.from_label('1')
zero = Statevector.from_label('0')
plus = Statevector.from_label('+')

two_state = zero.tensor(one) # Representation of 01 system
print(two_state.draw('text'))

state = Statevector([1/np.sqrt(2), 1j/np.sqrt(2)])
comb_state = plus.tensor(state)
print('State = ' + str(comb_state.draw('text')))

# Let's model a CX gate
CX = Operator([
    [1, 0, 0, 0],
    [0, 1, 0, 0],
    [0, 0, 0, 1],
    [0, 0, 1, 0]
])

CX_result = comb_state.evolve(CX)
print('Result of CX x state = ' + str(CX_result.draw('text'))) # Notice how Y inverts when X is 1