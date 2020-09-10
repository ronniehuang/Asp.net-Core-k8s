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
        [Category("Production")]
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
        [Test(Description = "add Customer details with name not in")]
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
                .GetNewName()
                //then
                .PostCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.Created)
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyContentIncludeNewName();
        }
        [Test(Description = "add Customer details with exists name")]
        [Category("Customers")]
        public void CreateCustomerDetails_Fail()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetRandomName()
                //then
                .PostCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.NotFound);
        }
        [Test(Description = "update Customer details with not exists name")]
        [Category("Customers")]
        public void UpdateCustomerDetails_Success()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetRandomName()
                .GetNewName()
                //then
                .PutCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.OK)
                //When
                .GetCustomerDetailsByID(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyContentIncludeNewName(); 
        }
        
        [Test(Description = "update Customer details with exists name")]
        [Category("Customers")]
        public void UpdateCustomerDetails_Fail()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetSecondName()
                //then
                .PutCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.NotFound);
        }
        [Test(Description = "Delete Customer details")]
        [Category("Customers")]
        public void DeleteCustomerDetails_Success()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetRandomName()
                //then
                .DeleteCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.OK);
        }
        [Test(Description = "Delete Customer details with noexists id")]
        [Category("Customers")]
        public void DeleteCustomerDetails_Fail()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetRandomName()
                //then
                .DeleteCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.OK)
                //then
                .DeleteCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.NotFound);
        }
        [Test(Description = "Get Customer details with exists id")]
        [Category("Customers")]
        [Category("Production")]
        public void GetOneCustomerDetails_Success()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetRandomName()
                //When
                .GetCustomerDetailsByID(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomerJSONContent();
        }
        [Test(Description = "Get Customer details with not exists id")]
        [Category("Customers")]
        public void GetOneCustomerDetails_Fail()
        {
            new Serivce.BaseSteps()
                //Given
                .SetTestDataByKey("AccessNumber:L1")
                //When
                .GetAllCustomerDetails(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.OK)
                .VerifyCustomersJSONContent()
                .GetRandomName()
                //then
                .DeleteCustmoerDetail(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                .VerifyHttpStatus(HttpStatusCode.OK)
                //When
                .GetCustomerDetailsByID(Serivce.BaseSteps.MicroserviceBaseUrl + "api/values")
                //Then
                .VerifyHttpStatus(HttpStatusCode.NotFound);
        }
    }
}
