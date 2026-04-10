using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User> GetById(Guid id)
    {
        return await BaseQuery()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> GetByEmail(string email)
    {
        return await BaseQuery()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await BaseQuery()
            .Include(x => x.Roles)
            .ToListAsync();
    }
}