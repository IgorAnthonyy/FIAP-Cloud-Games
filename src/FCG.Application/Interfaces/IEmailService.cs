using System.Threading.Tasks;
using FCG.Application.ViewModels;
using FCG.Domain.Enums;

namespace FCG.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync(UserViewModel user, string passoword = null, EmailOptions option = EmailOptions.Padrao);
}
