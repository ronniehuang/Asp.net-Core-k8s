using System;
using System.Net;
using System.Net.Http;
using IntegrationTests.Base;
using IntegrationTests.Serivce;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using Xunit;

namespace IntegrationTests.TestScripts
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("IntegrationTests")]
    [AllureDisplayIgnored]
    [NonParallelizable]
    public class PipelineIntegrationTest : AllureReportConfig
    {

        [Test(Description = "Get Customer details")]
        [Category("Customers")]
        public void GetAllCustomerDetails()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl+ "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomerJSONContent();
        }

    }
}
