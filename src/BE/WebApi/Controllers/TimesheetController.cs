using Digitime.Server.Application.Calendar.Comands;
using Digitime.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimesheetController : ControllerBase
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IMediator _mediator;

    public TimesheetController(ILogger<DashboardController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TimesheetEntryDto>> CreateTimesheetEntry([FromBody] CreateTimesheetEntryCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}
