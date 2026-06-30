using FCG.Domain.Entities;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface INotificationPublisher
{
    Task PublishUserCreatedAsync(User user, bool isAdmin, string? temporaryPassword);
}
