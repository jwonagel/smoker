import I2C_LCD_driver
from time import sleep
from datetime import datetime

mylcd = I2C_LCD_driver.lcd()

mylcd.lcd_clear()
mylcd.lcd_display_string("Hey Schatz", 1)
mylcd.lcd_display_string('i lieb di <3', 2)


fontdata1 = [      
        [ 0b01010, 
          0b11111,
          0b11111, 
          0b01110, 
          0b01110, 
          0b00100, 
          0b00000, 
          0b00000 ],
]


mylcd.lcd_load_custom_chars(fontdata1)
mylcd.lcd_write(0x80)
mylcd.lcd_write_char(0)