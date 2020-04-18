import time
import RPi.GPIO as GPIO

# Pin definitions
output_0 = 20
output_1 = 21

# Suppress warnings
GPIO.setwarnings(False)

# Use "GPIO" pin numbering
GPIO.setmode(GPIO.BCM)

# Set LED pin as output
GPIO.setup(output_0, GPIO.OUT)
GPIO.setup(output_1, GPIO.OUT)


# Blink forever
while True:
    GPIO.output(output_0, GPIO.LOW) # Turn LED on
    GPIO.output(output_1, GPIO.HIGH) # Turn LED on
    time.sleep(5)                   # Delay for 1 second
    # GPIO.output(output_0, GPIO.LOW)  # Turn LED off
    # GPIO.output(output_1, GPIO.LOW) # Turn LED on
    # time.sleep(5)                   # Delay for 1 second
