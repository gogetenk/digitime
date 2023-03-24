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

public record ProjectInvitationEvent(User Invitee, User Inviter, Project Project) : INotification
{
    public class ProjectInvitedEventHandler : INotificationHandler<ProjectInvitationEvent>
    {
        private readonly INotificationFactory _notificationService;

        public ProjectInvitedEventHandler(INotificationFactory notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(ProjectInvitationEvent notification, CancellationToken cancellationToken)
        {
            var title = "Invitation to join a project";
            var message = $"You have been invited to join the project '{notification.Project.Title}' by {notification.Inviter.Firstname}.";
            //var channels = notification.Project.Settings.NotificationPreferences["ProjectInvitationChannels"]; // TODO: gérer ça avec les user preds plutôt

            var notificationEntity = Notification.Create
            (
                NotificationTypeEnum.Info,
                title,
                message,
                notification.Invitee.ExternalId, // EXTERNAL ??
                null,
                new NotificationAction
                {
                    Title = "View",
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
                _notificationService.CreateAndSendAsync(notificationEntity));
        }
    }
}
