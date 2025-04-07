using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using revaly.auth.Application.Services.Interface;
using revaly.auth.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VaultService.Interface;

namespace revaly.auth.Application.Services
{
    public class TokenService(IConfiguration configuration, IVaultClient vaultClient) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IVaultClient _vaultClient = vaultClient;

        public string GenerateToken(User user)
        {
            var jwtSecret = _configuration["KeyVaultSecrets:JwtSecret"] ?? throw new Exception();
            var secret = _vaultClient.GetSecret(jwtSecret);

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
