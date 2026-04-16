using FCG.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IUserDomainService
{
    Task<User> CreateUser(User user, string role);
    Task DeleteUser(Guid idUserToDeleted);
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<User> UpdateUser(User user);
}