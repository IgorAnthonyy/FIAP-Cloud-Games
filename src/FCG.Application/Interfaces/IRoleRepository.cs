using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetByUserId(Guid userId);

    Task<Role> Insert(Role role);
    Role Delete(Role role);
}