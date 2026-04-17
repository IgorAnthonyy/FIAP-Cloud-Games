using FCG.Domain.Interfaces;
using Moq;

namespace CommonTestUtilities.Password;

public class PasswordServiceBuilder
{
    private readonly Mock<IPasswordService> _mock;

    public PasswordServiceBuilder()
    {
        _mock = new Mock<IPasswordService>();

        _mock.Setup(passwordService => passwordService.GenerateHash(It.IsAny<string>())).Returns("!%dlfjkd545");
    }

    public PasswordServiceBuilder VerifyPassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) == false)
        {
            _mock.Setup(passwordEncrypter => passwordEncrypter.VerifyPassword(It.IsAny<string>(), password)).Returns(true);
        }

        return this;
    }

    public IPasswordService Build() => _mock.Object;
}
