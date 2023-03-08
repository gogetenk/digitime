using System.Security.Claims;
using Digitime.Server.Application.Indicators.Queries;
using Digitime.Shared.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Authorize]
[Route("api/indicators")]
[ApiController]
public class IndicatorController : ControllerBase
{
    private readonly ISender _sender;

    public IndicatorController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<DashboardIndicatorsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIndicatorsForUser()
    {
        var query = new GetIndicatorsQuery(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _sender.Send(query);
        return Ok(response);
    }
}
