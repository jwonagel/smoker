
# Importing modules
import spidev # To communicate with SPI devices
import time
import RPi.GPIO as GPIO	# To use GPIO pins
from scipy import interpolate
import numpy as np


class temperatur_sensor:

    def __init__(self, treshold_non_connect=940, resistor_ptc100=220, pt100_coefficient=0.39):
        self.treshold_non_connect=treshold_non_connect
        self.resistor_ptc100 = resistor_ptc100
        self.pt100_coefficient = pt100_coefficient
        data = np.genfromtxt('/home/pi/smoker/Client/Resources/temp_data.csv', 
                      dtype=np.float32, 
                      delimiter=';', 
                      skip_header=1) 


        y = data[:,0]
        x = data[:,1]
        self.tck = interpolate.splrep(x, y)

        # Start SPI connection
        self.spi = spidev.SpiDev() # Created an object
        self.spi.open(0,0)


    # Read MCP3008 data
    def _analogInput(self, channel):
        self.spi.max_speed_hz = 1350000
        adc = self.spi.xfer2([1,(8+channel)<<4,0])
        data = ((adc[1]&3) << 8) + adc[2]
        return data


    def _calc_ptc_temp(self, value):
        ratio = value / 1024
        resistance = self.resistor_ptc100 * ratio / (1 - ratio)
        return (resistance - 100) * self.pt100_coefficient

    def _calc_prope_temp(self, value):
        return interpolate.splev([value], self.tck)[0]


    def get_temperatur(self, channel):
        value = self._analogInput(channel)
        if channel > 1:
            return self._calc_prope_temp(value)
        else:
            return self._calc_ptc_temp(value)


    def get_temperatur_of_all_channels(self, channel_to=6):
        values = np.full(channel_to, np.NAN, dtype=np.float)
        for i in range(channel_to):
            value = self._analogInput(i)
            if self.treshold_non_connect < value:
                values[i] = 0.0
            elif i < 2:
                values[i] = self._calc_ptc_temp(value)
            else:
                values[i] = self._calc_prope_temp(value)
        
        return values
    
    def get_inputs(self, channel_to=6):
        values = np.full(channel_to, False, dtype=np.bool)
        for i in range(channel_to):
            value = self._analogInput(i)
            if value < 1010:
                values[i] = True

        return values


if __name__ == "__main__":
    tmp = temperatur_sensor()

    while True:
        start = time.time()
        value = tmp.get_temperatur_of_all_channels() # Reading from CH0
        end = time.time()
        print('Temp: {0}, duration {1}'.format(value, end - start))
        time.sleep(1)

