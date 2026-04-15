using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCG.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateTokenJWT(User userLogged)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userLogged.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, userLogged.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
            };
            foreach (var role in userLogged.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var keyJwt = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(keyJwt, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
