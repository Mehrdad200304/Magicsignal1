using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Application.Interfaces.Authentication;
using MagicSignal.Modules.Accounts.Infrastructure.Services;
using MagicSignal.Modules.Accounts.Infrastructure.Services;
using MagicSignal.Modules.Accounts.Application.Interfaces.Repositories;
using MagicSignal.Modules.Accounts.Infrastructure.Repositories;

namespace MagicSignal.Modules.Accounts.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        // ثبت AuthService
        services.AddScoped<IAuthService, AuthService>();
    
        // ثبت JWT Token Generator
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    
        // ثبت Password Hashing Service
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        
        // ثبت Repositoryهای ماژول Accounts (دسترسی به دیتابیس برای مقالات و دسته‌بندی‌ها)
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}