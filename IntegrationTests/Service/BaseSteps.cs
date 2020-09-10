using FluentAssertions;
using Flurl.Http;
using IntegrationTests.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace IntegrationTests.Serivce
{
    public class BaseSteps : Base.BaseComponentTest
    {
        private HttpResponseMessage responseMessage;
        private string NewName;
        private string RandomID;
        private string RandomValue;
        private TestDataManagement testDataManagement;
        private string accessNumber;
        public BaseSteps()
        {
            testDataManagement = new TestDataManagement();
            accessNumber = "";
        }
        public BaseSteps SetTestDataByKey(string strKEY)
        {
            testDataManagement.SetTestDataSource(TestDataSourceType.JSON);
            if(Environment.GetEnvironmentVariable("APIEnvironment")!=null)
                testDataManagement.SetJsonFile(@"Data/"+Environment.GetEnvironmentVariable("APIEnvironment") +".json");
            else
                testDataManagement.SetJsonFile(@"Data/Dev.json");
            accessNumber = testDataManagement.GetTestData(strKEY);
            accessNumber.Should().NotBeNullOrEmpty();
            return this;
        }
        public BaseSteps VerifyHttpStatus(HttpStatusCode httpStatusCode)
        {
            VerifyCommonHttpStatus(responseMessage, httpStatusCode);
            return this;
        }
        public BaseSteps VerifyContentInclude(string containsContent)
        {
            StartAllureSteps("VerifyCustomerJSONContent", "");
            responseMessage.Content.ReadAsStringAsync().Result.Should().Contain(containsContent);
            StopAllureSteps("");
            return this;
        }
        public BaseSteps VerifyContentIncludeNewName()
        {
            StartAllureSteps("VerifyCustomerJSONContent", "");
            responseMessage.Content.ReadAsStringAsync().Result.Should().Contain(NewName);
            StopAllureSteps("");
            return this;
        }
        public BaseSteps VerifyCustomersJSONContent()
        {
            StartAllureSteps("VerifyCustomerJSONContent", "");
            JObject jObject = JObject.Parse(responseMessage.Content.ReadAsStringAsync().Result);
            if (jObject["data"].Count() > 0)
                jObject["data"].ToString().Should().NotBeNullOrEmpty();
            jObject["total"].Should().NotEqual("0");
            for(int i = 0; i < jObject["data"].Count(); i++)
            {
                jObject["data"][i]["id"].ToString().Should().NotBeNull();
                jObject["data"][i]["name"].ToString().Should().NotBeNull();
                jObject["data"][i]["value"].ToString().Should().NotBeNull();
            }
            StopAllureSteps("");
            return this;
        }
        public BaseSteps VerifyCustomerJSONContent()
        {
            StartAllureSteps("VerifyCustomerJSONContent", "");
            JObject jObject = JObject.Parse(responseMessage.Content.ReadAsStringAsync().Result);
            
            jObject["id"].ToString().Should().NotBeNull();
            jObject["name"].ToString().Should().NotBeNull();
            jObject["value"].ToString().Should().NotBeNull();
            
            StopAllureSteps("");
            return this;
        }
        public BaseSteps GetNewName()
        {
            StartAllureSteps("GetNewName", "");
            string responseJson = responseMessage.Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(responseJson);
            if (jObject["data"].Count() > 0)
                jObject["data"].ToString().Should().NotBeNullOrEmpty();
            for(int i=1;i<10000;i++)
            {
                string strName = "AutoTest"+i.ToString();
                if (!responseJson.Contains(strName))
                {
                    NewName = strName;
                    return this;
                }
            }
            StopAllureSteps("");
            return this;
        }
        public BaseSteps GetRandomName()
        {
            StartAllureSteps("GetRandomName", "");
            string responseJson = responseMessage.Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(responseJson);
            if (jObject["data"].Count() > 0)
                jObject["data"].ToString().Should().NotBeNullOrEmpty();
            int iRandom = new Random().Next(0, jObject["data"].Count() - 1);
            RandomID = jObject["data"][iRandom]["id"].ToString();
            NewName = jObject["data"][iRandom]["name"].ToString();
            RandomValue = jObject["data"][iRandom]["value"].ToString();
            StopAllureSteps("");
            return this;
        }
        public BaseSteps GetSecondName()
        {
            StartAllureSteps("GetSecondName", "");
            string responseJson = responseMessage.Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(responseJson);
            if (jObject["data"].Count() > 1)
                jObject["data"].ToString().Should().NotBeNullOrEmpty();
            int iRandom = new Random().Next(0, jObject["data"].Count() - 1);
            RandomID = jObject["data"][iRandom]["id"].ToString();
            if (iRandom == 0)
                iRandom = 1;
            else
                iRandom = 0;
            NewName = jObject["data"][iRandom]["name"].ToString();
            RandomValue = jObject["data"][iRandom]["value"].ToString();
            StopAllureSteps("");
            return this;
        }
        public BaseSteps PostCustmoerDetail(string baseUrl)
        {
            string postContent = "{\"name\":\""+ NewName + "\",\"value\":\""+ NewName + "\"}";
            StartAllureSteps("PostCustmoerDetail", postContent);
            AddUrlToStep(baseUrl);
            Thread.Sleep(2000);
            responseMessage = baseUrl.AllowAnyHttpStatus()
                           .WithHeader("User-Agent", "IntegrationTest")
                           .WithHeader("x-fapi-interaction-id", Guid.NewGuid().ToString())
                           .PostStringAsync(postContent).Result;

            StopAllureSteps(responseMessage.Content.ReadAsStringAsync().Result);
            return this;
        }
        public BaseSteps PutCustmoerDetail(string baseUrl)
        {
            string postContent = "{\"name\":\"" + NewName + "\",\"value\":\"" + RandomValue + "\"}";
            StartAllureSteps("PutCustmoerDetail", postContent);
            AddUrlToStep(baseUrl);
            Thread.Sleep(2000);
            responseMessage = (baseUrl+"/"+RandomID).AllowAnyHttpStatus()
                           .WithHeader("User-Agent", "IntegrationTest")
                           .WithHeader("x-fapi-interaction-id", Guid.NewGuid().ToString())
                           .PutStringAsync(postContent).Result;

            StopAllureSteps(responseMessage.Content.ReadAsStringAsync().Result);
            return this;
        }
        public BaseSteps DeleteCustmoerDetail(string baseUrl)
        {
            StartAllureSteps("DeleteCustmoerDetail", "");
            AddUrlToStep(baseUrl);
            Thread.Sleep(2000);
            responseMessage = (baseUrl + "/" + RandomID).AllowAnyHttpStatus()
                           .WithHeader("User-Agent", "IntegrationTest")
                           .WithHeader("x-fapi-interaction-id", Guid.NewGuid().ToString())
                           .DeleteAsync().Result;

            StopAllureSteps(responseMessage.Content.ReadAsStringAsync().Result);
            return this;
        }
        public BaseSteps GetAllCustomerDetails(string baseUrl)
        {
            StartAllureSteps("GetAllCustomerDetails", "");
            AddUrlToStep(baseUrl);
            Thread.Sleep(2000);
            responseMessage = baseUrl.AllowAnyHttpStatus()
                           .WithHeader("User-Agent", "IntegrationTest")
                           .WithHeader("x-fapi-interaction-id", Guid.NewGuid().ToString())
                           .GetAsync().Result;

            StopAllureSteps(responseMessage.Content.ReadAsStringAsync().Result);
            return this;
        }
        public BaseSteps GetCustomerDetailsByID(string baseUrl)
        {
            StartAllureSteps("GetCustomerDetailsByID", "");
            AddUrlToStep(baseUrl);
            Thread.Sleep(2000);
            responseMessage = (baseUrl + "/" + RandomID).AllowAnyHttpStatus()
                           .WithHeader("User-Agent", "IntegrationTest")
                           .WithHeader("x-fapi-interaction-id", Guid.NewGuid().ToString())
                           .GetAsync().Result;

            StopAllureSteps(responseMessage.Content.ReadAsStringAsync().Result);
            return this;
        }

        /// <summary>
        /// Format will be https://[API_ENV]/api/values
        /// </summary>
        public static string MicroserviceBaseUrl => "http://192.168.1.118:30000/";
        
    }
}
