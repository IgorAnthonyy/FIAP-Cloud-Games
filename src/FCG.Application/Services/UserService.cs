using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Views;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FCG.Application.Services;

public class UserService : BaseApplicationService, IUserService
{
    private readonly IEmailService _emailService;
    private readonly IUserDomainService _userDomainService;
    private readonly IMapper _mapper;
    private readonly IUserLogged _userLogged;
    public UserService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailService emailService,
        IUserDomainService userDomainService,
        IUserLogged userLogged) : base(unitOfWork)
    {
        _mapper = mapper;
        _emailService = emailService;
        _userDomainService = userDomainService;
        _userLogged = userLogged;
    }

    public async Task<UserResponse> CreateUser(UserCreate user)
    {
        var userMapped = _mapper.Map<User>(user);
        var insertedUser = await _userDomainService.CreateUser(userMapped, FCGConstant.UserDefault);
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
        var insertedUser = await _userDomainService.CreateUser(userMapped, FCGConstant.AdminRole);
        await UnitOfWork.CommitAsync();

        var userResponse = _mapper.Map<UserResponse>(insertedUser);
        var userView = _mapper.Map<UserView>(insertedUser);

        await _emailService.SendAsync(userView, temporaryPassword, Domain.Enums.EmailOptions.Admin);

        return userResponse;
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

    public async Task<bool> DeleteUser(Guid idUserToDeleted)
    {
        bool canDelete = await _userDomainService.DeleteUser(idUserToDeleted);
        if (canDelete)
        {
            await UnitOfWork.CommitAsync();
            return canDelete;
        }
        return canDelete;


    }
}
