using WebApi.Models;
using WebApi.Resourses;
using WebApi.Responses;

namespace WebApi.Services
{
    public interface IProductService
    {
        Task<ICollection<ProductPresentResource>> ListAsync();

        Task<Response<ProductPresentResource>> FindByIdAsync(int id);

        Task<Response<ProductPresentResource>> AddAsync(Product product);

        Task<Response<ProductPresentResource>> UpdateAsync(int id, Product product);

        Task<Response<ProductPresentResource>> RemoveAsync(int id);
    }
}
