using System.Security.Claims;
using Digitime.Server.Application.Notifications.Queries;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Dto;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly ISender _mediator;

    public NotificationsController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets the active notifications for the current user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<NotificationEntity>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<NotificationDto>>> GetActiveNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetNotificationsQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result.Adapt<List<NotificationDto>>());
    }
}