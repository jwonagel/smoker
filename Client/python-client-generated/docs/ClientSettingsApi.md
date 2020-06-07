# swagger_client.ClientSettingsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**client_settings_current_get**](ClientSettingsApi.md#client_settings_current_get) | **GET** /Client/Settings/current | 
[**client_settings_current_post**](ClientSettingsApi.md#client_settings_current_post) | **POST** /Client/Settings/current | 

# **client_settings_current_get**
> SettingsClient client_settings_current_get()



### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint

# Configure API key authorization: Bearer
configuration = swagger_client.Configuration()
configuration.api_key['Authorization'] = 'YOUR_API_KEY'
# Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
# configuration.api_key_prefix['Authorization'] = 'Bearer'

# create an instance of the API class
api_instance = swagger_client.ClientSettingsApi(swagger_client.ApiClient(configuration))

try:
    api_response = api_instance.client_settings_current_get()
    pprint(api_response)
except ApiException as e:
    print("Exception when calling ClientSettingsApi->client_settings_current_get: %s\n" % e)
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**SettingsClient**](SettingsClient.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **client_settings_current_post**
> client_settings_current_post(body=body)



### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint

# Configure API key authorization: Bearer
configuration = swagger_client.Configuration()
configuration.api_key['Authorization'] = 'YOUR_API_KEY'
# Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
# configuration.api_key_prefix['Authorization'] = 'Bearer'

# create an instance of the API class
api_instance = swagger_client.ClientSettingsApi(swagger_client.ApiClient(configuration))
body = swagger_client.SettingsClient() # SettingsClient |  (optional)

try:
    api_instance.client_settings_current_post(body=body)
except ApiException as e:
    print("Exception when calling ClientSettingsApi->client_settings_current_post: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**SettingsClient**](SettingsClient.md)|  | [optional] 

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

