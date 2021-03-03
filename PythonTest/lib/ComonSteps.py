import allure
import requests
import json

@allure.step
def Verify_statuscode(status_code,expected):
    assert status_code == expected
    
@allure.step
def Verify_jsonformat(response_body):
    #response_body = json.load(response)
    assert response_body["total"] >= 1
    assert len(response_body["customers"]) >= 1
    Verify_SingleJsonformat(response_body["customers"][0])
    
@allure.step
def Verify_SingleJsonformat(response_body):
    #response_body = json.load(response)
    assert response_body["id"] != ""
    assert response_body["name"] != ""
    assert response_body["email"] != ""
    
@allure.step
def get_request(apiurl):
    headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
    return requests.get(apiurl,headers=headers,verify=False)
    
@allure.step
def post_request(apiurl,senddata):
    headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
    return requests.post(apiurl,json = senddata, headers=headers,verify=False)
    
@allure.step
def put_request(apiurl,senddata):
    headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
    return requests.put(apiurl,json = senddata, headers=headers,verify=False)
    
@allure.step
def delete_request(apiurl):
    headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
    return requests.delete(apiurl, headers=headers,verify=False)