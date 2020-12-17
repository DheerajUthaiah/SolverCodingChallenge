using System.Threading.Tasks;
using DataService.Common;

namespace DataService.Data
{
    public interface ISolverRepository : IRepository
    {
        Task<string> GetDataFromDatabaseTable(string tableName, string database);
    }
}
