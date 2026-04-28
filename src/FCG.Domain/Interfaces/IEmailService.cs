using System.Threading.Tasks;
using FCG.Domain.Enums;
using FCG.Domain.Views;

namespace FCG.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendAsync(UserView user, string password = null, EmailOptions option = EmailOptions.Padrao);
}
