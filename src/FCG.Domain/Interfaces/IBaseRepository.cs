using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;
public interface IBaseRepository<T> where T : class, IBaseEntity
{
    Task<T> Insert(T entity);
    T Update(T entity);
    T Delete(T entity);
}