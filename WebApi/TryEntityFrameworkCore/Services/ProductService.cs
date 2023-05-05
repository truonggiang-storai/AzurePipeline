using TryEntityFrameworkCore.Domains;
using TryEntityFrameworkCore.Repositories;
using WebApi.Responses;

namespace WebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Response<Product>> AddAsync(Product product)
        {
            try
            {
                await _productRepository.AddAsync(product);

                return new Response<Product>(product);
            }
            catch (Exception ex)
            {
                return new Response<Product>($"Something went wrong: {ex.Message}");
            }
        }

        public async Task<Response<Product>> FindByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return new Response<Product>("Data not found.");
            }

            return new Response<Product>(product);
        }

        public async Task<ICollection<Product>> ListAsync()
        {
            return await _productRepository.ListAsync();
        }

        public async Task<Response<Product>> RemoveAsync(int id)
        {
            var currentProduct = await _productRepository.GetByIdAsync(id);

            if (currentProduct == null)
            {
                return new Response<Product>("Data not found.");
            }

            try
            {
                await _productRepository.DeleteAsync(currentProduct);

                return new Response<Product>(currentProduct);
            }
            catch (Exception ex)
            {
                return new Response<Product>($"Something went wrong: {ex.Message}");
            }
        }

        public async Task<Response<Product>> UpdateAsync(int id, Product product)
        {
            var currentProduct = await _productRepository.GetByIdAsync(id);

            if (currentProduct == null)
            {
                return new Response<Product>("Data not found.");
            }

            product.Id = id;

            try
            {
                await _productRepository.UpdateAsync(product);

                return new Response<Product>(product);
            }
            catch (Exception ex)
            {
                return new Response<Product>($"Something went wrong: {ex.Message}");
            }
        }
    }
}
