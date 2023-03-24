using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Notifications;
using Digitime.Server.Domain.Notifications.ValueObjects;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Users;
using MediatR;
using static Digitime.Server.Domain.Notifications.Notification;

namespace Digitime.Server.Application.Notifications.Events;

public record ProjectInvitedEvent(User Invitee, User Inviter, Project Project) : INotification
{
    public class ProjectInvitedEventHandler : INotificationHandler<ProjectInvitedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;

        public ProjectInvitedEventHandler(INotificationRepository notificationRepository, INotificationService notificationService)
        {
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
        }

        public async Task Handle(ProjectInvitedEvent notification, CancellationToken cancellationToken)
        {
            var title = "Invitation to join a project";
            var message = $"You have been invited to join the project '{notification.Project.Title}' by {notification.Inviter.Firstname}.";

            var notificationEntity = Notification.Create
            (
                NotificationTypeEnum.Info,
                title,
                message,
                notification.Invitee.ExternalId, // EXTERNAL ??
                null,
                new NotificationAction
                {
                    Title = "See",
                    Url = $"/projects/{notification.Project.Id}",
                    BackgroundColor = "",
                    TextColor = ""
                },
                DateTime.UtcNow,
                null,
                NotificationStatusEnum.Unread,
                new List<NotificationChannelEnum> { NotificationChannelEnum.InApp },
                new Dictionary<string, string>
                {
                    { "projectId", notification.Project.Id },
                    { "inviterId", notification.Inviter.Id }
                }
            );

            await Task.WhenAll(
                _notificationRepository.CreateAsync(notificationEntity),
                _notificationService.SendAsync(notificationEntity));
        }
    }
}
