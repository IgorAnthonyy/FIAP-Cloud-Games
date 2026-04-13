using FCG.Application.Interfaces;

namespace FCG.Infrastructure.Password;

public class PasswordHashService : IPasswordHashService
{
    public string GenerateHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        var result = BCrypt.Net.BCrypt.Verify(password, hash);
        return result;
    }
}
