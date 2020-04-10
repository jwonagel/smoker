import matplotlib.pyplot as plt
import pandas as pd
import seaborn as sns


ds = pd.read_csv('Client/TestADConverter/results.csv', sep=';')


ds.set_index('Temperatur')['Value'].plot()

plt.show()

input('Press any key...')