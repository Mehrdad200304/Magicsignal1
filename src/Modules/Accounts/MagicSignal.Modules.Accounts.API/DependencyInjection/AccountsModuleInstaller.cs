using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Application.DependencyInjection;
using MagicSignal.Modules.Accounts.Infrastructure.DependencyInjection;
//using MagicSignal.Modules.Accounts.Application.DependencyInjection;
//using MagicSignal.Modules.Accounts.Infrastructure.DependencyInjection;

namespace MagicSignal.Modules.Accounts.API.DependencyInjection;

public static class AccountsModuleInstaller
{
    public static IServiceCollection AddAccountsModule(this IServiceCollection services)
    {
        services
            .AddApplicationLayer()
            .AddInfrastructureLayer();

        return services;
    }
}