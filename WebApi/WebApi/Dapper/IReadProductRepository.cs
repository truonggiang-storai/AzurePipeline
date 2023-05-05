using WebApi.Models;

namespace WebApi.Dapper
{
    public interface IReadProductRepository
    {
        public Task<ICollection<Product>> GetAllAsync();

        public Task<Product> GetAsync(int id);
    }
}
