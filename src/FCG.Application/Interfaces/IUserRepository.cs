using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<IEnumerable<User>> GetAll();

    Task<User> Insert(User user);
    User Update(User user);
    User Delete(User user);
}