import I2C_LCD_driver
from time import sleep
from datetime import datetime

mylcd = I2C_LCD_driver.lcd()

mylcd.lcd_clear()
mylcd.lcd_display_string("Hello World!", 1)

mylcd.backlight(0)

sleep(1)

mylcd.backlight(1)

while True:
    mylcd.lcd_display_string("Time: %s" %datetime.now().strftime("%H:%M:%S"), 1)
    mylcd.lcd_display_string("Date: %s" %datetime.now().strftime("%d.%m.%Y"), 2)
    sleep(0.1)