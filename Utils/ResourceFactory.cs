
using log4net;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace Utils
{
    public class ResourceFactory
    {
        private static IConfiguration _config;
        public ResourceFactory(IConfiguration config)
        {
            _config = config;
        }

        #region SQL Server Connection
        public static SqlConnection GetConnection()
        {
            SqlConnection cn = null;

            try
            {
                cn = new SqlConnection(GetConnectionString());
                cn.Open();
            }
            catch (Exception ex)
            {
                throw new CustomException("Eccezione SQL durante la creazione di una connessione", ex);
            }

            return cn;
        }

        public static SqlConnection GetConnection(string dbConnection)
        {
            SqlConnection cn = null;

            try
            {
                if (dbConnection != null)
                {
                    cn = new SqlConnection(GetConnectionString(dbConnection));
                }
                cn.Open();
            }
            catch (Exception ex)
            {
                throw new CustomException("Eccezione SQL durante la creazione di una connessione", ex);
            }

            return cn;
        }


        public static string GetConnectionString()
        {
            string connString;
#if DEBUG
            //connString = _config.GetSection("AZURE:TestsqlAzure").Value;
            connString = _config.GetSection("ConnectionString").GetSection("SQLSERVER:LocalSvil").Value;
#else
            connString = _config.GetSection("ConnectionString").GetSection("AZURE:sqlAzure").Value;
#endif

            return connString;
        }

        #endregion
        public static string GetConnectionString(string dbConnection)
        {
            return _config.GetSection("ConnectionString").GetSection($"{dbConnection}").Value;
        }

        #region Oracle Connection
        public static OracleConnection GetOracleConnection(string dbConnection)
        {
            OracleConnection cn = null;

            try
            {
                if (dbConnection != null)
                {
                    cn = new OracleConnection(GetConnectionString(dbConnection));
                }
                cn.Open();
            }
            catch (Exception ex)
            {
                throw new CustomException("Eccezione SQL durante la creazione di una connessione", ex);
            }

            return cn;
        }
        public static OracleConnection GetOracleConnection()
        {
            OracleConnection cn = null;

            try
            {
                cn = new OracleConnection(GetOracleConnectionString());
                cn.Open();
            }
            catch (Exception ex)
            {
                throw new CustomException("Eccezione SQL durante la creazione di una connessione a JDE", ex);
            }

            return cn;
        }

        public static string GetOracleConnectionString()
        {
            string connString;
#if DEBUG

            connString = _config.GetSection("ConnectionString").GetSection("ORACLE:TestJDE").Value;
#else
            connString = _config.GetSection("ConnectionString").GetSection("ORACLE:JDE").Value;
#endif
            return connString;
        }

        #endregion

        public static string GetAppSetting(string key)
        {
            string ret = _config.GetSection($"{key}").Value;
            return ret;
        }

        public static string ApplicationPath
        {
            get
            {
#if(DEBUG)
                return AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
#else
                return AppContext.BaseDirectory;
#endif
            }
        }

#region Logging
        public static ILog FileLogger(Type logtype) {
            return LogManager.GetLogger(logtype);
        }
#endregion

    }
}
