using System.Data;
using System.Data.SqlClient;

namespace Wiki.Data.Context
{
    public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);

                if (string.IsNullOrWhiteSpace(_connection.ConnectionString))
                    _connection.ConnectionString = _connectionString;

                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        public void Dispose()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed) return;
            _connection.Close();
            _connection = null;
        }
    }
}
