using FCG.Domain.Interfaces;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Domain.Services;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordHashService;

    public UserDomainService(IUserRepository userRepository, IPasswordService passwordHashService)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
    }

    public async Task<User> CreateUser(User user, string role)
    {
        var userSearch = await _userRepository.GetByEmail(user.Email.Value);

        if (userSearch != null) throw new BusinessException("Usuário encontrado");

        var password = new Password(user.Password);

        user.CreateUser(user, new Role(role));
        user.Password = _passwordHashService.GenerateHash(password.Value);
        User insertedUser = await _userRepository.Insert(user);

        return insertedUser;
    }

    public async Task<bool> DeleteUser(Guid idUserToDeleted)
    {

        User userToDeleted = await _userRepository.GetById(idUserToDeleted);

        if (userToDeleted == null) throw new BusinessException("Usuário a ser deletado não existe");

        _userRepository.Delete(userToDeleted);

        return true;

    }
}
