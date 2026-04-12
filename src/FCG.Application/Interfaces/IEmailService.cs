using FCG.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public enum EmailOptions
    {
        Padrao = 1,
        Admin = 2
    }
    public interface IEmailService
    {
        Task<bool> SendAsync(UserViewModel user, string passoword = null, EmailOptions option = EmailOptions.Padrao);
    }
}
