using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Infrastructure.Helper
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly PasswordHasher<object> _hash = new();
        public string GenerateHash(string password)
        {
            return _hash.HashPassword(null, password);
        }

        public bool VerifyPassword(string hash, string password)
        {
            var result = _hash.VerifyHashedPassword(null, hash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
