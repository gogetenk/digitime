using AutoMapper;
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
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TimesheetController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateTimesheetEntryReponse>> CreateTimesheetEntry([FromBody] CreateTimesheetEntryRequest request)
    {
        var resp = await _mediator.Send(_mapper.Map<CreateTimesheetEntryCommand>(request));
        return Ok(_mapper.Map<CreateTimesheetEntryReponse>(resp));
    }
}
