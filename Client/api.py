import datetime
import requests, json
import time
import os
from signalrcore.hub_connection_builder import HubConnectionBuilder
import uuid

authority_host = os.getenv('Authority')
secret = os.getenv('ClientSecret')
api_host = os.getenv('api_host')
verify_ssl = json.loads(os.getenv('verify_ssl', 'True').lower())

class api:

    def __init__(self):
        self.token_url = authority_host + '/connect/token'
        self.client_secret = secret
        self.expiration_time = time.time()
        self.access_token = ''

    def post_measurement(self, measurement):
        headers = self._get_headers()
        measurement['timeStamp'] = datetime.datetime.now().isoformat()
        url = api_host + '/Smoker'
        mdata = json.dumps(measurement)
        try:
            response = requests.post(url, headers=headers, data=mdata)
            return response.status_code == 200
        except Exception as e:
            print(e)

        return False

    def check_and_update_accesstoken(self):
        """Checks for valid token or updates"""
        current_time = time.time()
        if current_time + 30 >= self.expiration_time:
            try:
                token_type, access_token, expires_in = self._get_access_token()
                self.access_token = access_token
                self.expiration_time = current_time + expires_in
            except Exception as e:
                print(e)
                raise e

    def get_or_update_access_token(self):
        self.check_and_update_accesstoken()
        return self.access_token

    def _get_access_token(self):
        scopes = ['smokerapi']
        client_id = 'smoker'
        data = {'grant_type': 'client_credentials'}
        try:
            access_token_response = requests.post(self.token_url, data=data, verify=verify_ssl, allow_redirects=False, auth=(client_id, self.client_secret))
        except Exception as e:
            print('error while requestining accesstoken', e)
            raise e
        
        tokens = json.loads(access_token_response.text)
        return tokens['token_type'], tokens['access_token'], tokens['expires_in']

    def _get_headers(self):
        token = self.get_or_update_access_token()
        return {
            'content-type': 'application/json',
            'Authorization': 'Bearer ' + token
        }

    def connect_signal_r(self, message_received_func, open_close_update_func):
        url = api_host + '/messagehub'
        hub_connection = HubConnectionBuilder() \
        .with_url(url, options={
            "verify_ssl": verify_ssl,
            "access_token_factory": self.get_or_update_access_token
        }) \
        .with_automatic_reconnect({
            "type": "raw",
            "keep_alive_interval": 10,
            "reconnect_interval": 15,
            "max_attempts": 15
        }) \
        .build()

        hub_connection.on('ReceiveMessage', message_received_func)
        hub_connection.on('ReceiveUpdateOpenCloseState', open_close_update_func)
        hub_connection.start();
        return hub_connection
        
    def get_settings(self):
        headers = self._get_headers()
        url = api_host + '/Smoker/Settings/latest'
        try:
            response = requests.get(url, headers=headers)
            data = response.json()
            return data
        except Exception as e:
            print(e)
            return None
        
        

def login_function():
    api_token = api_instance.get_or_update_access_token()

    return api_token



api_instance = None

def message_received(args):
    print('MEssage received')
    print(args[0])
    print(args[1])

def open_close_update(value):
    print('Open Close Update')
    print(value)

if __name__ == '__main__':
    print(os.environ)
    api_instance = api()
    hub_connection = api_instance.connect_signal_r(message_received, open_close_update)

    settings = api_instance.get_settings()
    print(settings)

    # measurement = {
    #     "measurementId": str(uuid.uuid4()),
    #     "fireSensor": 357,
    #     "contentSensor": 88,
    #     "sensor1": 38,
    #     "sensor2": 0,
    #     "sensor3": 0,
    #     "sensor4": 0,
    #     "openCloseState": 0,
    #     "isAutoMode": True
    # }

    # api_instance.post_measurement(measurement)

    for i in range(30):
        print(i)
        hub_connection.send('SendMessage', ['Test', str(i)])
        time.sleep(5)
    # api_instance.get_smoker()
    # test_signal_r()

    # time.sleep(10)
    # api_instance.get_smoker()
    