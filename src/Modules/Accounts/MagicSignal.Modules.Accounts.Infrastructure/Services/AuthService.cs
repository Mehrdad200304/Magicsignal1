using MagicSignal.Modules.Accounts.Application.DTOs.Auth;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Application.Interfaces.Authentication;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MagicSignal.Modules.Accounts.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHashingService _passwordHashingService;

    public AuthService(
        ApplicationDbContext context,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHashingService passwordHashingService)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHashingService = passwordHashingService;
    }

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

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Username, "Admin");

        return new AuthResult
        {
            IsSuccess = true,
            Token = token,
            Message = "ورود با موفقیت انجام شد"
        };
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
{
    var existingUser = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

    if (existingUser != null)
    {
        return new AuthResult
        {
            IsSuccess = false,
            Message = "نام کاربری یا ایمیل قبلاً ثبت شده است."
        };
    }

    var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
    if (userRole == null)
    {
        return new AuthResult
        {
            IsSuccess = false,
            Message = "نقش کاربر یافت نشد."
        };
    }

    var hashedPassword = _passwordHashingService.HashPassword(request.Password);

    var newUser = new User
    {
        Username = request.Username,
        Email = request.Email,
        PasswordHash = hashedPassword,
        RoleId = userRole.Id // نقش پیش‌فرض
    };

    _context.Users.Add(newUser);
    await _context.SaveChangesAsync();

    var token = _jwtTokenGenerator.GenerateToken(newUser.Id, newUser.Username, "Admin");

    return new AuthResult
    {
        IsSuccess = true,
        Token = token,
        Message = "ثبت‌نام با موفقیت انجام شد"
    };
}

    // متد کمکی برای احراز هویت (اختیاری)
    public async Task<User?> AuthenticateAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user != null && _passwordHashingService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }
}
