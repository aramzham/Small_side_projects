using Microsoft.Extensions.DependencyInjection;
using WalletTask.BL;

namespace WalletTask.WebApi.Core.Extensions
{
    public static class BlServiceExtension
    {
        public static IServiceCollection AddWalletBL(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(x => new WalletTaskBL(connectionString));
            return services;
        }
    }
}