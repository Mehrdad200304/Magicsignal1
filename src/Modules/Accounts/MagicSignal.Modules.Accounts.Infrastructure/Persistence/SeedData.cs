using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Domain.Entities;
using BCrypt.Net;

namespace MagicSignal.Modules.Accounts.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // مطمئن شو که دیتابیس ساخته شده
        await context.Database.EnsureCreatedAsync();

        // اگه رول ها وجود ندارن، بساز
        if (!context.Roles.Any())
        {
            var roles = new[]
            {
                new Role 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Admin", 
                    Description = "مدیر سیستم با دسترسی کامل",
                    CreatedAt = DateTime.UtcNow
                },
                new Role 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "VipUser", 
                    Description = "کاربر ویژه با دسترسی به Vip",
                    CreatedAt = DateTime.UtcNow
                },
                new Role 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "User", 
                    Description = "کاربر عادی",
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }

        // اگه هیچ کاربری وجود نداره، یک ادمین بساز
        if (!context.Users.Any())
        {
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                RoleId = adminRole.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}