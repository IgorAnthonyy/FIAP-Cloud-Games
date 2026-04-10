using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<IEnumerable<Role>> GetByUserId(Guid userId);
}