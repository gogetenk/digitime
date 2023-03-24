using System.Threading.Tasks;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Application.Abstractions;

public interface INotificationFactory
{
    Task CreateAndSendAsync(Notification notification);
}
