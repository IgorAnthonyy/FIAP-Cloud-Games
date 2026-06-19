using FCG.Domain.Entities;
using System.Threading.Tasks;

namespace FCG.Domain.Interfaces;

public interface INotificationPublisher
{
    Task PublishUserDefaultCreatedAsync(User user);
    Task PublishUserAdminCreatedAsync(User user, string temporaryPassword);
}
