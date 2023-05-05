using WebApi.Models;

namespace WebApi.Services.UserApi
{
    public interface IUserApiService
    {
        Task<IEnumerable<User>> ListUsersByIdsAsync(List<long> ids);
    }
}
