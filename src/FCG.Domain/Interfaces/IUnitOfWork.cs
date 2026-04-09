using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}