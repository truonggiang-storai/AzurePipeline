using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Models;
using UserApi.Services;
using Xunit;

namespace IntegrationTest.BusinessServices
{
    public class UserApiServiceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UserApiServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddAsync_ShouldRunCorrectly()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var userData = new User
                {
                    Name = "Test user"
                };

                // Act
                var response = await userService.AddAsync(userData);

                // Assert
                response.Should().NotBeNull();

                var user = response.Resource;
                user.Should().NotBeNull();
                user.Name.Should().Be(userData.Name);
            }
        }



        [Fact]
        public async Task ListAsync_ShouldRunCorrectly()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var userData1 = new User { Name = "Test user 1" };
                var userData2 = new User { Name = "Test user 2" };
                var userData3 = new User { Name = "Test user 3" };

                // Act
                await userService.AddAsync(userData1);
                await userService.AddAsync(userData2);
                await userService.AddAsync(userData3);
                var response = await userService.ListAsync();

                // Assert
                response.Should().NotBeNull();

                response.Should().HaveCount(3);

                var expectedData = new List<User> { userData1, userData2, userData3 }.AsEnumerable();
                response.Should().Equal(expectedData);
            }
        }
    }
}
