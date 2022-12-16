using System.Security.Claims;
using Digitime.Server.Application.Calendar.Commands;
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
    /// Creates a new timesheet entry on the specified timesheet. If the timesheet doesn't exist, it will be created.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The created timesheet entry</returns>
    [Authorize(Policy = "Worker")]
    [HttpPost("entry")]
    [ProducesResponseType(typeof(CreateTimesheetEntryReponse), StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CreateTimesheetEntryReponse>> CreateTimesheetEntry([FromBody] CreateTimesheetEntryRequest request)
    {
        request = request with { UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value };
        var resp = await _sender.Send(request.Adapt<CreateTimesheetEntryCommand>());
        return Created(_Endpoint, resp.Adapt<CreateTimesheetEntryReponse>());
    }
}
