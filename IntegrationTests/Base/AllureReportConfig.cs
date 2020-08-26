using Allure.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace IntegrationTests.Base
{
    public abstract class AllureReportConfig
    {
        public AllureReportConfig()
        {
            Environment.SetEnvironmentVariable(
                AllureConstants.ALLURE_CONFIG_ENV_VARIABLE,
                Path.Combine(AssemblyDirectory, AllureConstants.CONFIG_FILENAME));
        }
        private string AssemblyDirectory
        {
            get
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string theDirectory = Path.GetDirectoryName(assemblyLocation);
                return theDirectory;
            }
        }
    }
}
