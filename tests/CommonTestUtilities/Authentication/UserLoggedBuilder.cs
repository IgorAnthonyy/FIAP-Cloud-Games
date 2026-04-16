using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using Moq;

namespace CommonTestUtilities.Authentication;

public class UserLoggedBuilder
{
    public static IUserLogged Build(User user)
    {
        var mock = new Mock<IUserLogged>();

        mock.Setup(u => u.UserId).Returns(user.Id);
        mock.Setup(u => u.Roles).Returns((List<Role>)(user.Roles ?? []));
        mock.Setup(u => u.IsAdmin).Returns(
            user.Roles?.Any(r => string.Equals(r.Name, "Admin", StringComparison.OrdinalIgnoreCase)) ?? false
        );

        return mock.Object;
    }

    public static IUserLogged BuildAdmin(User user)
    {
        var mock = new Mock<IUserLogged>();

        mock.Setup(u => u.UserId).Returns(user.Id);
        mock.Setup(u => u.Roles).Returns([new Role("Admin")]);
        mock.Setup(u => u.IsAdmin).Returns(true);

        return mock.Object;
    }
}