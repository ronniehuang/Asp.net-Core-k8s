using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace ApiDemo.Controllers
{
    /// <summary>
    /// Summary description for Class2.
    /// </summary>
    public class MyDataBase
    {
        private string strConnectionString;
        protected SqlConnection conn;

        public MyDataBase()
        {
            strConnectionString = ReadJsonConfig("LocalDB");
        }
        public string ReadJsonConfig(string strKey)
        {
            var config = new ConfigurationBuilder()
                 .AddJsonFile("Config.json")
                 .Build();
            return config[strKey];

        }
        public bool ConnectionIsValid()
        {
            try
            {
                string strSql = @"select top 1 id from sysobjects";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                Object o = cmd.ExecuteScalar();
                return true;
            }
            catch
            {
                CloseDatabase();
                if (OpenDatabase())
                    return true;
                else
                    return false;
            }
        }

        public void CloseDatabase()
        {
            try
            {
                if ((conn != null) && (conn.State.ToString() == "Open"))
                    conn.Close();
                conn = null;
            }
            catch (Exception ex)
            {                
                string strErr;
                strErr = ("Open Database Error£º" + ex.Message);
            }
        }

        public bool OpenDatabase()
        {
            try
            {
                conn = new SqlConnection(strConnectionString);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                string strErr;
                strErr = ("Open Database Error£º" + ex.Message);

                return false;
            }
        }
    }
    public enum EXECUTE_RETURN
    {
        DATABASE_OPEN_FAILED = -101,
        EXECUTE_SQL_ERROR = -102,
        EXECUTE_SQL_SUCCESS = 0,
    }

}