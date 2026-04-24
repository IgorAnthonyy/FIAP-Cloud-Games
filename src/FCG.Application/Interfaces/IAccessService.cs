using FCG.Application.DTOs;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces;

public interface IAccessService
{
    Task<string> Login(AccessLogin login);
}

