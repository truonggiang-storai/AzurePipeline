using WebApi.Models;

namespace WebApi.Dapper
{
    public interface IWriteProductRepository : IGenericRepository<Product>
    {
        Task<bool> DeleteAsync(int id);
    }
}
