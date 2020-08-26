using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace IntegrationTests.Data
{
    public class TestDataManagement
    {
        private TestDataSourceType sourceType;
        private string jsonSourceDataFile;
        private string databaseConnectString;
        public void SetTestDataSource(TestDataSourceType fromSourceType)
        {
            sourceType = fromSourceType;
        }
        public string GetTestData(string keyWord)
        {
            if (sourceType == TestDataSourceType.JSON)
                return GetJSONTestData(keyWord);
            return "";
        }
        public string GetJSONTestData(string keyWord)
        {
            var configJSON = new ConfigurationBuilder()
                .AddJsonFile(jsonSourceDataFile)
                .Build();
            return configJSON[keyWord];
        }
        public void SetJsonFile(string inputFileName)
        {
            jsonSourceDataFile = inputFileName;
        }
        public void SetDataBase(string connectString)
        {
            databaseConnectString = connectString;
        }
        
    }
    public enum TestDataSourceType
    {
        [EnumMember(Value = "JSON")]
        JSON = 0,
        [EnumMember(Value = "DATABASE")]
        DATABASE = 1,
        [EnumMember(Value = "MIDDLEWARE")]
        MIDDLEWARE = 2,
    }
}
