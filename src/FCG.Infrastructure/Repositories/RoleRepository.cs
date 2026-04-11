using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext context) : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<IEnumerable<Role>> GetByUserId(Guid userId)
    {
        return await BaseQuery()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

}