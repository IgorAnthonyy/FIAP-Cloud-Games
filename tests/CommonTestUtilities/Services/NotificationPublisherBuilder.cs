using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using Moq;

namespace CommonTestUtilities.Services;

public class NotificationPublisherBuilder
{
    public static INotificationPublisher Build(User user)
    {
        var mock = new Mock<INotificationPublisher>();

        mock.Setup(s => s.PublishUserDefaultCreatedAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        mock.Setup(s => s.PublishUserAdminCreatedAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        return mock.Object;
    }
}
