using Allure.Commons;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IntegrationTests.Base
{
    public abstract class BaseComponentTest
    {
        
        private AllureSteps allureSteps;
        
        public BaseComponentTest()
        {
            
            allureSteps = new AllureSteps();
        }

        
        public void StartAllureSteps(string stepName, string outputString)
        {
            allureSteps.StartStep(stepName, outputString);
        }
        public void AddUrlToStep(string url)
        {
            allureSteps.AddOutputToAllureReport("URL", url);
        }
        public void StopAllureSteps(string outputString)
        {
            allureSteps.StopStep(outputString);
        }
        
        public void VerifyCommonHttpStatus(HttpResponseMessage responseMessage,HttpStatusCode httpStatusCode)
        {
            allureSteps.StartStep("VerifyHttpStatus:"+ httpStatusCode.ToString(), "");
            responseMessage.StatusCode.Should().Be(httpStatusCode);
            allureSteps.StopStep("");
        }
       
        

    }
}
