import time
import RPi.GPIO as GPIO


class smoker_gpio:

    def __init__(self, open_pin=4, close_pin=5, menu_pin=18, up_pin=23, down_pin=24):
        self.__open_pin = open_pin
        self.close_pin = close_pin
        self.menu_pin = menu_pin
        self.up_pin = up_pin
        self.down_pin = down_pin

        self.is_opening = False
        self.is_closing = False

        # Suppress warnings
        GPIO.setwarnings(False)
        # Use "GPIO" pin numbering
        GPIO.setmode(GPIO.BCM)

        # Set Relays pin as output
        GPIO.setup(self.__open_pin, GPIO.OUT, initial=GPIO.HIGH)
        GPIO.setup(self.close_pin, GPIO.OUT, initial=GPIO.HIGH)

        # Setup input pins for menu
        GPIO.setup(self.menu_pin, GPIO.IN, pull_up_down=GPIO.PUD_UP)
        GPIO.setup(self.up_pin, GPIO.IN, pull_up_down=GPIO.PUD_UP)
        GPIO.setup(self.down_pin, GPIO.IN, pull_up_down=GPIO.PUD_UP)

        self.up = None
        self.down = None
        self.menu = None

        GPIO.add_event_detect(self.menu_pin, GPIO.FALLING, callback=self.handle_button_event, bouncetime=300)
        GPIO.add_event_detect(self.up_pin, GPIO.FALLING, callback=self.handle_button_event, bouncetime=300)
        GPIO.add_event_detect(self.down_pin, GPIO.FALLING, callback=self.handle_button_event, bouncetime=300)


    def handle_button_event(self, channel):
        if channel == self.menu_pin and self.menu is not None:
            self.menu()

        if channel == self.up_pin and self.up is not None:
            self.up()

        if channel == self.down_pin and self.down is not None:
            self.down()

    
    def register_menu_function(self, function):
        self.menu = function

    def register_up_function(self, function):
        self.up = function

    def register_down_function(self, function):
        self.down = function    

    def stop_motor(self):
        GPIO.output(self.close_pin, GPIO.HIGH) # Stop 
        GPIO.output(self.__open_pin, GPIO.HIGH) 


    def is_up_pressed(self):
        return GPIO.input(self.up_pin) == False

    def is_down_pressed(self):
        return GPIO.input(self.down_pin) == False

    def open(self):
        if self.is_closing: # stop anything
            self.stop_motor()
        else:
            GPIO.output(self.__open_pin, GPIO.LOW) # Turn LED on


    def close(self):
        if self.is_opening:
            self.stop_motor()
        else:
            GPIO.output(self.close_pin, GPIO.LOW)


