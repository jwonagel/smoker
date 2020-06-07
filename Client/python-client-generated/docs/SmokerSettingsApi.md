# swagger_client.SmokerSettingsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**smoker_settings_latest_get**](SmokerSettingsApi.md#smoker_settings_latest_get) | **GET** /Smoker/Settings/latest | 

# **smoker_settings_latest_get**
> SettingsSmoker smoker_settings_latest_get()



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
api_instance = swagger_client.SmokerSettingsApi(swagger_client.ApiClient(configuration))

try:
    api_response = api_instance.smoker_settings_latest_get()
    pprint(api_response)
except ApiException as e:
    print("Exception when calling SmokerSettingsApi->smoker_settings_latest_get: %s\n" % e)
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**SettingsSmoker**](SettingsSmoker.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

