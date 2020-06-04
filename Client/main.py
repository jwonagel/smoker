import libs.I2C_LCD_driver as lcd_driver
import libs.Temperatur as Temperatur
from libs.smoker_gpio import smoker_gpio
from time import sleep
import api 
import datetime
from menu import Main_menu
import settings as sett
import swagger_client as smoker_api

gpio = smoker_gpio()
mylcd = lcd_driver.lcd()
mylcd.lcd_clear()
temp_sensors = Temperatur.temperatur_sensor()



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


def create_temperatur_menu():
    m = menu.temparatur_menu(mylcd.lcd_display_string_thread_save, 
                mylcd.lcd_clear)
    
    m.activate()
    return m

# temp_menu = create_temperatur_menu()

# #gpio.register_menu_function(menu_function)
# gpio.register_down_function(temp_menu.on_down_pressed)
# gpio.register_up_function(temp_menu.on_up_pressed)

cnt = 0

settings = sett.read_settings()

main_menu = Main_menu(mylcd.lcd_display_string_thread_save, mylcd.lcd_clear, gpio, settings)
temp_menu = main_menu.get_temp_menu()


api_instance = api.api()

def map_temperaturs_to_measuremnt(temperaturs):
    measurement = smoker_api.MeasurementSmoker(
        fire_sensor=temperaturs[0],
        content_sensor=temperaturs[1],
        sensor1=temperaturs[2],
        sensor2=temperaturs[3],
        sensor3=temperaturs[4],
        sensor4=temperaturs[5]
    )



while True:
    values = []
    temp = temp_sensors.get_temperatur_of_all_channels()
    temp_menu.update_temperaturs(temp)

    sleep(3)

    cnt += 1
    if cnt % 20 == 0:
        api_measurement = map_temperaturs_to_measuremnt(temp)
        api_instance.post_measurement(api_measurement)
    