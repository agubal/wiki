using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wiki.Data
{
    public interface IRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null);
        Task ExecuteAsync(string sql, object param = null);
    }
}
