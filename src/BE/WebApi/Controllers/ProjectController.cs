using System.Security.Claims;
using Digitime.Server.Application.Projects.Commands;
using Digitime.Server.Application.Projects.Queries;
using Digitime.Shared.Contracts.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Authorize]
[Route("api/projects")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ISender _sender;

    public ProjectController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Post endpoint to create a project
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand request)
    {
        var query = new CreateProjectCommand(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, request.Title, request.Code, request.Description, request.WorkspaceId);
        var response = await _sender.Send(query);
        if (response is null)
            return BadRequest(new { error = "Project creation failed." });

        return CreatedAtAction(nameof(GetUserProjects), response);
    }

    /// <summary>
    /// Get endpoint to get all the projects associated to the current user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetUserProjectsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProjects()
    {
        var query = new GetProjectsQuery(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _sender.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Get endpoint to get a project from its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectById([FromRoute] string id)
    {
        var query = new GetProjectByIdQuery(id);
        var response = await _sender.Send(query);
        if (response is null)
            return NotFound(new { error = "No project has been found for this Id." });

        return Ok(response);
    }
}
