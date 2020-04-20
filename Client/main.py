import libs.I2C_LCD_driver as lcd_driver
import libs.Temperatur as Temperatur
from libs.smoker_gpio import smoker_gpio
from time import sleep
import swagger_client
from swagger_client.rest import ApiException
import datetime
from menu import Main_menu

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


main_menu = Main_menu(mylcd.lcd_display_string_thread_save, mylcd.lcd_clear, gpio)
temp_menu = main_menu.get_temp_menu()

configuration = swagger_client.Configuration()
configuration.host = '192.168.140.228:9080'
api_instance = swagger_client.SmokerApi(swagger_client.ApiClient(configuration))

def send_temp(temp):
    body = swagger_client.MeasurementSmoker()
    body.sensor2 = temp
    body.time_stamp = datetime.datetime.now().isoformat()

    try:
        api_response = api_instance.smoker_post(body=body)
        print(api_response)
    except ApiException as e:
        print('Exception while SmokerApi -> smokder_post: $s\n' % e)
    except Exception as e:
        print(e)



while True:
    values = []
    temp2 = temp_sensors.get_temperatur_of_all_channels()
    temp_menu.update_temperaturs(temp2)
    sleep(3)

    cnt += 1
    if cnt % 20 == 0:
        send_temp(temp2[2])
    