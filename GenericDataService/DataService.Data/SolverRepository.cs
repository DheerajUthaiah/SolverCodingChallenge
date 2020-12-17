using DataService.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Data
{
    public class SolverRepository : RepositoryBase, ISolverRepository
    {
        public SolverRepository(ILogger logger, SqlConfigurationList sqlConfigurationList) :base(logger, sqlConfigurationList)
        {
        }
        public async Task<string> GetDataFromDatabaseTable(string tableName, string database)
        {
            var sql = $"SELECT * FROM {tableName} FOR JSON AUTO";
            var result = await QueryAsync(sql, database);
            return result;
        }
    }
}
