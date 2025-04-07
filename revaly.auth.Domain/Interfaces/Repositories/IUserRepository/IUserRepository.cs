
using revaly.auth.Domain.Entities;

namespace revaly.auth.Domain.Interfaces.Repositories.IUserRepository
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
