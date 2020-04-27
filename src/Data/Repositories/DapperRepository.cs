using Npgsql;
using System.Data;

namespace Staffinfo.Divers.Data.Repositories
{
    public abstract class DapperRepository
    {
        /// <summary>
        /// Connection string to database
        /// </summary>
        public string ConnectionString { get; }

        public DapperRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Connection to database
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(ConnectionString);
            }
        }
    }
}
