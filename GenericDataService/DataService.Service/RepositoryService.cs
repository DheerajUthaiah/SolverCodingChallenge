using DataService.Common;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class RepositoryService<TRepository, TConcreteType> : IRepositoryService<TRepository>
        where TRepository : class, IRepository
        where TConcreteType : class, TRepository
    {

        protected readonly ConcurrentDictionary<string, string> DbNameToConnectionStringMap;
        protected IServiceProvider ServiceProvider;
        private SqlConfigurationList SqlConfigurationList;

        public RepositoryService(IServiceProvider serviceProvider, SqlConfigurationList sqlConfigurationList)
        {
            this.ServiceProvider = serviceProvider;
            this.SqlConfigurationList = sqlConfigurationList;
            var keyValuePair = sqlConfigurationList.SqlConfigurations.Select(config => new KeyValuePair<string, string>(config.DbName, config.ConnectionString));
            DbNameToConnectionStringMap = new ConcurrentDictionary<string, string>(keyValuePair);
        }
        public Task<TRepository> GetRepositoryForDatabase(string database)
        {
            if(DbNameToConnectionStringMap.TryGetValue(database, out string connectionString))
            {
                return DoGetRepositoryForDatabase();
            }
            else
            {
                throw new InvalidDbException(database);
            }
        }

        private Task<TRepository> DoGetRepositoryForDatabase()
        {
            var logger = ServiceProvider.GetService(typeof(ILogger)) as ILogger;
            var repository = (TRepository)Activator.CreateInstance(typeof(TConcreteType), logger, SqlConfigurationList);
            return Task.FromResult(repository);
        }
    }
}
