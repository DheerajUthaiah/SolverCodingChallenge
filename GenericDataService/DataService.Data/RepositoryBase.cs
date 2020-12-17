using Serilog;
using System;
using System.Data.SqlClient;
using DataService.Common;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DataService.Data
{
    public abstract class RepositoryBase
    {
        public ILogger Logger { get; set; }
        public SqlConfigurationList SqlConfigurationList { get; set; }
        public RepositoryBase(ILogger logger, SqlConfigurationList sqlConfigurationList)
        {
            Logger = logger;
            SqlConfigurationList = sqlConfigurationList;
        }

        private SqlConfiguration ConfigureConnection(string key)
        {
            var connectionString = SqlConfigurationList.GetValueByKey(key);
            var sqlConfiguration = new SqlConfiguration { ConnectionString = connectionString, DbName = key };
            return sqlConfiguration;
        }

        protected virtual async Task<T> DataCallGate<T>(Func<SqlConnection, T> dataFunc, string database)
        {
            try
            {
                var sqlConfiguration = ConfigureConnection(database);
                using (var dbConn = new SqlConnection(sqlConfiguration.ConnectionString))
                {
                    dbConn.Open();
                    return dataFunc(dbConn);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                throw ex;
            }
        }

        protected virtual async Task<string> QueryAsync(string sql, string database)
        {
            string result = default;
            var data = await DataCallGate(sqlConnection => sqlConnection.Query<string>(sql, buffered: false).ToList(), database);
            if (data != null)
                result = string.Join("", data);
            return result;
        }
    }
}