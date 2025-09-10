namespace MagicSignal.Modules.Accounts.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string username, string role);
}