using Allure.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Serilog;

namespace IntegrationTests.Base
{
    public class AllureSteps
    {
        private string stepUUID;
        private string staticStepName;
        public void StopStep(string OutputString)
        {
            try
            {
                if (stepUUID != "")
                {
                    if (OutputString != "")
                        AddOutputToAllureReport("Response", OutputString);
                    AllureLifecycle.Instance.StopStep(stepUUID);
                    stepUUID = "";
                    staticStepName = "";
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
        }
        public void StartStep(string stepName, string OutputString)
        {
            try
            {
                StopStep("");
                stepUUID = Guid.NewGuid().ToString();
                staticStepName = stepName;
                StepResult stepResult = new StepResult
                {
                    name = stepName,
                    status = Status.passed
                };
                AllureLifecycle.Instance.StartStep(stepUUID, stepResult);
                if (OutputString != "")
                    AddOutputToAllureReport("Request", OutputString);
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }

        }
        public void StartStepFail(string stepName, string OutputString)
        {
            try
            {
                if (stepUUID != "")
                {
                    AllureLifecycle.Instance.StopStep(stepUUID);
                }
                stepUUID = Guid.NewGuid().ToString();
                StepResult stepResult = new StepResult
                {
                    name = stepName,
                    status = Status.failed
                };
                AllureLifecycle.Instance.StartStep(stepUUID, stepResult);
                if (OutputString != "")
                    AddOutputToAllureReport("Request", OutputString);
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }

        }
        public void AddOutputToAllureReport(string requestOrResponse, string strOutput)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(strOutput);
                if (requestOrResponse == "Request")
                    AllureLifecycle.Instance.AddAttachment("Request_" + DateTime.Now.ToString("yyyyMMddhhmmss"), "text/plain", bytes);
                else if (requestOrResponse == "URL")
                    AllureLifecycle.Instance.AddAttachment("URL_" + DateTime.Now.ToString("yyyyMMddhhmmss"), "text/plain", bytes);
                else if (requestOrResponse == "Response")
                    AllureLifecycle.Instance.AddAttachment("Response_" + DateTime.Now.ToString("yyyyMMddhhmmss"), "text/plain", bytes);
                else
                    AllureLifecycle.Instance.AddAttachment("Attachment_" + DateTime.Now.ToString("yyyyMMddhhmmss"), "text/plain", bytes);
            }
            catch(Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
        }
    }
}
