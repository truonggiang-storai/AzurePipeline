using WebApi.Models;

namespace WebApi.Dapper
{
    public class ReadProductRepository : DapperRepository<Product>, IReadProductRepository
    {
        public ReadProductRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            var sql = @"SELECT * FROM Product";

            return await QueryAsync<Product>(sql);
        }

        public async Task<Product> GetAsync(int id)
        {
            var sql = @"SELECT * FROM Product
                            WHERE id = @id";

            return await QuerySingleOrDefaultAsync<Product>(sql, new { id = id });
        }
    }
}
