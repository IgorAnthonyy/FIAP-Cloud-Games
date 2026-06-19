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

    public async Task PublishUserDefaultCreatedAsync(User user)
    {
        var @event = new UserDefaultCreatedEvent(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email.Value
        );

        await _publishEndpoint.Publish(@event, context =>
        {
            context.SetRoutingKey("user.created.default");
        });
    }

    public async Task PublishUserAdminCreatedAsync(User user, string temporaryPassword)
    {
        var @event = new UserAdminCreatedEvent(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email.Value,
            TemporaryPassword: temporaryPassword
        );

        await _publishEndpoint.Publish(@event, context =>
        {
            context.SetRoutingKey("user.created.admin");
        });
    }
}
