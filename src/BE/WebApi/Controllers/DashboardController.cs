using System.Security.Claims;
using Digitime.Server.Application.Timesheets.Queries;
using Digitime.Shared.Contracts.Timesheets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender mediator)
    {
        _sender = mediator;
    }

    [Authorize(Policy = "Worker")]
    [HttpGet("calendar")]
    [ProducesResponseType(typeof(Shared.Dto.CalendarDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendar([FromQuery] GetCalendarQuery query)
    {
        query = query with { UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value };
        return Ok(await _sender.Send(query));
    }

    [Authorize(Policy = "Reviewer")]
    [HttpGet("timesheets")]
    [ProducesResponseType(typeof(GetTimesheetForUserAndDateResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimesheetsToReview([FromQuery] GetTimesheetsToReviewQuery query)
    {
        query = query with { UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value };
        return Ok(await _sender.Send(query));
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new List<string>()
        {
            "Toto",
            "Titi",
            "Tata"
        };
    }

    [HttpGet("indicators")]
    public async Task<IEnumerable<string>> GetIndicators()
    {
        return new List<string>()
        {
            "71,897",
            "58.16%",
            "24.57%"
        };
    }
}
