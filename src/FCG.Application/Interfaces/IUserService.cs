using FCG.Application.DTOs;
using FCG.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> CreateUser(UserDTO user);
    }
}
