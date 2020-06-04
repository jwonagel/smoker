import swagger_client
from swagger_client.rest import ApiException
import datetime
import requests, json
import time
import os
from signalrcore.hub_connection_builder import HubConnectionBuilder


authority_host = os.getenv('Authority')
secret = os.getenv('ClientSecret')
api_host = os.getenv('api_host')
verify_ssl = json.loads(os.getenv('verify_ssl', 'True').lower())

class api:

    def __init__(self):
        configuration = swagger_client.Configuration()
        configuration.host = api_host 
        configuration.verify_ssl = verify_ssl
        self.token_url = authority_host + '/connect/token'
        self.client_secret = secret
        self.api_instance = swagger_client.SmokerApi(swagger_client.ApiClient(configuration))
        self.expiration_time = time.time()
        self.configuration = configuration
        self.access_token = ''

    def post_measurement(self, measurement):
        self.check_and_update_accesstoken()
        measurement.time_stamp = datetime.datetime.now().isoformat()


        try:
            api_response = self.api_instance.smoker_post(body = measurement)
            print(api_response)
        except ApiException as e:
            print('Exception while SmokerApi -> smokder_post: $s\n' % e)
        except Exception as e:
            print(e)

    def get_smoker(self):
        self.check_and_update_accesstoken()
        body = self.api_instance.smoker_get()
        print(body)

    def check_and_update_accesstoken(self):
        """Checks for valid token or updates"""
        current_time = time.time()
        if current_time + 30 >= self.expiration_time:
            try:
                token_type, access_token, expires_in = self._get_access_token()
                self.access_token = access_token
                self.expiration_time = current_time + expires_in
                self.configuration.api_key['Authorization'] = token_type + ' ' + access_token
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
            access_token_response = requests.post(self.token_url, data=data, verify=False, allow_redirects=False, auth=(client_id, self.client_secret))
        except Exception as e:
            print('error while requestining accesstoken', e)
            raise e
        
        tokens = json.loads(access_token_response.text)
        return tokens['token_type'], tokens['access_token'], tokens['expires_in']


def login_function():
    api_token = api_instance.get_or_update_access_token()
    return api_token


def test_signal_r():
    url = api_host + '/messagehub'
    hub_connection = HubConnectionBuilder() \
        .with_url(url, options={
            "verify_ssl": verify_ssl,
            "access_token_factory": api_instance.get_or_update_access_token
        }) \
        .with_automatic_reconnect({
            "type": "raw",
            "keep_alive_interval": 10,
            "reconnect_interval": 15,
            "max_attempts": 15
        }) \
        .build()

    hub_connection.on('ReceiveMessage', print)
    hub_connection.start()
    message = None

    for i in range(100):
        time.sleep(1)


api_instance = None

if __name__ == '__main__':
    print(os.environ)
    api_instance = api()
    # measurement = swagger_client.MeasurementSmoker()
    # measurement.sensor1 = 1
    # measurement.sensor2 = 3
    api_instance.get_smoker()
    test_signal_r()

    # time.sleep(10)
    # api_instance.get_smoker()
    