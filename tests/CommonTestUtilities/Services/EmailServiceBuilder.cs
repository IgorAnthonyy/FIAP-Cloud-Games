using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Interfaces;
using FCG.Domain.Views;
using Moq;

namespace CommonTestUtilities.Services;

public class EmailServiceBuilder
{
    public static IEmailService Build(User user)
    {
        var mock = new Mock<IEmailService>();

        mock.Setup(s => s.SendAsync(
                It.IsAny<UserView>(),
                It.IsAny<string>(),
                It.IsAny<EmailOptions>()))
            .ReturnsAsync(true);

        mock.Setup(s => s.SendAsync(It.IsAny<UserView>()))
            .ReturnsAsync(true);

        return mock.Object;
    }
}
