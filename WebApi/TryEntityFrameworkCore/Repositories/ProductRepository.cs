using Microsoft.EntityFrameworkCore;
using TryEntityFrameworkCore.Domains;

namespace TryEntityFrameworkCore.Repositories
{
    public class ProductRepository : EfRepository<DBContext, Product, long>, IProductRepository
    {
        public ProductRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbContext.Products
                .Where(product => (product.Price >= minPrice 
                                && product.Price <= maxPrice))
                .ToListAsync();
        }
    }
}
