using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateTokenJWT(User userLogged);
    }
}
