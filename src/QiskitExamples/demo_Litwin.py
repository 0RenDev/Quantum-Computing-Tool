from IPython.display import display
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
from qiskit.primitives import Sampler
from qiskit.quantum_info import Operator
from qiskit.visualization import plot_histogram
from qiskit.quantum_info import Statevector
from numpy import sqrt
from numpy import array
from numpy import matmul
import matplotlib.pyplot as plt


print("Program for checking basic matrix, vectors, quantum states, and operations: \n")

print("Zero state of a qubit")
ket0 = array([1, 0])
display("Ket0: " , ket0)

print("Zero state of a qubit")
ket1 = array([0, 1])
display("Ket1: ", ket1, "\n")

print("superposition of two computational basis vectors.")
ketplus = ((1 / sqrt(2))*ket0) + ((1 / sqrt(2)) * ket1)
display("Ket+: " , ketplus)
i = Statevector(ketplus).measure()
display("Measurement: ", i, "\n")

print("different superposition of two computational basis vectors.")
ketminus = ((1 / sqrt(2))*ket0) - ((1 / sqrt(2)) * ket1)
display("Ket-:", ketminus)
j = Statevector(ketplus).measure()
display("Measurement: ", j, "\n")

print("Unary Operators: ")
print("identity matrix / No-op")
I = Operator([[1, 0], [0, 1]])
display("I ", I, "\n")

print("inverse matrix / Not operation/ bit-flip")
X = Operator([[0, 1], [1, 0]])
display("X ", X, "\n")

print("Rotate about the y axis")
Y = Operator([[0, -1.0j], [1.0j, 0]])
display("Y ", Y, "\n")

print("Rotate about the z axis pi radians 180 degrees")
Z = Operator([[1, 0], [0, -1]])
display("Z ", Z, "\n")

print("Rotate about the z axis pi/2 radians, 90 degrees")
S = Operator([[1, 0], [0, 1.0j]])
display("S ", S, "\n")

print("Rotate about the z axis pi/4 radians, 45 degrees")
T = Operator([[1, 0], [0, (1 + 1.0j) / sqrt(2)]])
display("T ", T, "\n")

print("takes a qubit from a definte basis state into a superpostions if two states")
H = Operator([[1 / sqrt(2), 1 / sqrt(2)], [1 / sqrt(2), -1 / sqrt(2)]])
display("H ", H, "\n")

print("Checking some of the properties of the Unary Operators")
print("Testing the HXH = Z property:")
u = Statevector([(1 + 2.0j) / 3, -2 / 3])
display("Starting with this statevector u:", u,)
resultHXH = matmul(H ,  matmul(X ,  matmul(H , u)))
display('The result of H*X*H*u is: ', resultHXH)
resultZ = matmul(Z , u)
display('The result of Z*u is: ', resultZ, "\n")

print("Testing the HYH = -Y property:")
v = Statevector([1 / sqrt(2), 1 / sqrt(2)])
display("Starting with this statevector v:", v)
resultHYH = matmul(H ,  matmul(Y ,  matmul(H , v)))
display('The result of H*Y*H*v is: ', resultHYH)
resultZ = matmul((-1 * Y) , v)
display('The result of -Y*v is: ', resultZ, "\n")

print("Testing the H^2 = I property:")
resultH2 = Operator(matmul(H , H))
display("H*H ", resultH2)
print("*the two very small numbers are supposed to be zero and are representative of floating point errors\n")

print("Here's a simple example of a quantum circuit:")
print(" this circuit has 2 qubits and after measuring has an equal chance for 00, 01, 10, 11 outputs")
C = QuantumRegister(1, "C")
D = QuantumRegister(1, "D")
# measurement bits
A = ClassicalRegister(1, "A")
B = ClassicalRegister(1, "B")
circuit = QuantumCircuit(A, B, C, D)

circuit.h(C)
circuit.h(D)
circuit.cx(D, C)
# measurement gates
circuit.measure(D, B)
circuit.measure(C, A)

# Matplotlib drawing
display(circuit.draw())

results = Sampler().run(circuit).result()
statistics = results.quasi_dists[0].binary_probabilities()
display(statistics)
histogram = plot_histogram(statistics)
plt.show()
