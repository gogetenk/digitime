using System;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Notifications;

namespace Digitime.Server.Application.Notifications.Services;

public class NotificationFactory : INotificationFactory
{
    private readonly IEmailRepository _emailrepository;
    private readonly IPushRepository _pushRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;

    public NotificationFactory(IEmailRepository emailRepository, IPushRepository pushRepository, INotificationRepository notificationRepository, IUserRepository userRepository)
    {
        _emailrepository = emailRepository;
        _pushRepository = pushRepository;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task CreateAndSendAsync(Notification notification)
    {
        // Any notification created in DB will be served in-app
        await _notificationRepository.CreateAsync(notification);

        // Envoyer la notification par e-mail
        await SendByEmail(notification);

        // Envoyer la notification push
        await SendByPush(notification);
    }

    private async Task SendByPush(Notification notification)
    {
        if (notification.Channels.Contains(Notification.NotificationChannelEnum.Push))
        {
            await _pushRepository.SendAsync(notification);
        }
    }

    private async Task SendByEmail(Notification notification)
    {
        if (notification.Channels.Contains(Notification.NotificationChannelEnum.Email))
        {
            var user = await _userRepository.GetbyExternalIdAsync(notification.UserId);
            if (user is null)
                throw new InvalidOperationException($"Cannot send an email to the user {notification.UserId} because he couldn't be found.");

            await _emailrepository.SendEmailAsync(user.Email, notification.Title, notification.Message);
        }
    }
}
