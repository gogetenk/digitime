using System.Security.Claims;
using Digitime.Server.Application.Projects.Commands;
using Digitime.Server.Application.Projects.Queries;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Dto;
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

    /// <summary>
    /// Sends an invitation to a user to join a workspace and a project.
    /// </summary>
    /// <param name="command">The command containing the invitation details.</param>
    /// <returns>A status indicating the result of the invitation process.</returns>
    /// <response code="200">Invitation sent successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="403">User does not have the required permissions.</response>
    [HttpPost("invite")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Invite([FromBody] InviteMemberDto request)
    {
        var command = new SendInvitationCommand(request.ProjectId, User.FindFirst(ClaimTypes.NameIdentifier)!.Value, request.InviteeEmail);
        await _sender.Send(command);
        return Ok();
    }

    /// <summary>
    /// Registers a new user through an invitation link.
    /// </summary>
    /// <param name="command">The command containing the registration details.</param>
    /// <returns>A status indicating the result of the registration process.</returns>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">Invalid request data.</response>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterWithInvitation([FromBody] RegisterWithInvitationCommand command)
    {
        await _sender.Send(command);
        return Ok();
    }
}
