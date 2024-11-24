# ASU Quantum Computing Tool for Education

This is the repository for the ASU Quantum Computing Tool for Education, developed as part of the CSE 423/485 Capstone classes for Spring '24 and Fall '24 semesters. This tool emulates a quantum computer, built in C#, allowing users to simulate quantum gates, circuits, and algorithms.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Documentation](#documentation)
- [Contributing](#contributing)

---

## Overview

The ASU Quantum Computing Tool is designed for:
- **Educational Use**: Developed to be integrated into VIPLE, this tool will be used to help introduce beginners to fundamental quantum computing concepts like gates, circuits, and eventually, algorithms.

The project consists of four main branches:
1. **Quantum Computing**: Core quantum computing logic, including gates, circuit builders, and circuit execution.
2. **Linear Algebra**: A custom built linear-algebra library implementing all necessary operations and functionalities.
3. **Test Cases**: Unit tests to confirm the validity of our implemented methods.
4. **Console App**: Demo applications showcasing how to use the library and various circuits like full adders.

---

## Features

- **Quantum Gate Emulation**: Supports standard gates like Hadamard, Pauli-Gates, CX, TOF, Rotation Gates, etc.
- **State Visualization**: View qubit states as the pure statevector, a probability distribution, a bitstring histogram representation, and others.
- **Qiskit-Inspired Examples**: Built to closely relate to qiskit functionally, including python-esque method overloading.
- **Built-In Testing**: Includes pre-written test cases for verifying results.
- **Extensible Design**: Easily add new gates or modify circuit behavior.

---

## Getting Started

### Prerequisites

To build and run this project, you will need:
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the .NET desktop development workload.
- .NET Framework 4.8 (installed by default with Visual Studio 2022).
- (Optional) [DocFX](https://github.com/dotnet/docfx) for generating documentation (details in Documentation section).

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/0RenDev/Quantum-Computing-Tool.git
   cd Quantum-Computing-Tool
   ```
2. Open LinearAlgebra.sln in Visual Studio.
3. Build the solution.

## Usage

Here's a simple example of how to build and execute a quantum circuit using our tool:
```
using QuantumComputing.LinearAlgebra;
using QuantumComputing.QuantumCircuits;

public class QuantumCircuitDemo
{
    public void TestCircuit()
    {
        // circuit has 2 quantum lines and 0 classical lines
        QuantumCircuitBuilder qc = new QuantumCircuitBuilder(2, 0);

        // apply an H gate to qubit 0, CX with control 0 on target 1, bell state
        qc.AddGateH(0);
        qc.AddGateCX(0, 1);

        // print the visual representation of the circuit
        Console.WriteLine(qc.ToString());

        // execute the circuit and get the result
        CircuitExecution exe = new CircuitExecution(qc);
        QuantumComputing.LinearAlgebra.Vector result = exe.ExecuteCircuit();

        // print the resulting quantum state
        Console.WriteLine(result.ToString());

        // print expectation values for inputted observables (see documentation for more use cases)
        exe.PrintExpectationValues(["IZ", "IX", "ZI", "XI", "ZZ", "XX"]);
    }
}
```

## Documentation

This project includes comprehensive documentation to help you use/understnad the ASU Quantum Computing Tool for Education. The documentation provides:
- **API References**: Details about all classes, methods, and supported operations.
- **Usage Cases**: Input and output parameters for all implemented methods.
- **Design Philosophy**: Thought process behind each method, going over topics like the reasoning for a method's structure.

### Building and Accessing the Documentation

To build and view the documentation locally:

1. Install [DocFX](https://github.com/dotnet/docfx) if it’s not already installed from earlier.
2. Open a terminal and navigate to the root directory of the repository.
3. Run the following commands:
   ```bash
   dotnet clean
   dotnet build
   docfx build
   docfx ./docfx.json --serve
   ```
4. Then visit the [Docs](https://localhost:8080)

## Contributing

This project was developed by the CSE423/485 Capstone Team from Spring and Fall 2024:
- **Jett Bauman**
- **Zachary Litwin**
- **Sean Lowe**
- **Ryan Rajesh**
- **Griffith Simmon**

Directed by the guidance of our brilliant sponsors:
- **Dr. Gennaro De Luca**
- **Dr. Yinong Chen**
