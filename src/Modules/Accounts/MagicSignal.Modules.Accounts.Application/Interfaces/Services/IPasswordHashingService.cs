namespace MagicSignal.Modules.Accounts.Application.Interfaces.Services;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}