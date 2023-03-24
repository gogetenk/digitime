using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Notifications;
using MediatR;

namespace Digitime.Server.Application.Notifications.Queries;

public record GetNotificationsQuery(string userId) : IRequest<List<Notification>>
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<Notification>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetNotificationsAsync(request.userId);
        }
    }
}