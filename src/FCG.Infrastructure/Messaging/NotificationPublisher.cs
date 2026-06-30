using FCG.Domain.Entities;
using FCG.Shared.Events;
using FCG.Domain.Interfaces;
using MassTransit;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Messaging;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public NotificationPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishUserCreatedAsync(User user, bool isAdmin, string? temporaryPassword)
    {
        var @event = new UserCreatedEvent(
            UserId: user.Id,
            Name: user.Name,
            Email: user.Email.Value,
            IsAdmin: isAdmin,
            TemporaryPassword: temporaryPassword
        );

        await _publishEndpoint.Publish(@event, context =>
        {
            context.SetRoutingKey("user.created-created");
        });
    }
}
