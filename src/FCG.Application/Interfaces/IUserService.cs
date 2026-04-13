using FCG.Application.DTOs;
using FCG.Domain.Views;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(UserCreate user);
    }
}
