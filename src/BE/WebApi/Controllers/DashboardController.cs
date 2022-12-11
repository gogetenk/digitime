using Digitime.Server.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender mediator)
    {
        _sender = mediator;
    }

    [HttpGet("calendar")]
    [ProducesResponseType(typeof(Shared.Dto.CalendarDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendar([FromQuery] GetCalendarQuery query)
    {
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
