using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers
{
    [Route("/users")]
    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into UserController");
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> ListAsync()
        {
            _logger.LogInformation("Get list of users.");

            return await _userService.ListAsync();
        }

        [HttpPost("ids")]
        public async Task<IEnumerable<User>> ListByIdsAsync([FromBody] List<long> ids)
        {
            _logger.LogInformation("Get list of users by IDs.");

            return await _userService.ListByIdsAsync(ids);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            _logger.LogInformation("Get user by ID.");

            var response = await _userService.FindByIdAsync(id);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Resource);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] User user)
        {
            _logger.LogInformation("Create user.");

            var response = await _userService.AddAsync(user);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] User user)
        {
            _logger.LogInformation("Update user.");

            var response = await _userService.UpdateAsync(id, user);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            _logger.LogInformation("Delete user.");

            var response = await _userService.RemoveAsync(id);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Resource);
        }
    }
}
