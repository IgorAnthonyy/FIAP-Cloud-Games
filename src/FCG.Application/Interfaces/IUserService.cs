using FCG.Application.DTOs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateUser(UserCreate user);
    Task<UserResponse> CreateAdmin(AdminCreate user);
    Task<UserResponse> UpdateUser(UserUpdate user);
    Task ChangePassword(RequestChangePassword user);
    Task DeleteUser(Guid idUserToDeleted);
}
