namespace FCG.Domain.Interfaces;

public interface IPasswordService
{
    string GenerateHash(string password);
    bool VerifyPassword(string hash, string password);
}