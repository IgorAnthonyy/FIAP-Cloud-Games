using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Infrastructure.Helper
{
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
}
