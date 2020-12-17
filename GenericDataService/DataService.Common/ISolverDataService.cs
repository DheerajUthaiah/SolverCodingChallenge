using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Common
{
    public interface ISolverDataService
    {
        Task<string> GetData(string database, string table);
    }
}
