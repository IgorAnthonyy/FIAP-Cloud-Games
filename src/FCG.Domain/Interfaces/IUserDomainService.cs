using FCG.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IUserDomainService
{
    Task<User> Create(User user, string role);
    Task Delete(Guid idUserToDeleted);
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<User> Update(User user);
    Task ChangePassword(Guid idUser, string newPassword);

}