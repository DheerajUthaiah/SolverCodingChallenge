using DataService.Common;
using DataService.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class SolverDataService : BusinessServiceBase, ISolverDataService
    {
        public SolverDataService(IRepositoryService<ISolverRepository> repositoryService, ILogger logger): base(logger, repositoryService)
        {
        }
        public Task<string> GetData(string database, string table)
        {
            return DoQueryDB(database, repository => DoGetData(table, database, repository));
        }

        protected async Task<string> DoGetData(string table, string database, ISolverRepository repository)
        {
            var result = await repository.GetDataFromDatabaseTable(table, database);
            return result;
        }
    }
}
