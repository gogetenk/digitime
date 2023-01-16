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
}
