using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Resourses;
using WebApi.Responses;
using WebApi.Services;

namespace UnitTest.Controllers
{
    public class TestProductController
    {
        private readonly ProductController _productController;

        private readonly Mock<IProductService> _productServiceMock;

        private readonly Mock<ILogger<ProductController>> _loggerMock;

        private readonly Mock<ProductPresentResource> _productMock;

        private readonly ICollection<ProductPresentResource> _products;

        public TestProductController()
        {
            _productServiceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _productController = new ProductController(_loggerMock.Object, _productServiceMock.Object);
            _productMock = new Mock<ProductPresentResource>();
            _products = new List<ProductPresentResource>()
            {
                _productMock.Object,
            };
        }

        [Fact]
        public async Task WebApi_ShouldGetListAsyncSuccessfully()
        {
            // arrange
            _productServiceMock.Setup(s => s.ListAsync()).Returns(Task.FromResult(_products));

            // act
            var result = await _productController.ListAsync();

            // asert
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Get list of products." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
            Assert.Equal(_products.AsEnumerable(), result);
        }

        [Fact]
        public async Task WebApi_ShouldFindByIdAsyncSuccessfully()
        {
            // arrange
            var successfulProductRepsonse = new Response<ProductPresentResource>(_productMock.Object);
            _productServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(successfulProductRepsonse));

            // act
            var result = await _productController.FindByIdAsync(1);
            var okResult = (OkObjectResult)result;

            // asert
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Get product by ID." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task WebApi_ShouldFindByIdAsyncFailed()
        {
            // arrange
            var failedProductRepsonse = new Response<ProductPresentResource>(It.IsAny<string>());
            _productServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(failedProductRepsonse));

            // act
            var result = await _productController.FindByIdAsync(1);
            var badRequestResult = (BadRequestObjectResult)result;

            // asert
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Get product by ID." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
