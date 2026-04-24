using FCG.Domain.Entities;

namespace FCG.Application.Interfaces;

public interface ITokenService
{
    string GenerateTokenJWT(User userLogged);
}
