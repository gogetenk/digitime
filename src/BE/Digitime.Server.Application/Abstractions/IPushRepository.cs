using System.Threading.Tasks;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Application.Abstractions;

public interface IPushRepository
{
    public Task SendAsync(Notification notification);
}
