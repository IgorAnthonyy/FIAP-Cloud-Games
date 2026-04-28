using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface IAccessDomainService
{
    Task<User> Login(Email email, Password password);
}
