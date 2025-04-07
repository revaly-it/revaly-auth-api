
using revaly.auth.Domain.Entities;

namespace revaly.auth.Domain.Interfaces.Repositories.IUserRepository
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<bool> UserExistsAsync(Guid id);
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
    }
}
