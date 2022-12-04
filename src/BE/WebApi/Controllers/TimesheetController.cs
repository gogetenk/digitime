using Digitime.Server.Application.Calendar.Comands;
using Digitime.Server.Application.Calendar.Queries;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Route("api/timesheets")]
[ApiController]
public class TimesheetController : ControllerBase
{
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
    [HttpPost("entry")]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CreateTimesheetEntryReponse>> CreateTimesheetEntry([FromBody] CreateTimesheetEntryRequest request)
    {
        var resp = await _sender.Send((CreateTimesheetEntryCommand)request);
        return Ok((CreateTimesheetEntryReponse)resp);
    }

    /// <summary>
    /// Gets the timesheet for the specified user and date.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The timesheet</returns>
    [HttpGet("{userId}/{date}")]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CreateTimesheetEntryReponse>> GetTimesheet([FromRoute] string userId, [FromRoute] DateTime date)
    {
        var resp = await _sender.Send(new GetTimesheetForUserAndMonthQuery(userId, date));
        if (resp is null)
            return NotFound();
        
        return Ok(resp);
    }
}
 