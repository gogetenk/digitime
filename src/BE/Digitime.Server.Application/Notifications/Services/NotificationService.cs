using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Application.Notifications.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailRepository _emailrepository;
    private readonly IPushRepository _pushRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;

    public NotificationService(IEmailRepository emailRepository, IPushRepository pushRepository, INotificationRepository notificationRepository, IUserRepository userRepository)
    {
        _emailrepository = emailRepository;
        _pushRepository = pushRepository;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task SendAsync(Notification notification)
    {
        var user = await _userRepository.GetbyExternalIdAsync(notification.UserId);

        // Envoyer la notification par e-mail
        if (notification.Channels.Contains(Notification.NotificationChannelEnum.Email))
        {
            await _emailrepository.SendEmailAsync(user.Email, notification.Title, notification.Message);
        }

        // Envoyer la notification push
        if (notification.Channels.Contains(Notification.NotificationChannelEnum.Push))
        {
            await _pushRepository.SendAsync(notification);
        }

        // Stocker la notification in-app
        if (notification.Channels.Contains(Notification.NotificationChannelEnum.InApp))
        {
            await _notificationRepository.CreateAsync(notification);
        }
    }
}
