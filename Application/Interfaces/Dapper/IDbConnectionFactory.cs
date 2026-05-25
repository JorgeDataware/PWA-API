using System.Data;

namespace PWA_API.Application.Interfaces.Dapper;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
