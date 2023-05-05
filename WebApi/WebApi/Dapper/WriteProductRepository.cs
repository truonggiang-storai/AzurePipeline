using Dapper;
using System.Data;
using WebApi.Models;

namespace WebApi.Dapper
{
    public class WriteProductRepository : DapperRepository<Product>, IWriteProductRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public WriteProductRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
        {
            _sqlConnectionFactory = connectionFactory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var sql = @"DELETE FROM Product
                            WHERE id = @id";

                int rowAffected = await connection.ExecuteAsync(sql, new { id });

                return rowAffected > 0;
            }
        }
    }
}
