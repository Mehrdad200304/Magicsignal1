using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Application.DependencyInjection;
using MagicSignal.Modules.Accounts.Infrastructure.DependencyInjection;

namespace MagicSignal.Modules.Accounts.DependencyInjection
{
    public static class AccountsModule
    {
        public static IServiceCollection AddAccountsModule(this IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(AccountsModule).Assembly);
            return services;
        }
    }
}