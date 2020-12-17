using DataService.Common;
using DataService.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class BusinessServiceBase
    {
        public ILogger Logger { get; set; }
        public IRepositoryService<ISolverRepository> RepositoryService { get; set; }

        public BusinessServiceBase(ILogger logger, IRepositoryService<ISolverRepository> repositoryService)
        {
            Logger = logger;
            RepositoryService = repositoryService;
        }

        protected virtual async Task<string> DoQueryDB(string database, Func<ISolverRepository, Task<string>> queryFunc)
        {
            var repository = await RepositoryService.GetRepositoryForDatabase(database);
            var result = await queryFunc(repository);
            return result;
        }
    }
}
