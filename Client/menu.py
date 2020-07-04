import numpy as np
import time
from settings import Settings


class Main_menu:

    def __init__(self, write_function, clear_function, gpio, settings, open_close_state):
        self.menu_items = []
        self.menu_items.append(Temparatur_menu(write_function, clear_function))
        self.menu_items.append(Manuel_mode_menu(write_function, clear_function, settings, open_close_state))
        self.menu_items.append(Auto_mode_menu(write_function, clear_function, gpio, settings, open_close_state))
        self.menu_items.append(Info_menu(write_function, clear_function, settings))

        self.gpio = gpio
        self.current_item = 0
        self.__activate_item()
        self.__mode = False
        gpio.register_menu_function(self.switch_menu)

    def get_temp_menu(self):
        return self.menu_items[0]

    def __activate_item(self):
        menu_item = self.menu_items[self.current_item]
        self.gpio.register_down_function(menu_item.on_down_pressed)
        self.gpio.register_up_function(menu_item.on_up_pressed)
        menu_item.activate()

    
    def switch_menu(self):
        self.menu_items[self.current_item].deactivate()
        self.current_item = (self.current_item + 1) % len(self.menu_items)
        self.__activate_item()    





class Abstract_menu_item:
    def __init__(self, write_function, clear_function):
        self.write_function = write_function
        self.clear_function = clear_function
        self.is_active = False

    def activate(self):
        self.is_active = True
        self.clear_function()

    def deactivate(self):
        self.is_active = False

    def on_up_pressed(self):
        pass

    def on_down_pressed(self):
        pass


class Info_menu(Abstract_menu_item):

    def __init__(self, write_function, clear_function, settings):
        super().__init__(write_function, clear_function)
        self.settings = settings
        self.pos = 0

    
    def activate(self):
        super().activate()
        self.pos = 0
        self.write_function('Info Menu')
        self.write_function('up/down for info', 2)


    def on_up_pressed(self):
        pass

    def on_down_pressed(self):
        pass



class Auto_mode_menu(Abstract_menu_item):

    def __init__(self, write_function, clear_function, gpio, settings, open_close_state):
        super().__init__(write_function, clear_function)
        self.settings = settings
        self.open_close_state = open_close_state
        self.gpio = gpio
        
    def activate(self):
        super().activate()
        self.write_function('Change temp.')
        self.__write_current_value()

    def __write_current_value(self):
        self.write_function('Treshold {}'.format(self.settings.get_settings()['openCloseTreshold'], 2))

    def on_up_pressed(self):
        self.open_close_state.set_auto_mode()
        while self.gpio.is_up_pressed() and self.settings.get_settings()['openCloseTreshold'] < 300:
            self.settings.get_settings()['openCloseTreshold'] += 1
            self.__write_current_value()
            time.sleep(0.2)

    def on_down_pressed(self):
        self.open_close_state.set_auto_mode()
        while self.gpio.is_down_pressed() and self.settings.get_settings()['openCloseTreshold'] > 20:
            self.settings.get_settings()['openCloseTreshold'] -= 1
            self.__write_current_value()
            time.sleep(0.2)


class Manuel_mode_menu(Abstract_menu_item):
    def __init__(self, write_function, clear_function, settings, open_close_state):
        super().__init__(write_function, clear_function)
        self.settings = settings
        self.open_close_state = open_close_state
    
    def on_up_pressed(self):
        self.open_close_state.is_auto_mode = False
        self.clear_function()
        self.write_function('Opening')
        self.open_close_state.handle_open()
        self.activate()

    def on_down_pressed(self):
        self.settings.is_auto_mode = False
        self.clear_function()
        self.write_function('Closing')
        self.open_close_state.handle_close()
        self.activate()

    def activate(self):
        super().activate()
        self.write_function('Activate manual', 1)
        self.write_function('up to open...', 2)



def create_manual_menu(write_function, clear_function, gpio):
    return Manuel_mode(write_function, )

class Temparatur_menu(Abstract_menu_item):
    def __init__(self, write_function, clear_function):
        super().__init__(write_function, clear_function)
        self.temperaturs = np.zeros(6)
        self.state = 0
        self.states = 3
    
    def on_up_pressed(self):
        self.state += 1
        self.state %= self.states
        self._update_display()

    def activate(self):
        super().activate()
        self._update_display()

    def on_down_pressed(self):
        self.state -= 1
        self.state %= self.states
        self._update_display()


    def update_temperaturs(self, temperaurs):
        self.temperaturs = temperaurs
        if self.is_active:
            self._update_display()

    def _update_display(self):
        if self.state == 0:
            line0 = 'Feuer: {0}'.format(self._get_temp_value(0))            
            line1 = 'Innen: {0}'.format(self._get_temp_value(1))
        if self.state == 1:
            line0 = 'Sensor 1: {0}'.format(self._get_temp_value(2))            
            line1 = 'Sensor 2: {0}'.format(self._get_temp_value(3)) 
        if self.state == 2:
            line0 = 'Sensor 3: {0}'.format(self._get_temp_value(4))            
            line1 = 'Sensor 4: {0}'.format(self._get_temp_value(5)) 

        self.write_function(line0, 1)
        self.write_function(line1, 2)


    def _get_temp_value(self, index):
        temp = self.temperaturs[index]
        if np.isnan(temp):
            return 'X'
        else:
            return '{:.1f}C'.format(temp)
    