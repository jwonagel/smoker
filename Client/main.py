import libs.I2C_LCD_driver as lcd_driver
import libs.Temperatur as Temperatur
from libs.smoker_gpio import smoker_gpio
from time import sleep
from threading import Lock


gpio = smoker_gpio()
mylcd = lcd_driver.lcd()
mylcd.lcd_clear()
temp_sensors = Temperatur.temperatur_sensor()


lock = Lock()

def write_threadsave(value, line):
    with lock:
        mylcd.lcd_display_string(value, line)

def clear_threadsave():
    with lock:
        mylcd.lcd_clear()

def menu_function():
    print('Menu Pressed')
    write_threadsave('Menu pressed', 2)
    sleep(1)
    clear_threadsave()

def up_function():
    while gpio.is_up_pressed():
        gpio.open()
        sleep(0.1)

    gpio.stop_motor()

def down_function():
    while gpio.is_down_pressed():
        gpio.close()
        sleep(0.1)

    gpio.stop_motor()

gpio.register_menu_function(menu_function)
gpio.register_down_function(down_function)
gpio.register_up_function(up_function)

while True:
    values = []
    for i in range(10):
        temp2 = temp_sensors.get_temperatur(2)
        values.append(temp2)
    
    temp2 = sum(values) / 10
    write_threadsave("Temperatur: {:.2f}".format(temp2), 1)
    sleep(1)
    