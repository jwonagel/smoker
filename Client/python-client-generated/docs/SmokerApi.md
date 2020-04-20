# swagger_client.SmokerApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**smoker_get**](SmokerApi.md#smoker_get) | **GET** /Smoker | 
[**smoker_post**](SmokerApi.md#smoker_post) | **POST** /Smoker | 

# **smoker_get**
> list[str] smoker_get()



### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint

# create an instance of the API class
api_instance = swagger_client.SmokerApi()

try:
    api_response = api_instance.smoker_get()
    pprint(api_response)
except ApiException as e:
    print("Exception when calling SmokerApi->smoker_get: %s\n" % e)
```

### Parameters
This endpoint does not need any parameter.

### Return type

**list[str]**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **smoker_post**
> smoker_post(body=body)



### Example
```python
from __future__ import print_function
import time
import swagger_client
from swagger_client.rest import ApiException
from pprint import pprint

# create an instance of the API class
api_instance = swagger_client.SmokerApi()
body = swagger_client.MeasurementSmoker() # MeasurementSmoker |  (optional)

try:
    api_instance.smoker_post(body=body)
except ApiException as e:
    print("Exception when calling SmokerApi->smoker_post: %s\n" % e)
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**MeasurementSmoker**](MeasurementSmoker.md)|  | [optional] 

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

