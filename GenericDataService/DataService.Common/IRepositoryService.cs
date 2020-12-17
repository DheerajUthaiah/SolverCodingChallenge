using System.Threading.Tasks;

namespace DataService.Common
{
    public interface IRepositoryService<TRepository> where TRepository:IRepository
    {
        Task<TRepository> GetRepositoryForDatabase(string database);
    }
}
