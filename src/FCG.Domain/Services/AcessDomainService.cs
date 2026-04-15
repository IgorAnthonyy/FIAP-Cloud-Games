using FCG.Domain.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Services
{
    public class AcessDomainService : IAcessDomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        
        public AcessDomainService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<User> Login(Email email, Password password)
        {
            User userTryingLogin = (await _userRepository.GetByEmail(email.Value)) ?? throw new BusinessException("Usuário que tentou logar não encontrado");

            if (!_passwordService.VerifyPassword(userTryingLogin.Password, password.Value)) throw new BusinessException("Senha inválida");

            return userTryingLogin;

        }

        
    }
}
