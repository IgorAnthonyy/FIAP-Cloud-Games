using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces.IService
{
    public interface IUserDomainService
    {
        Task<User> CreateUser(User user, string role);
    }
}
