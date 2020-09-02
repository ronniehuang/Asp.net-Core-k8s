using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ApiDemo.Controllers
{
    /// <summary>
    /// Summary description for CommonDb.
    /// </summary>
    public class CommonDb : MyDataBase
    {

        public static string pSqlConnString;
        public CommonDb()
        {

            this.OpenDatabase();
        }
        
        public SqlConnection getConnection()
        {
            return conn;
        }
       
        public int ExecSql(string strSql)
        {
            //strSql = strSql.Replace("'", "''");
            strSql = strSql.Replace("--", "~~");
            //strSql = strSql.Replace("'", "''");

            try
            {

                DataSet dsTemp = new DataSet();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 1200;
                sqlCommand.CommandText = strSql;
                sqlCommand.Connection = conn;
                sqlCommand.ExecuteNonQuery();
                
                return 1;
            }
            catch (Exception ex)
            {
                string strErr;
                strErr = (strSql + "£º" + ex.Message);
                throw new Exception(strErr);

            }
        }
        
        public int GetDataSet(string strSql, out DataSet dsAll)
        {
            //strSql = strSql.Replace("'", "''");
            strSql = strSql.Replace("--", "~~");
            dsAll = null;
            if (!ConnectionIsValid())
            {
                this.OpenDatabase();
            }
            try
            {

                DataSet dsTemp = new DataSet();
                SqlCommand sqlCommand = new SqlCommand();

                sqlCommand.CommandText = strSql;
                sqlCommand.Connection = conn;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                adapter.Fill(dsTemp);
                dsAll = dsTemp;
                return 1;
            }
            catch (Exception ex)
            {
                string strErr;
                strErr = (strSql + "£º" + ex.Message);
                throw new Exception(strErr);
            }
        }


    }
}