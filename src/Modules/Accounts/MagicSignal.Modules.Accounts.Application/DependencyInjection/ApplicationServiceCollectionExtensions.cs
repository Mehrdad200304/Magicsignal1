using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MagicSignal.Modules.Accounts.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // ثبت سرویس‌های کاربردی مدیریت محتوا
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}