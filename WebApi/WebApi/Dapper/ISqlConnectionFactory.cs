using System.Data;

namespace WebApi.Dapper
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();

        IDbConnection GetNewConnection();
    }
}
