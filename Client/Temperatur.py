
# Importing modules
import spidev # To communicate with SPI devices
from numpy import interp	# To scale values
from time import sleep	# To add delay
import RPi.GPIO as GPIO	# To use GPIO pins
from scipy import interpolate
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

data = np.genfromtxt('Client/Resources/temp_data.csv', dtype=np.float32, delimiter=';', skip_header=1) 

y = data[0]
x = data[1]
tck = interpolate.splrep(x, y)
# x = np.linspace(0, 10, num=11, endpoint=True)
# y = np.cos(-x**2/9.0)







# Start SPI connection
spi = spidev.SpiDev() # Created an object
spi.open(0,0)




# Read MCP3008 data
def _analogInput(channel):
    spi.max_speed_hz = 1350000
    adc = spi.xfer2([1,(8+channel)<<4,0])
    data = ((adc[1]&3) << 8) + adc[2]
    return data


def get_temperatur(channel):
    value = _analogInput(1)
    return interpolate.splev(800, tck)


if __name__ == "__main__":
    while True:
        value = get_temperatur(1) # Reading from CH0
        print(value)
        sleep(1)
