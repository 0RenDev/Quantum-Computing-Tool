import pandas as pd
import plotly.express as px

# Load the Excel file.
file_path = 'ScatterPlot.xlsx'  # Replace with your Excel file path

#file_path = 'CircuitPerformance14QB50Gate50Iteration.xlsx'  # Replace with your Excel file path
df = pd.read_excel(file_path)
qubits_col = 'Qubits'
gates_col = 'Gates'
avg_time_col = 'Avg Time (ms)'

# Create 3D scatter plot using Plotly with smaller marker size.
fig = px.scatter_3d(
    df, 
    x=qubits_col, 
    y=gates_col, 
    z=avg_time_col, 
    title="3D Scatter Plot of Qubits, Gates, and Avg Time",
    labels={qubits_col: 'Qubits', gates_col: 'Gates', avg_time_col: 'Avg Time'},
    template='plotly_dark'
)

# Customize marker size.
fig.update_traces(marker=dict(size=4))
fig.show()
