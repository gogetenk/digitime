using System.Security.Claims;
using Digitime.Server.Application.Projects;
using Digitime.Shared.Contracts.Projects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Route("api/projects")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ISender _sender;

    public ProjectController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GetUserProjectsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProjects()
    {
        var query = new GetProjectsQuery(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _sender.Send(query);
        if (response.Projects is null || !response.Projects.Any())
            return NotFound("No project has been found for the current user.");

        return Ok(response);
    }
}
