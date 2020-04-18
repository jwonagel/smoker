import time
import RPi.GPIO as GPIO

# Pin definitions
input_0 = 18
input_1 = 23
input_2 = 24

# Suppress warnings
GPIO.setwarnings(False)

# Use "GPIO" pin numbering
GPIO.setmode(GPIO.BCM)

def callback_1(channel):
    print('Button pressed')
    print(channel)


# Set LED pin as output
GPIO.setup(input_0, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(input_1, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(input_2, GPIO.IN, pull_up_down=GPIO.PUD_UP)


GPIO.add_event_detect(input_0, GPIO.FALLING, callback=callback_1, bouncetime=300)
GPIO.add_event_detect(input_1, GPIO.FALLING, callback=callback_1, bouncetime=300)
GPIO.add_event_detect(input_2, GPIO.FALLING, callback=callback_1, bouncetime=300)


while True:
    time.sleep(1)