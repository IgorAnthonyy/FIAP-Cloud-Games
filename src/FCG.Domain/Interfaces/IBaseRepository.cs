using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;
public interface IBaseRepository<T> where T : class, IBaseEntity
{
    Task<T> Insert(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
}