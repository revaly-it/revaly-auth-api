using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using revaly.auth.Infrastructure.Context;
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
                .AddMySQLDatabase(configuration);

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
        
    }
}
