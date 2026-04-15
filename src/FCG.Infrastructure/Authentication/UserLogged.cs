using FCG.Application.Interfaces;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FCG.Infrastructure.Authentication
{
    public class UserLogged : IUserLogged
    {

        private readonly IHttpContextAccessor _httpContextAcessor;

        public UserLogged(IHttpContextAccessor httpContextAcessor)
        {
            _httpContextAcessor = httpContextAcessor;
        }

        public Guid UserId => new (_httpContextAcessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public List<Role> Roles => _httpContextAcessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(r => new Role
        {
            Name = r.Value,
        }).ToList();

        public bool IsAdmin => _httpContextAcessor.HttpContext.User.FindAll(ClaimTypes.Role).Any(r => r.Value == FCGConstant.AdminRole);
    }
}
