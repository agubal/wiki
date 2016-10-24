using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Wiki.Data.Context;

namespace Wiki.Data
{
    public class Repository : IRepository
    {
        private readonly IDapperContext _context;

        public Repository(string connectionString)
        {
            _context = new DapperContext(connectionString);
        }

        public Repository(IDapperContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, object param)
        {
            return await _context.Connection.QueryAsync<T>(query, param);
        }

        public async Task ExecuteAsync(string sql, object param)
        {
            await _context.Connection.ExecuteAsync(sql, param);
        }
    }
}
