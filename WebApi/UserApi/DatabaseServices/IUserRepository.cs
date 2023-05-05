using UserApi.Models;

namespace UserApi.DatabaseServices
{
    public interface IUserRepository
    {
        Task<ICollection<User>> ListAsync();
        Task<ICollection<User>> FindUsersByIdsAsync(List<long> ids);
        Task AddAsync(User user);
        Task<User?> FindByIdAsync(int id);
        void Update(User user);
        void Remove(User user);
    }
}
