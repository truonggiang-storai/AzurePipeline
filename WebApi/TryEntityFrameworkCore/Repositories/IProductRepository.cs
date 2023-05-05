using TryEntityFrameworkCore.Domains;

namespace TryEntityFrameworkCore.Repositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
        Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice);
    }
}
