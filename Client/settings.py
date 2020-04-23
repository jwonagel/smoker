import datetime
import json

class Settings:

    def __init__(self, 
                 open_close_treshold, 
                 is_auto_mode, 
                 fire_notification_temperatur, 
                 temperatur_update_cycle_seconds, 
                 last_settings_update, 
                 last_settings_update_user,
                 alerts):
        self.open_close_treshold = open_close_treshold
        self.is_auto_mode = is_auto_mode
        self.fire_notification_temperatur = fire_notification_temperatur
        self.temperatur_update_cycle_seconds = temperatur_update_cycle_seconds
        self.last_settings_update =  datetime.datetime.fromisoformat(last_settings_update)
        self.last_settings_update_user = last_settings_update_user
        self.alerts = alerts       



path='/home/pi/smokerdata/settingsdata.json'

def default(o):
    if isinstance(o, (datetime.date, datetime.datetime)):
        return o.isoformat()

def write_settings(settings):
    data = json.dumps(settings.__dict__, default=default)
    with open(path, mode='w') as file:
        file.write(data)


def read_settings():
    filedata = None
    with open(path, mode='r') as file:
        filedata = file.read()
    jdata = json.loads(filedata)
    settings = Settings(**jdata)
    return settings
