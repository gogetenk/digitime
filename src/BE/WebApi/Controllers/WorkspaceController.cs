using System.Security.Claims;
using Digitime.Server.Application.Projects.Queries;
using Digitime.Server.Application.Workspaces.Commands;
using Digitime.Server.Application.Workspaces.Queries;
using Digitime.Shared.Contracts.Workspaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digitime.Server.Controllers;

[Authorize]
[Route("api/workspaces")]
[ApiController]
public class WorkspaceController : ControllerBase
{
    private readonly ISender _sender;

    public WorkspaceController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Post endpoint to create a workspace
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateWorkspaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWorkspaceCommand request)
    {
        var query = new CreateWorkspaceCommand(request.Name, request.Description, null, User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _sender.Send(query);
        if (response is null)
            return BadRequest(new { error = "Workspace creation failed." });

        return CreatedAtAction(nameof(GetUserWorkspaces), response);
    }

    /// <summary>
    /// Get endpoint to get all the workspaces associated to the current user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetUserWorkspacesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserWorkspaces()
    {
        var query = new GetProjectsQuery(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await _sender.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Get endpoint to get a workspace from its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WorkspaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWorkspaceById([FromRoute] string id)
    {
        var query = new GetWorkspaceByIdQuery(id);
        var response = await _sender.Send(query);
        if (response is null)
            return NotFound(new { error = "No workspace has been found for this Id." });

        return Ok(response);
    }
}
