using System.Security.Claims;
using Digitime.Server.Application.Timesheets.Commands;
using Digitime.Server.Application.Timesheets.Queries;
using Digitime.Shared.Contracts.Timesheets;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Route($"api/{_Endpoint}")]
[ApiController]
public class TimesheetController : ControllerBase
{
    private const string _Endpoint = "timesheets";
    private readonly ISender _sender;

    public TimesheetController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get the current user's timesheet for the specified month. Country is used to enrich with public holidays.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [Authorize(Roles = "Worker, Reviewer")]
    [HttpGet("calendar")]
    [ProducesResponseType(typeof(Shared.Dto.CalendarDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendar([FromQuery] GetCalendarQuery query)
    {
        query = query with { UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value };
        return Ok(await _sender.Send(query));
    }

    /// <summary>
    /// Gets pending worktime to review for the current user.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [Authorize(Roles = "Reviewer")]
    [HttpGet("timesheets")]
    [ProducesResponseType(typeof(GetTimesheetForUserAndDateResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimesheetsToReview([FromQuery] GetTimesheetsToReviewQuery query)
    {
        query = query with { UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value };
        return Ok(await _sender.Send(query));
    }

    /// <summary>
    /// Creates a new timesheet entry on the specified timesheet. If the timesheet doesn't exist, it will be created.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The created timesheet entry</returns>
    [Authorize(Roles = "Worker, Reviewer")]
    [HttpPost("entry")]
    [ProducesResponseType(typeof(CreateTimesheetEntryReponse), StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CreateTimesheetEntryReponse>> CreateTimesheetEntry([FromBody] CreateTimesheetEntryRequest request)
    {
        request = request with { UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value };
        var resp = await _sender.Send(request.Adapt<CreateTimesheetEntryCommand>());
        return Created(_Endpoint, resp.Adapt<CreateTimesheetEntryReponse>());
    }
}
