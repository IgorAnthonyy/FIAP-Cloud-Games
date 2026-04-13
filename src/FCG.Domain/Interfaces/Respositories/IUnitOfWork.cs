using System.Threading.Tasks;

namespace FCG.Domain.Interfaces.Respositories;

public interface IUnitOfWork
{
    Task CommitAsync();
}