using FCG.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IUserDomainService
{
    Task<User> CreateUser(User user, string role);

    Task<bool> DeleteUser(Guid idUserToDeleted);
}