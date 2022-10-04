//using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;
public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public OidcConfigurationController(ILogger<OidcConfigurationController> logger)
    {
        //ClientRequestParametersProvider = clientRequestParametersProvider;
        _logger = logger;
    }

    //public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        //var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        //return Ok(parameters);
        return Ok();
    }
}
