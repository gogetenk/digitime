using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ILogger<DashboardController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        _logger.LogInformation("Received request");
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
        _logger.LogInformation(nameof(GetIndicators));
        return new List<string>()
        {
            "71,897",
            "58.16%",
            "24.57%"
        };
    }

    [HttpGet("timesheets")]
    public IEnumerable<string> GetTimesheets()
    {
        _logger.LogInformation(nameof(GetIndicators));
        return new List<string>()
        {
            "Project1",
            "Project2",
            "Project3"
        };
    }
}
