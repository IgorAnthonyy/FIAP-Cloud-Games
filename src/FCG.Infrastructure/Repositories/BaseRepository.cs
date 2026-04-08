using System;
using System.Linq;
using System.Threading.Tasks;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
{
    public ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    protected IQueryable<TEntity> BaseQuery<TEntity>(bool tracking = false) where TEntity : class, IBaseEntity
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (tracking)
            query = query.AsTracking();
        else
            query = query.AsNoTracking();

        return query;
    }

    protected IQueryable<T> BaseQuery(bool tracking = false)
    {
        var query = _context.Set<T>().AsQueryable();

        if (tracking)
            query = query.AsTracking();
        else
            query = query.AsNoTracking();

        return query;
    }


    public Task<T?> GetById(Guid id)
    {
        return BaseQuery().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T> Insert(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        await Task.Run(() => _context.Set<T>().Update(entity));
        return entity;
    }

    public async Task<T> Delete(T entity)
    {
        await Task.Run(() => _context.Set<T>().Remove(entity));
        return entity;
    }
}