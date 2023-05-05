using Microsoft.EntityFrameworkCore;
using System.Linq;
using UserApi.Models;

namespace UserApi.DatabaseServices
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> ListAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.Id == id);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task<ICollection<User>> FindUsersByIdsAsync(List<long> ids)
        {
            return await _context.Users
                .Where(user => ids.Contains(user.Id))
                .ToListAsync();
        }
    }
}
