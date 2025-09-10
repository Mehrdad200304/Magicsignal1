using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Infrastructure.Services;

namespace MagicSignal.Modules.Accounts.Infrastructure.DependencyInjection
{
    public static class AccountsServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services)
        {
            // اینجا سرویس Auth ثبت میشه
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}