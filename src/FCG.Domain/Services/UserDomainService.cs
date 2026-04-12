using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.IService;
using FCG.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace FCG.Domain.Services;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;

    public UserDomainService(IUserRepository userRepository, IPasswordHashService passwordHashService)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
    }


    public async Task<User> CreateUser(User user, string role)
    {

        var userSearch = await _userRepository.GetByEmail(user.Email.Value);

        if (userSearch != null) throw new Exception("Usuário encontrado");
        var password = new Password(user.Password);

        user.CreateUser(user, new Role(role));
        user.Password = _passwordHashService.GenerateHash(password.Value);
        User insertedUser = await _userRepository.Insert(user);

        return insertedUser;
    }
}
