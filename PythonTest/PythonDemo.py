import requests
import allure
from lib import ComonSteps
import json

def test_get_check_status_code_equals_200():
    with open('config.json') as json_file:
        data = json.load(json_file)
    response = ComonSteps.get_request(data['APIURL'])
    allure.attach(response.content, 'response.json',
                      allure.attachment_type.TEXT)
    ComonSteps.Verify_statuscode(response.status_code,200)
    ComonSteps.Verify_jsonformat(response.json())
    

def test_get_byid_check_status_code_equals_200():
    with open('config.json') as json_file:
        data = json.load(json_file)
    
    response = ComonSteps.get_request(data['APIURL'])
    allure.attach(response.content, 'response.json',
                      allure.attachment_type.TEXT)
    ComonSteps.Verify_statuscode(response.status_code,200)
    ComonSteps.Verify_jsonformat(response.json())
    id = response.json()["customers"][0]["id"]
    
    response = ComonSteps.get_request(data['APIURL']+"/"+str(id))
    allure.attach(response.content, 'response.json',
                      allure.attachment_type.TEXT)
    ComonSteps.Verify_statuscode(response.status_code,200)
    ComonSteps.Verify_SingleJsonformat(response.json())

def test_post_create_customer_equals_201():
    with open('config.json') as json_file:
        data = json.load(json_file)
    
    customer = {'name':'testNew','email':'testnew@test.com'}
    response = ComonSteps.post_request(data['APIURL'],customer)
    ComonSteps.Verify_statuscode(response.status_code,201)

def test_put_modify_customer_equals_200():
    with open('config.json') as json_file:
        data = json.load(json_file)
    
    response = ComonSteps.get_request(data['APIURL'])
    allure.attach(response.content, 'response.json',
                      allure.attachment_type.TEXT)
    ComonSteps.Verify_statuscode(response.status_code,200)
    ComonSteps.Verify_jsonformat(response.json())
    id = response.json()["customers"][0]["id"]
    customer = {"id": 0,"name":"testNew","email":"testnew@test.com"}
    
    response = ComonSteps.put_request(data['APIURL']+"/"+str(id),customer)
    allure.attach(response.content, 'response.json',
                      allure.attachment_type.TEXT)
    ComonSteps.Verify_statuscode(response.status_code,200)
    ComonSteps.Verify_SingleJsonformat(response.json())
    
def test_delete_customer_equals_200():
    with open('config.json') as json_file:
        data = json.load(json_file)
    
    response = ComonSteps.get_request(data['APIURL'])
    allure.attach(response.content, 'response.json',
                      allure.attachment_type.TEXT)
    ComonSteps.Verify_statuscode(response.status_code,200)
    ComonSteps.Verify_jsonformat(response.json())
    id = response.json()["customers"][0]["id"]
    
    response = ComonSteps.delete_request(data['APIURL']+"/"+str(id))
    
    ComonSteps.Verify_statuscode(response.status_code,200)
    
