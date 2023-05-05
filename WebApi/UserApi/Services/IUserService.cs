using UserApi.Models;
using UserApi.Responses;

namespace UserApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> ListAsync();
        Task<IEnumerable<User>> ListByIdsAsync(List<long> ids);

        Task<Response<User>> FindByIdAsync(int id);

        Task<Response<User>> AddAsync(User user);

        Task<Response<User>> UpdateAsync(int id, User user);

        Task<Response<User>> RemoveAsync(int id);
    }
}
