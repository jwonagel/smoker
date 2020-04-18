from scipy import interpolate
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

ds = pd.read_csv('Client/TestADConverter/results.csv', sep=';')
y = ds['Temperatur'].to_numpy()
x = ds['Value'].to_numpy()
 
# x = np.linspace(0, 10, num=11, endpoint=True)
# y = np.cos(-x**2/9.0)

tck = interpolate.splrep(x, y)

print(interpolate.splev(800, tck))