using System.Collections.Generic;
using System.Threading.Tasks;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Application.Abstractions;

public interface INotificationRepository
{
    Task<List<Notification>> GetNotificationsAsync(string userId);
    Task DeleteAsync(string id);
    Task CreateAsync(Notification notification);
}
