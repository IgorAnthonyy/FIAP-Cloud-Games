using FCG.Application.Interfaces;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FCG.Infrastructure.Authentication;

public class UserLogged : IUserLogged
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserLogged(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal?.FindFirst("sub")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new UnauthorizedAccessException("Usuário autenticado inválido ou sem identificação.");
        }
    }

    public List<Role> Roles => [.. (_httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role) ?? [])
            .Select(r => new Role
            {
                Name = r.Value,
            })];

    public bool IsAdmin => (_httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role) ?? [])
        .Any(r => string.Equals(r.Value, FCGConstant.AdminRole, StringComparison.OrdinalIgnoreCase));
}
