using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Resourses;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("/products")]
    public class ProductController : BaseApiController
    {
        private readonly ILogger<ProductController> _logger;

        private readonly IProductService _productService;

        public ProductController(
            ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into ProductController");
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductPresentResource>> ListAsync()
        {
            _logger.LogInformation("Get list of products.");

            return await _productService.ListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            _logger.LogInformation("Get product by ID.");

            var response = await _productService.FindByIdAsync(id);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Resource);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductResource resource)
        {
            _logger.LogInformation("Create product.");

            var product = new Product
            {
                Name = resource.Name,
                Price = resource.Price,
                CreatedById = resource.CreatedById
            };

            var response = await _productService.AddAsync(product);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Resource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductResource resource)
        {
            _logger.LogInformation("Update product.");

            var product = new Product
            {
                Name = resource.Name,
                Price = resource.Price
            };

            var response = await _productService.UpdateAsync(id, product);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Resource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            _logger.LogInformation("Delete product.");

            var response = await _productService.RemoveAsync(id);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Resource);
        }
    }
}
