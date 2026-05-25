using System.Data;
using Npgsql;
using PWA_API.Application.Interfaces.Dapper;

namespace PWA_API.Infrastructure.Dapper;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}
