using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Views;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FCG.Application.Services;

public class UserService : BaseApplicationService, IUserService
{
    private readonly IEmailService _emailService;
    private readonly IUserDomainService _userDomainService;
    private readonly IMapper _mapper;
    private readonly IUserLogged _userLogged;
    private readonly IPasswordService _passwordService;

    public UserService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailService emailService,
        IUserDomainService userDomainService,
        IUserLogged userLogged,
        IPasswordService passwordService) : base(unitOfWork)
    {
        _mapper = mapper;
        _emailService = emailService;
        _userDomainService = userDomainService;
        _userLogged = userLogged;
        _passwordService = passwordService;
    }

    public async Task<UserResponse> CreateUser(UserCreate user)
    {
        var userMapped = _mapper.Map<User>(user);
        var insertedUser = await _userDomainService.Create(userMapped, FCGConstant.UserDefault);
        await UnitOfWork.CommitAsync();

        var userResponse = _mapper.Map<UserResponse>(insertedUser);
        var userView = _mapper.Map<UserView>(insertedUser);

        await _emailService.SendAsync(userView);

        return userResponse;
    }

    public async Task<UserResponse> CreateAdmin(AdminCreate user)
    {
        var temporaryPassword = GenerateTemporaryPassword();

        var userMapped = _mapper.Map<User>(user);
        userMapped.Password = temporaryPassword;
        
        var insertedUser = await _userDomainService.Create(userMapped, FCGConstant.AdminRole);
        await UnitOfWork.CommitAsync();

        var userResponse = _mapper.Map<UserResponse>(insertedUser);
        var userView = _mapper.Map<UserView>(insertedUser);

        await _emailService.SendAsync(userView, temporaryPassword, Domain.Enums.EmailOptions.Admin);

        return userResponse;
    }
    
    public async Task<UserResponse> UpdateUser(UserUpdate user)
    {
        var loggedId = _userLogged.UserId;

        var userToUpdate = await _userDomainService.GetById(user.Id);

        bool isAdmin = _userLogged.IsAdmin;

        if (!isAdmin && loggedId != user.Id)
            throw new BusinessException("Você não tem permissão para editar este usuário");

        if (user.Situation.HasValue && !isAdmin)
            throw new BusinessException("Apenas administradores podem alterar a situação do usuário");

        userToUpdate.Name = user.Name;
        userToUpdate.Phone = user.Phone;
        userToUpdate.BirthDate = user.BirthDate;

        if (isAdmin && user.Situation.HasValue)
            userToUpdate.Situation = user.Situation.Value;

        await _userDomainService.Update(userToUpdate);
        await UnitOfWork.CommitAsync();

        return _mapper.Map<UserResponse>(userToUpdate);
    }

    public async Task ChangePassword(RequestChangePassword request)
    {
        var loggedId = _userLogged.UserId;
        var loggedUser = await _userDomainService.GetById(loggedId);

        var passwordMatch = _passwordService.VerifyPassword(loggedUser.Password, request.Password);

        if (!passwordMatch)
            throw new BusinessException("A senha inserida é diferente da senha atual");

        await _userDomainService.ChangePassword(loggedId, request.NewPassword);
        await UnitOfWork.CommitAsync();
    }

    public async Task DeleteUser(Guid idUserToDeleted)
    {
        if (idUserToDeleted == _userLogged.UserId) throw new BusinessException("Você está tentando se apagar do sistema, caso queira que isso aconteça solicite isso para algum administrador");

        if (!_userLogged.IsAdmin)
            throw new UnauthorizedAccessException("Apenas administradores podem deletar usuários");

        await _userDomainService.Delete(idUserToDeleted);
        await UnitOfWork.CommitAsync();
    }

    private static string GenerateTemporaryPassword(int length = 12)
    {
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string numbers = "0123456789";
        const string symbols = "!@#$%&*";
        var allChars = upper + lower + numbers + symbols;

        var chars = new List<char>
        {
            upper[RandomNumberGenerator.GetInt32(upper.Length)],
            lower[RandomNumberGenerator.GetInt32(lower.Length)],
            numbers[RandomNumberGenerator.GetInt32(numbers.Length)],
            symbols[RandomNumberGenerator.GetInt32(symbols.Length)]
        };

        for (var i = chars.Count; i < length; i++)
        {
            chars.Add(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);
        }

        for (var i = chars.Count - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars.ToArray());
    }
}
