using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using revaly.auth.Infrastructure.Context;
using System.Text;
using VaultService.Extensions;
using VaultService.Interface;

namespace revaly.auth.CrossCutting.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddVaultService(configuration)
                .AddMySQLDatabase(configuration)
                .AddCors();

            return services;
        }

        private static IServiceCollection AddVaultService(this IServiceCollection services, IConfiguration configuration)
        {
            var vaultAddress = configuration["KeyVault:Address"] ?? throw new ArgumentNullException("KeyVault Address is missing");
            var vaultToken = configuration["KeyVault:Token"] ?? throw new ArgumentNullException("KeyVault Token is missing");

            return services.AddVaultService(
                vaultAddress: vaultAddress,
                vaultToken: vaultToken
            );
        }

        private static IServiceCollection AddMySQLDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MySQLContext>((provider, options) =>
            {
                var vaultService = provider.GetRequiredService<IVaultClient>();

                var connectionStringKey = configuration["KeyVaultSecrets:Database:ConnectionString"] ?? throw new ArgumentNullException("Connection string is missing");

                var connectionString = vaultService.GetSecret(connectionStringKey);


                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            });
            return services;
        }

        private static IServiceCollection AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IVaultClient>((options, vaultService) =>
                {
                    var jwtSecretKey = configuration["KeyVaultSecrets:JwtSecret"]
                        ?? throw new ArgumentNullException("JwtSecret is missing in Vault");

                    var jwtSecret = vaultService.GetSecret(jwtSecretKey);

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
