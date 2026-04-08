using System;
using System.Threading.Tasks;
using FCG.Domain.Interfaces;

public interface IBaseRepository<T> where T : class, IBaseEntity
{
    Task<T?> GetById(Guid id);
    Task<T> Insert(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
}