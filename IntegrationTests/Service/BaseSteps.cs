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
        public BaseSteps VerifyCustomerJSONContent()
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

        
        /// <summary>
        /// Format will be https://[API_ENV]/api/values
        /// </summary>
        public static string MicroserviceBaseUrl => "http://192.168.1.118:30000/";
        
    }
}
