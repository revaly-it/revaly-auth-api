using Microsoft.EntityFrameworkCore;
using revaly.auth.Domain.Entities;
using revaly.auth.Domain.Interfaces.Repositories.IUserRepository;
using revaly.auth.Infrastructure.Context;

namespace revaly.auth.Infrastructure.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLContext _mySQLContext;

        public UserRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _mySQLContext.Users.FindAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _mySQLContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _mySQLContext.Users.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _mySQLContext.Users.AddAsync(user);
            await _mySQLContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _mySQLContext.Users.Update(user);
            await _mySQLContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _mySQLContext.Users.Remove(user);
                await _mySQLContext.SaveChangesAsync();
            }
        }
    }
}
