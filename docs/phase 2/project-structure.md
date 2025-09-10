✅ساختار های مورد نیاز روز ۱ تا ۲


ساختار کلاس Entity User

public class User
{
public Guid Id { get; set; }
public string Username { get; set; } = default!;
public string Email { get; set; } = default!;
public string PasswordHash { get; set; } = default!;
}

🟢ساخت کلاس Role Entity

`csharp
public class Role
{
public Guid Id { get; set; }
public string Name { get; set; } = default!; // Admin, VipUser, User
public string Description { get; set; } = default!;
public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

public ICollection<User> Users { get; set; } = new List<User>();
}

🟢ساختار کلاس UserRole Entity
using System;

namespace MagicSignal.Modules.Accounts.Domain.Entities;

public class UserRole
{
public Guid UserId { get; set; }
public User User { get; set; } = default!;
public Guid RoleId { get; set; }

public Role Role { get; set; } = default!;
}

🟢ساختار #تنظیمات Database

Connection String (appsettings.json)

`json
{
"ConnectionStrings": {
"DefaultConnection": "Server=localhost\\MSSQLSERVER01;Database=MagicSignalDb;Trusted_Connection=true;TrustServerCertificate=true;"
},
"Logging": {
"LogLevel": {
"Default": "Information",
"Microsoft.AspNetCore": "Warning"
}
},
"AllowedHosts": "*"
}


🟢ساختار پیکربندی DbContext

DbContext Configuration

`csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
base.OnModelCreating(modelBuilder);

    // User to Role relationship
    modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);
}


🟢ساختار # Seed Data

`csharp
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

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰
✅ساختار های مورد نیاز روز ۵ تا ۷:

🟢ساختار PasswordHashingService
sing MagicSignal.Modules.Accounts.Application.Interfaces.Services;

namespace MagicSignal.Modules.Accounts.Infrastructure.Services;

public class PasswordHashingService : IPasswordHashingService
{
   public string HashPassword(string password)
   {
     return BCrypt.Net.BCrypt.HashPassword(password);
   }

public bool VerifyPassword(string password, string hashedPassword)
    { 
         return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

🟢ساختار  اینترفیس IPasswordHashingService

Interface:
csharp
namespace MagicSignal.Modules.Accounts.Application.Interfaces.Services;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}


🟢 ثبت Dependency Injection
فایل تغییر یافته:

src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs


کد اضافه شده:
csharp
using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Application.Interfaces.Authentication;
using MagicSignal.Modules.Accounts.Infrastructure.Services;

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

        return services;
    }
}


نتیجه: ✅ سرویس هش پسورد در DI Container ثبت شد


🟢تغییر AuthService Constructor
فایل تغییر یافته:

src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/Services/AuthService.cs


تغییرات Constructor:

قبل:
csharp
public class AuthService : IAuthService
{
private readonly ApplicationDbContext _context;
private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(ApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
}

بعد:
csharp
public class AuthService : IAuthService
{
private readonly ApplicationDbContext _context;
private readonly IJwtTokenGenerator _jwtTokenGenerator;
private readonly IPasswordHashingService _passwordHashingService;

    public AuthService(ApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator, IPasswordHashingService passwordHashingService)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHashingService = passwordHashingService;
    }
}


🟢تغییر متد LoginAsync برای امنیت
کد ناامن قبلی:
csharp
public async Task<AuthResult> LoginAsync(LoginRequest request)
{
var user = await _context.Users
.Include(u => u.Role)
.FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.Password);

    if (user == null)
    {
        return new AuthResult
        {
            IsSuccess = false,
            Message = "نام کاربری یا رمز عبور اشتباه است."
        };
    }
    // ...
}


کد امن جدید:
csharp
public async Task<AuthResult> LoginAsync(LoginRequest request)
{
var user = await _context.Users
.Include(u => u.Role)
.FirstOrDefaultAsync(u => u.Username == request.Username);

    if (user == null || !_passwordHashingService.VerifyPassword(request.Password, user.PasswordHash))
    {
        return new AuthResult
        {
            IsSuccess = false,
            Message = "نام کاربری یا رمز عبور اشتباه است."
        };
    }

    var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Username, user.Role?.Name ?? "User");
    return new AuthResult
        IsSuccess = true,
        Token = token,
        Message = "ورود با موفقیت انجام شد"
    };
}

🟢تغییر SeedData برای Hash کردن
فایل تغییر یافته:

src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/Persistence/SeedData.cs


تغییرات:

۱. اضافه کردن using:
csharp
using BCrypt.Net;  // اضافه شد
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MagicSignal.Modules.Accounts.Domain.Entities;


۲. تغییر password پیش‌فرض:
csharp
// قبل (ناامن)
var adminUser = new User
{
Id = Guid.NewGuid(),
Username = "admin",
Email = "admin@example.com",
PasswordHash = "123456", // خطرناک!
RoleId = adminRole.Id,
CreatedAt = DateTime.UtcNow
};

// بعد (امن)
var adminUser = new User
{
Id = Guid.NewGuid(),
Username = "admin",
Email = "admin@example.com",
PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), // امن!
RoleId = adminRole.Id,
CreatedAt = DateTime.UtcNow
};

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز ۸ تا ۹:

🟢ساختار Article.cs
src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/Entitiesَ/Article.cs

🟢ساختار Category.cs
src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/Entitiesَ/Category.cs

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز ۱۰ تا ۱۲:

🟢ساختار ArticleController
src/Modules/Accounts/MagicSignal.Modules.Accounts.API/Controllers/ArticleController.cs

🟢ساختار CategoryController
src/Modules/Accounts/MagicSignal.Modules.Accounts.API/Controllers/CategoryController.cs

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز ۱۳ تا ۱۴:

📁 فایل‌های ایجاد/تغییر یافته
`
MagicSignal.API/
├── Program.cs ✅ (تغییر یافته - JWT Configuration)
├── appsettings.json ✅ (تغییر یافته - JWT Settings)
├── Controllers/
│  ├── AuthController.cs ✅ (موجود قبلی)
│  ├── ArticleController.cs ✅ (تغییر یافته - Authorization Attributes)
│  └── CategoryController.cs ✅ (تغییر یافته - Authorization Attributes)
└── Middleware/
  └── ExceptionHandlingMiddleware.cs ✅ (تغییر یافته - Anonymous Type Fix)

MagicSignal.Modules.Accounts.Domain/
└── Entities/
  ├── Article.cs ✅ (تغییر یافته - Validation Fix)
  └── Category.cs ✅ (تغییر یافته - Validation Fix)

MagicSignal.Modules.Accounts.Application/
└── Attributes/Validation/
       └── UniqueCategoryAttribute.cs ✅ (تغییر یافته - Service Resolution Fix)

اضافه کردن Authorization Attribute

تست Authorization

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز ۱۵

ساختار فایل TestClass:
tests/MagicSignal.Modules.Accounts.Tests/AuthServiceTests.cs