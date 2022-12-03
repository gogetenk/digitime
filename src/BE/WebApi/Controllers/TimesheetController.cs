using Digitime.Server.Application.Calendar.Comands;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimesheetController : ControllerBase
{
    private readonly ISender _sender;

    public TimesheetController(IMediator mediator)
    {
        _sender = mediator;
    }

    /// <summary>
    /// Creates a new timesheet entry on the specified timesheet. If the timesheet doesn't exist, it will be created.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The created timesheet entry</returns>
    [HttpPost("entry")]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CreateTimesheetEntryReponse>> CreateTimesheetEntry([FromBody] CreateTimesheetEntryRequest request)
    {
        var resp = await _sender.Send((CreateTimesheetEntryCommand)request);
        return Ok((CreateTimesheetEntryReponse)resp);
    }
}
