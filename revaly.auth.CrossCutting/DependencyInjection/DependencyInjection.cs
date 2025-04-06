using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaultService.Extensions;

namespace revaly.auth.CrossCutting.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddVaultService(configuration);
            return services;
        }

        private static IServiceCollection AddVaultService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var vaultAddress = configuration["KeyVault:Address"] ?? throw new ArgumentNullException("KeyVault Address is missing");
            var vaultToken = configuration["KeyVault:Token"] ?? throw new ArgumentNullException("KeyVault Token is missing");

            return services.AddVaultService(
                vaultAddress: vaultAddress,
                vaultToken: vaultToken
            );
        }
    }
}
