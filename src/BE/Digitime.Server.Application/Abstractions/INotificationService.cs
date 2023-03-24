using System.Threading.Tasks;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Application.Abstractions;

public interface INotificationService
{
    Task SendAsync(Notification notification);
}
