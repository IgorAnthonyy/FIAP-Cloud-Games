using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Interfaces
{
    public interface IUserLogged
    {
        string UserId{ get; }

        List<Role> Roles { get; }

        bool IsAdmin { get; }
    }
}
