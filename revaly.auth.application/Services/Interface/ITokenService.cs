using revaly.auth.Domain.Entities;

namespace revaly.auth.Application.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
