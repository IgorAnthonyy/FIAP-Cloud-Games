using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces.Respositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string email);
    Task<IEnumerable<User>> GetAll();
}