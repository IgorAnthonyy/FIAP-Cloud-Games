using FCG.Application.DTOs;
using FCG.Application.ViewModels;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> CreateUser(UserDTO user);
    }
}
