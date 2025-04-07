using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using revaly.auth.Application.Commands.AuthCommand.RegisterUserCommand;
using revaly.auth.Application.Handlers.AuthCommandHandler.RegisterUserCommandHandler;
using revaly.auth.Application.Services;
using revaly.auth.Application.Services.Interface;
using revaly.auth.Domain.Interfaces.IUnitOfWork;
using revaly.auth.Domain.Interfaces.Repositories.IUserRepository;
using revaly.auth.Infrastructure.Context;
using revaly.auth.Infrastructure.Persistence;
using revaly.auth.Infrastructure.Repositories.UserRepository;
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
                .AddCors()
                .AddRepositories()
                .AddUnitOfWork()
                .AddServices()
                .AddHandlers()
                .AddValidation();

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

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<ITokenService, TokenService>();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommand>());
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            return services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<RegisterUserCommand>();
        }
    }
}
