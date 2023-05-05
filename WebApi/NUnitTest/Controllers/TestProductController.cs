using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Resourses;
using WebApi.Responses;
using WebApi.Services;

namespace NUnitTest.Controllers
{
    [TestFixture]
    public class TestProductController
    {
        private ProductController productController;

        private Mock<IProductService> productServiceMock;

        private Mock<ILogger<ProductController>> loggerMock;

        private Mock<ProductPresentResource> productMock;

        private ICollection<ProductPresentResource> products;

        [SetUp]
        public void SetUp()
        {
            productServiceMock = new Mock<IProductService>();
            loggerMock = new Mock<ILogger<ProductController>>();
            productController = new ProductController(loggerMock.Object, productServiceMock.Object);
            productMock = new Mock<ProductPresentResource>();
            products = new List<ProductPresentResource>()
            {
                productMock.Object,
            };
        }

        [Test]
        public async Task WebApiShouldGetListAsyncSuccessfully()
        {
            // arrange
            productServiceMock.Setup(s => s.ListAsync()).Returns(Task.FromResult(products));

            // act
            var result = await productController.ListAsync();

            // asert
            loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Get list of products." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
            Assert.AreEqual(products.AsEnumerable(), result);
        }

        [Test]
        public async Task WebApiShouldFindByIdAsyncSuccessfully()
        {
            // arrange
            var successfulProductRepsonse = new Response<ProductPresentResource>(productMock.Object);
            productServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(successfulProductRepsonse));

            // act
            var result = await productController.FindByIdAsync(1);
            var okResult = (OkObjectResult)result;

            // asert
            loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Get product by ID." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            okResult.Value.Should().Be(productMock.Object);
        }

        [Test]
        public async Task WebApiShouldFindByIdAsyncFailed()
        {
            // arrange
            var failedProductRepsonse = new Response<ProductPresentResource>(It.IsAny<string>());
            productServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(failedProductRepsonse));

            // act
            var result = await productController.FindByIdAsync(1);
            var badRequestResult = (BadRequestObjectResult)result;

            // asert
            loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Get product by ID." && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            badRequestResult.Value.Should().BeNull();
        }
    }
}
