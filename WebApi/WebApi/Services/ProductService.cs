using WebApi.Dapper;
using WebApi.Models;
using WebApi.Resourses;
using WebApi.Responses;
using WebApi.Services.UserApi;

namespace WebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IReadProductRepository _readProductRepository;

        private readonly IWriteProductRepository _writeProductRepository;

        private readonly IUserApiService _userApiService;

        public ProductService(
            IReadProductRepository readProductRepository, 
            IWriteProductRepository writeProductRepository,
            IUserApiService userApiService)
        {
            _readProductRepository = readProductRepository;
            _writeProductRepository = writeProductRepository;
            _userApiService = userApiService;
        }

        public async Task<Response<ProductPresentResource>> AddAsync(Product product)
        {
            var rowEffected = await _writeProductRepository.AddAsync(product);

            if (rowEffected <= 0)
            {
                return new Response<ProductPresentResource>("Insert failed.");
            }

            return new Response<ProductPresentResource>(await GetSingleResponse(product));
        }

        public async Task<Response<ProductPresentResource>> FindByIdAsync(int id)
        {
            var product = await _readProductRepository.GetAsync(id);

            if (product == null)
            {
                return new Response<ProductPresentResource>("Data not found.");
            }

            return new Response<ProductPresentResource>(await GetSingleResponse(product));
        }

        public async Task<ICollection<ProductPresentResource>> ListAsync()
        {
            var products = await _readProductRepository.GetAllAsync();

            var userIds = products.Select(product => product.CreatedById).ToList();

            var users = await _userApiService.ListUsersByIdsAsync(userIds);

            var indexedUsers = users.ToDictionary(user => user.Id, user => user);

            var response = new List<ProductPresentResource>();

            foreach (var product in products)
            {
                User createdBy = default;
                indexedUsers.TryGetValue(product.CreatedById, out createdBy);
                response.Add(new ProductPresentResource
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    CreatedBy = createdBy
                });
            }

            return response;
        }

        public async Task<Response<ProductPresentResource>> RemoveAsync(int id)
        {
            var currentProduct = await _readProductRepository.GetAsync(id);

            if (currentProduct == null)
            {
                return new Response<ProductPresentResource>("Data not found.");
            }

            var isDeleted = await _writeProductRepository.DeleteAsync(id);

            if (!isDeleted)
            {
                return new Response<ProductPresentResource>("Delete failed.");
            }

            return new Response<ProductPresentResource>(await GetSingleResponse(currentProduct));
        }

        public async Task<Response<ProductPresentResource>> UpdateAsync(int id, Product product)
        {
            var currentProduct = await _readProductRepository.GetAsync(id);

            if (currentProduct == null)
            {
                return new Response<ProductPresentResource>("Data not found.");
            }

            currentProduct.Name = product.Name;
            currentProduct.Price = product.Price;

            var isUpdated = await _writeProductRepository.UpdateAsync(currentProduct);

            if (!isUpdated)
            {
                return new Response<ProductPresentResource>("Update failed.");
            }

            return new Response<ProductPresentResource>(await GetSingleResponse(currentProduct));
        }

        private async Task<ProductPresentResource> GetSingleResponse(Product product)
        {
            var users = await _userApiService.ListUsersByIdsAsync(new List<long> { product.CreatedById });

            return new ProductPresentResource
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedBy = users.FirstOrDefault()
            };
        }
    }
}
