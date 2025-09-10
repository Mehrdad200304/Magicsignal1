using MagicSignal.Modules.Accounts.Application.DTOs.Auth;
using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<User?> AuthenticateAsync(LoginRequest request);
    Task<AuthResult> RegisterAsync(RegisterRequest request);

}