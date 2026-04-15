using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class AcessService : IAcessService
    {
        private readonly ITokenService _tokenService;
        private readonly IAcessDomainService _acessDomainService;
        public AcessService(ITokenService tokenService, IAcessDomainService acessDomainService)
        {
            _tokenService = tokenService;
            _acessDomainService = acessDomainService;
        }

        public async Task<string> Login(AcessLogin login)
        {
            var email = new Email(login.Email);
            var password = new Password(login.Password);

            User userTryingLogin = await _acessDomainService.Login(email, password);

            return _tokenService.GenerateTokenJWT(userTryingLogin);
        }
    }
}
