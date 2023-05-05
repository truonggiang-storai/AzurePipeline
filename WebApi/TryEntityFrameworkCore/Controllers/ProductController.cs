using Microsoft.AspNetCore.Mvc;
using TryEntityFrameworkCore.Domains;
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
        public async Task<IEnumerable<Product>> ListAsync()
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
        public async Task<IActionResult> AddAsync([FromBody] Product product)
        {
            _logger.LogInformation("Create product.");

            var response = await _productService.AddAsync(product);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product product)
        {
            _logger.LogInformation("Update product.");

            _logger.LogInformation($"{product.Id} - {product.Name} - {product.Price}");

            var response = await _productService.UpdateAsync(id, product);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(product);
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
