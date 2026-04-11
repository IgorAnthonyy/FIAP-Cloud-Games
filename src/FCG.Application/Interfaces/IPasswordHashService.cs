using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Interfaces
{
    public interface IPasswordHashService
    {
        string GenerateHash(string password);
        bool VerifyPassword(string hash, string password);
    }
}
