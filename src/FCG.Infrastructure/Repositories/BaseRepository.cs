using System;
using System.Linq;
using System.Threading.Tasks;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class, IBaseEntity
{
    protected ApplicationDbContext _context = context;

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

    public async Task<T> Insert(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }
}