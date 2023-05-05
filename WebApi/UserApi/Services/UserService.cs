using UserApi.DatabaseServices;
using UserApi.Models;
using UserApi.Responses;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<User>> AddAsync(User user)
        {
            try
            {
                await _userRepository.AddAsync(user);

                return new Response<User>(user);
            }
            catch (Exception ex)
            {
                return new Response<User>($"Something went wrong: {ex.Message}");
            }
        }

        public async Task<Response<User>> FindByIdAsync(int id)
        {
            var currentUser = await _userRepository.FindByIdAsync(id);

            if (currentUser == null)
            {
                return new Response<User>("Data not found.");
            }

            return new Response<User>(currentUser);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync();
        }

        public async Task<IEnumerable<User>> ListByIdsAsync(List<long> ids)
        {
            return await _userRepository.FindUsersByIdsAsync(ids);
        }

        public async Task<Response<User>> RemoveAsync(int id)
        {
            var currentUser = await _userRepository.FindByIdAsync(id);

            if (currentUser == null)
            {
                return new Response<User>("Data not found.");
            }

            try
            {
                _userRepository.Remove(currentUser);

                return new Response<User>(currentUser);
            }
            catch (Exception ex)
            {
                return new Response<User>($"Something went wrong: {ex.Message}");
            }
        }

        public async Task<Response<User>> UpdateAsync(int id, User user)
        {
            var currentUser = await _userRepository.FindByIdAsync(id);

            if (currentUser == null)
            {
                return new Response<User>("Data not found.");
            }

            currentUser.Name = user.Name;

            try
            {
                _userRepository.Update(currentUser);

                return new Response<User>(currentUser);
            }
            catch (Exception ex)
            {
                return new Response<User>($"Something went wrong: {ex.Message}");
            }
        }
    }
}
