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
                .VerifyCustomersJSONContent();
        }
        [Test(Description = "Get Customer details with bad url")]
        [Category("Customers")]
        public void GetAllCustomerDetails_BadURL()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values1111")
                //Then
                .VerifyHttpStatus(HttpStatusCode.NotFound);
        }
        [Test(Description = "add Customer details with id not in")]
        [Category("Customers")]
        public void CreateCustomerDetails_Success()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetMaxID()
                //then
                .PostCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values","Name","Value")
                .VerifyHttpStatus(HttpStatusCode.Created)
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyContentInclude("");
        }
    }
}
