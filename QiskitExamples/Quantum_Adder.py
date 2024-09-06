from IPython.display import display
from qiskit import QuantumCircuit, QuantumRegister, ClassicalRegister
from qiskit.primitives import Sampler
from qiskit.quantum_info import Operator
from qiskit.visualization import plot_histogram
from qiskit.visualization import plot_bloch_multivector
from qiskit.quantum_info import Statevector
from numpy import sqrt
from numpy import array
from numpy import matmul
import matplotlib.pyplot as plt

# create names for bits for better understand
A_in = QuantumRegister(1, name='A_in') # input A
B_in = QuantumRegister(1, name='B_in') # input B
C_in = QuantumRegister(1, name='C_in') # carry in
Out = QuantumRegister(1, name='Out') # carry in

# intermediate steps of full adder
AND_1 = QuantumRegister(1, name='AND_1') 
AND_2 = QuantumRegister(1, name='AND_2')
OR_1 = QuantumRegister(1, name='OR_1')

C_out = ClassicalRegister(1, name ='C_out') # carry out
S_out = ClassicalRegister(1, name ='S_out') # Output

# XOR gate
# Creating a circuit with 2 qbits and one cbit
XOR = QuantumCircuit(A_in, B_in, S_out)

# Inputs : comment out to put input to 0
XOR.x(0) # flips A_in to 1
XOR.x(1) # flips B_in to 1

# Used for visual seperation, shouldn't affect function
XOR.barrier()

# Applying the CNOT gate
XOR.cx(0,1)
XOR.barrier()

# Measuring qbit1 and put result to cbit
XOR.measure(1,0)

print("\n\n\n\nXOR gate")
# Drawing the circuit diagram
display(XOR.draw())

# Gets probability of measurements
results = Sampler().run(XOR).result()
statistics = results.quasi_dists[0].binary_probabilities()
display(statistics)



#AND gate
# Creating a circuit with 3 qbits and one cbit
AND = QuantumCircuit(A_in, B_in, Out, S_out)

# Inputs: comment out to put input to 0
AND.x(0) # flips A_in to 1
AND.x(1) # flips B_in to 1
# no changes to C_in should always initialize to zero

AND.barrier()

# Applying the CCNOT gate
AND.ccx(0,1,2)

# used for visual seperation, shouldn't affect function
AND.barrier()

# Measuring qbit2 and put result to cbit
AND.measure(2,0)

print("\n\n\n\nAND gate")
# Drawing the circuit diagram
display(AND.draw())

results = Sampler().run(AND).result()
statistics = results.quasi_dists[0].binary_probabilities()
display(statistics)




# Half Adder
# Creating a circuit with 3 qubits and 2 cbits
HalfAdder = QuantumCircuit(A_in, B_in, Out, C_out, S_out)

# Inputs: comment out to put input to 0
HalfAdder.x(0) # flips A_in to 1
HalfAdder.x(1) # flips B_in to 1
# qbit2 should stay 0

HalfAdder.barrier()

# Applying AND operation and put result to qbit2
HalfAdder.ccx(0,1,2)
HalfAdder.barrier()

# Applying XOR operation and put result to qbit1
HalfAdder.cx(0,1)
HalfAdder.barrier()

# Reading outputs
HalfAdder.measure(1,0) # Reading XOR value ( sum bit )
HalfAdder.measure(2,1) # Reading AND value ( carry-out bit )


print("\n\n\n\nHalf Adder")

# Drawing the circuit diagram
display(HalfAdder.draw())

results = Sampler().run(HalfAdder).result()
statistics = results.quasi_dists[0].binary_probabilities()
display(statistics)



# OR gate: # this idea for a OR gate comes from Lahiru Madushanka's blogpost "Quantum Half-adder and Full-adder"
# found here https://lahirumadushankablog.wordpress.com/2020/02/04/quantum-half-adder-and-full-adder/
# Creating a circuit with 3 qbits and 1 cbit
OR = QuantumCircuit(A_in, B_in, Out, S_out)

# Inputs: comment out to put input to 0
OR.x(0) # flips A_in to 1
OR.x(1) # flips B_in to 1
# no changes to C_in should always be 0

OR.barrier()

# OR gate implementation
# Adding NOT opration to inputs of AND gate
OR.x(0) 
OR.x(1)
# AND gate
OR.ccx(0,1,2)
# Adding NOT operation to output of AND gate
OR.x(2)
OR.barrier()

# Measuring C_in and put result to classical bit
OR.measure(2,0)

print("\n\n\n\nOR gate")

# Drawing the circuit diagram
display(OR.draw())

results = Sampler().run(OR).result()
statistics = results.quasi_dists[0].binary_probabilities()
display(statistics)



# full adder
FullAdder = QuantumCircuit(A_in, B_in, C_in, AND_1, AND_2, OR_1, C_out, S_out)

# Inputs: comment out to put input to 0
#FullAdder.x(0) # flips A_in to 1
FullAdder.x(1) # flips B_in to 1
FullAdder.x(2) # flips C_in to 1

FullAdder.barrier()

# AND_1 gate
FullAdder.ccx(0,1,3)

FullAdder.barrier()

#XOR_1 stores into B_in
FullAdder.cx(0,1)

# AND_2 gate
FullAdder.ccx(1,2,4)

FullAdder.barrier()

#XOR_2 stores into C_in
FullAdder.cx(1,2)

FullAdder.barrier()

#OR gate
FullAdder.x(3) 
FullAdder.x(4)
FullAdder.ccx(3,4,5)
FullAdder.x(5)

FullAdder.barrier()

# Measuring C_in and put result to S_out
FullAdder.measure(2,1)

# Measuring OR_1 and put result to C_out
FullAdder.measure(5,0)

print("\n\nHalf Adder")

# Drawing the circuit diagram
display(FullAdder.draw())

results = Sampler().run(FullAdder).result()
statistics = results.quasi_dists[0].binary_probabilities()
display(statistics)











def fulladdertest(A,B,Cin):
    # full adder
    FullAdder = QuantumCircuit(A_in, B_in, C_in, AND_1, AND_2, OR_1, C_out, S_out)

    if A:
        FullAdder.x(0) # flips A_in to 1
    if B:
        FullAdder.x(1) # flips B_in to 1
    if Cin:
        FullAdder.x(2) # flips C_in to 1

    FullAdder.barrier()

    # AND_1 gate
    FullAdder.ccx(0,1,3)

    FullAdder.barrier()

    #XOR_1 stores into B_in
    FullAdder.cx(0,1)

    # AND_2 gate
    FullAdder.ccx(1,2,4)

    FullAdder.barrier()

    #XOR_2 stores into C_in
    FullAdder.cx(1,2)

    FullAdder.barrier()

    #OR gate
    FullAdder.x(3) 
    FullAdder.x(4)
    FullAdder.ccx(3,4,5)
    FullAdder.x(5)

    FullAdder.barrier()

    # Measuring C_in and put result to S_out
    FullAdder.measure(2,1)

    # Measuring OR_1 and put result to C_out
    FullAdder.measure(5,0)

    results = Sampler().run(FullAdder).result()
    statistics = results.quasi_dists[0].binary_probabilities()
    print(A, B, Cin, " : ", statistics)


def FullAdderValues():
    print("\n\n\n\nTruth Table Test:")
    bool_values = [True, False]
    
    for a in bool_values:
        for b in bool_values:
            for c in bool_values:
                fulladdertest(a, b, c)


FullAdderValues()








