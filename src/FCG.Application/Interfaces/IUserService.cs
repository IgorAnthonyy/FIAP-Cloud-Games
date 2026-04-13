using FCG.Application.DTOs;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateUser(UserCreate user);
    Task<UserResponse> CreateAdmin(AdminCreate user);
}
