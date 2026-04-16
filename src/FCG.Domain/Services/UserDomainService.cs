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

    public async Task DeleteUser(Guid idUserToDeleted)
    {

        User userToDeleted = await _userRepository.GetById(idUserToDeleted);

        if (userToDeleted == null) throw new BusinessException("Usuário a ser deletado não existe");

        _userRepository.Delete(userToDeleted);

    }
    
    public async Task<User> GetById(Guid id)
    {
        return await _userRepository.GetById(id);
    }
    
    public async Task<User> GetByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }
    
    public async Task<User> UpdateUser(User user)
    {
        User userToUpdate = await _userRepository.GetById(user.Id);
        if (userToUpdate == null)
            throw new BusinessException("Usuário não encontrado");

        userToUpdate.Name = user.Name;
        userToUpdate.Phone = user.Phone;
        userToUpdate.BirthDate = user.BirthDate;

        _userRepository.Update(userToUpdate);

        return userToUpdate;
    }
}