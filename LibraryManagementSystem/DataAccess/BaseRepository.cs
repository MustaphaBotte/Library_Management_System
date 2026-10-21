using Microsoft.Data.SqlClient;

namespace LMS.DataAccess
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LMS")
                ?? throw new InvalidOperationException("Connection string 'LMS' not found.");
        }

        protected SqlConnection CreateConnection() => new(_connectionString);
    }
}
