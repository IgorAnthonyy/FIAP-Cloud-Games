using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
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

        public string UserEmail => _httpContextAcessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }
}
