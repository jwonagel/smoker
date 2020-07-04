import libs.I2C_LCD_driver as lcd_driver
import libs.Temperatur as Temperatur
from libs.smoker_gpio import smoker_gpio
import time 
import api 
import datetime
from menu import Main_menu
import settings as sett
import uuid

hysteresis_value_content = 12
open_close_factor = 10
step_ratio = 0.1

gpio = smoker_gpio()
mylcd = lcd_driver.lcd()
mylcd.lcd_clear()
temp_sensors = Temperatur.temperatur_sensor()



def up_function():
    while gpio.is_up_pressed():
        gpio.open()
        time.sleep(0.1)

    gpio.stop_motor()

def down_function():
    while gpio.is_down_pressed():
        gpio.close()
        time.sleep(0.1)

    gpio.stop_motor()


def create_temperatur_menu():
    m = menu.temparatur_menu(mylcd.lcd_display_string_thread_save, 
                mylcd.lcd_clear)
    
    m.activate()
    return m

class OpenCloseState:

    def __init__(self, factor, gpio):
        self.open_close_ratio = 0.0
        self.is_auto_mode = True
        self.factor = factor
        self.gpio = gpio
    
    def set_auto_mode(self):
        self.is_auto_mode = True

    def set_manual_mode(self):
        self.is_auto_mode = False

    def _get_duration(self, ratio):
        return ratio * self.factor

    def _get_factor(self, duration):
        return duration / self.factor

    def set_ratio(self, ratio):
        if self.open_close_ratio > ratio:
            self.close_ratio(self.open_close_ratio - ratio)
        elif self.open_close_ratio < ratio:
            self.open_ratio(ratio - self.open_close_ratio)


    def open_ratio(self, ratio):
        if (ratio + self.open_close_ratio > 1.0):
            ratio = 1.0 - self.open_close_ratio
                              
        duration = self._get_duration(ratio)
        if duration < 0.05:
            return
        self.gpio.open()
        time.sleep(duration)
        self.gpio.stop_motor()
        self.open_close_ratio += ratio

    def close_ratio(self, ratio):
        if (self.open_close_ratio - ratio < 0):
            ratio = self.open_close_ratio
                              
        duration = self._get_duration(ratio)
        if duration < 0.05:
            return
        self.gpio.close()
        time.sleep(duration)
        self.gpio.stop_motor()
        self.open_close_ratio -= ratio


    def handle_close(self):
        self.is_auto_mode = False
        now = time.time()
        while self.gpio.is_down_pressed():
            self.gpio.close()
            time.sleep(0.1)
        self.gpio.stop_motor()
        duration = time.time() - now    
          
        self.open_close_ratio -= self._get_factor(duration) 
        if (self.open_close_ratio < 0):
            self.open_close_ratio = 0

    def handle_open(self):
        self.is_auto_mode = False
        now = time.time()
        while self.gpio.is_up_pressed():
            self.gpio.open()
            time.sleep(0.1)
        self.gpio.stop_motor()            
        duration = time.time() - now      
        self.open_close_ratio += self._get_factor(duration) 
        


cnt = 0

open_close_state = OpenCloseState(open_close_factor, gpio)
api_instance = api.api()
settings = sett.Settings()

def load_settings(api_instance, settings):
    try:
        set = api_instance.get_settings()
        if set is None:
            set = read_settings()
        else:
            sett.write_settings(settings)
        settings.update_settings(set)
        return
    except Exception as e:
        print('Error while loading settings')
        print(e)
    
    set = sett.read_settings()
    settings.update_settings(set)
    



load_settings(api_instance, settings)

main_menu = Main_menu(mylcd.lcd_display_string_thread_save, mylcd.lcd_clear, gpio, settings, open_close_state)
temp_menu = main_menu.get_temp_menu()

# gpio.register_menu_function(menu_function)
# gpio.register_down_function(temp_menu.on_down_pressed)
# gpio.register_up_function(temp_menu.on_up_pressed)


def map_temperaturs_to_measuremnt(temperaturs, open_close_state, settings):
    measurement = {
        "measurementId": str(uuid.uuid4()),
        "fireSensor": temperaturs[0],
        "contentSensor": temperaturs[1],
        "sensor1": temperaturs[2],
        "sensor2": temperaturs[3],
        "sensor3": temperaturs[4],
        "sensor4": temperaturs[5],
        "openCloseState": open_close_state.open_close_ratio,
        "isAutoMode": open_close_state.is_auto_mode,
        "openCloseTreshold": settings.get_settings()['openCloseTreshold']
    }
    return measurement

## connect
def receive_message(values):
    print(values)
    if values is not None and len(values) == 2 and values[0] == 'Settings' and values[1] == 'Update':
        load_settings(api_instance, settings)


def open_close_update(open_close_model, oc_state):
    print(open_close_model)
    model = open_close_model[0]
    if not model['isAutoMode']:
        oc_state.set_manual_mode()
        oc_state.set_ratio(model['openCloseState'])
    else:
        oc_state.set_auto_mode()

def connect_signal_r(api_instance):
    api_instance.connect_signal_r(receive_message, lambda x : open_close_update(x, open_close_state))


def handle_measurment():
    total_sleep_time = settings.get_settings()['temperaturUpdateCycleSeconds']
    five_sec_steps = total_sleep_time // 5
    
    for i in range(five_sec_steps):
        temp = temp_sensors.get_temperatur_of_all_channels()
        temp_menu.update_temperaturs(temp)
        if i == 0: 
            api_measurement = map_temperaturs_to_measuremnt(temp, open_close_state, settings)
            api_instance.post_measurement(api_measurement)
        current_temp = api_measurement['contentSensor']
        if settings.get_settings()['isAutoMode'] and open_close_state.is_auto_mode:
            open_close_treshold = settings.get_settings()['openCloseTreshold']
            if current_temp - hysteresis_value_content / 2 > open_close_treshold:
                open_close_state.open_ratio(step_ratio)
            elif current_temp + hysteresis_value_content / 2 < open_close_treshold:
                open_close_state.close_ratio(step_ratio)
        
        time.sleep(4.9)
    time.sleep(total_sleep_time % 5)



while True:
    try:
        connect_signal_r(api_instance)
    except Exception as e:
        print(e)
        print('Retry in 10 sec')
        time.sleep(10)
        continue


    while True:
        handle_measurment()



