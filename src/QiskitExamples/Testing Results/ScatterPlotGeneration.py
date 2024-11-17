import pandas as pd
import plotly.express as px

# Load the Excel file
file_path = 'ScatterPlot.xlsx'  # Replace with your Excel file path

#file_path = 'CircuitPerformance14QB50Gate50Iteration.xlsx'  # Replace with your Excel file path
df = pd.read_excel(file_path)

# Check the first few rows of the data to ensure it's loaded correctly
print(df.head())

# Ensure the columns are correctly named and the data is in the expected format
# Adjust these names if they differ in your file
qubits_col = 'Qubits'
gates_col = 'Gates'
avg_time_col = 'Avg Time (ms)'

# Create 3D scatter plot using Plotly with smaller marker size
fig = px.scatter_3d(
    df, 
    x=qubits_col, 
    y=gates_col, 
    z=avg_time_col, 
    title="3D Scatter Plot of Qubits, Gates, and Avg Time",
    labels={qubits_col: 'Qubits', gates_col: 'Gates', avg_time_col: 'Avg Time'},
    template='plotly_dark'  # Optional: Choose a template for aesthetics
)

# Customize marker size
fig.update_traces(marker=dict(size=4))  # Adjust size as needed (default is around 6-8)

# Show the plot
fig.show()
