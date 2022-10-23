using Domain.Utils;
using Npgsql;

namespace Infrastructure.DBConfiguration.Dapper
{
    public class ConnectionFactory
    {
        internal static NpgsqlConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(EnvironmentManager.GetConnectionString());
            connection.Open();
            return connection;
        }
    }
}