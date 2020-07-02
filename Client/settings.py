import datetime
import json

import datetime
import json

class Settings:

    def __init__(self):
        self.settings = {}


    def update_settings(self, settings):
        self.settings = settings

    def get_settings(self):
        return self.settings     



path='/home/pi/smokerdata/settingsdata.json'

def default(o):
    if isinstance(o, (datetime.date, datetime.datetime)):
        return o.isoformat()

def write_settings(settings):
    data = json.dumps(settings.get_settings(), default=default)
    with open(path, mode='w') as file:
        file.write(data)


def read_settings():
    str = ''
    with open(path, mode='r') as file:
        str = file.read()
    return json.loads(str)
