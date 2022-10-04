using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public DashboardController(ILogger<WeatherForecastController> logger)
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
}
