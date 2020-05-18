import swagger_client
from swagger_client.rest import ApiException
import datetime
import requests, json

class api:

    def __init__(self, host, api_token, verify_ssl=True):
        configuration = swagger_client.Configuration()
        configuration.host = host 
        configuration.verify_ssl = verify_ssl
        configuration.api_key['Authorization'] = api_token
        self.api_instance = swagger_client.SmokerApi(swagger_client.ApiClient(configuration))
        

    def post_measurement(self, temp):
        body = swagger_client.MeasurementSmoker()
        body.sensor2 = temp
        body.time_stamp = datetime.datetime.now().isoformat()


        try:
            api_response = self.api_instance.smoker_post(body = body)
            print(api_response)
        except ApiException as e:
            print('Exception while SmokerApi -> smokder_post: $s\n' % e)
        except Exception as e:
            print(e)



def test():
    scopes = ['smokerapi']
    
    token_url = 'https://localhost:5123/connect/token'

    client_id = 'smoker'
    client_secret = 'secret'

    data = {'grant_type': 'client_credentials'}
    access_token_response = requests.post(token_url, data=data, verify=False, allow_redirects=False, auth=(client_id, client_secret))
    print(access_token_response.headers)
    print(access_token_response.text)

    tokens = json.loads(access_token_response.text)
    print("access token: " + tokens['access_token'])
    return tokens['token_type'], tokens['access_token']
    

def test_get(token):
    smoker_api_url = 'https://localhost:5001/Smoker'

    api_call_headers = {'Authorization': token, 'accept': 'application/json'}
    api_call_respose = requests.get(smoker_api_url, headers=api_call_headers, verify=False)
    print(api_call_respose)
    print(api_call_respose.content)



if __name__ == '__main__':
    token_type, token = test()
    api_token = token_type + ' ' + token
    test_get(api_token)
    api_instance = api('https://localhost:5001', api_token, verify_ssl=False)
    api_instance.post_measurement(45)
    