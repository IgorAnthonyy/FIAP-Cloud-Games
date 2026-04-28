using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.ValueObjects;
using System.Threading.Tasks;

namespace FCG.Application.Services;

public class AccessService : IAccessService
{
    private readonly ITokenService _tokenService;
    private readonly IAccessDomainService _accessDomainService;
    public AccessService(ITokenService tokenService, IAccessDomainService accessDomainService)
    {
        _tokenService = tokenService;
        _accessDomainService = accessDomainService;
    }

    public async Task<string> Login(AccessLogin login)
    {
        var email = new Email(login.Email);
        var password = new Password(login.Password);

        User userTryingLogin = await _accessDomainService.Login(email, password);

        return _tokenService.GenerateTokenJWT(userTryingLogin);
    }
}
