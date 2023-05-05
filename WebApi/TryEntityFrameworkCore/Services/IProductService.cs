using TryEntityFrameworkCore.Domains;
using WebApi.Responses;

namespace WebApi.Services
{
    public interface IProductService
    {
        Task<ICollection<Product>> ListAsync();

        Task<Response<Product>> FindByIdAsync(int id);

        Task<Response<Product>> AddAsync(Product product);

        Task<Response<Product>> UpdateAsync(int id, Product product);

        Task<Response<Product>> RemoveAsync(int id);
    }
}
