using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Shared.Contracts.Workspaces;
using MediatR;

namespace Digitime.Server.Application.Workspaces.Queries;
public record GetWorkspaceByIdQuery(string Id) : IRequest<WorkspaceDto>
{
    public class GetWorkspaceByIdQueryHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        public GetWorkspaceByIdQueryHandler(IWorkspaceRepository workspaceRepository)
        {
            _workspaceRepository = workspaceRepository;
        }
        public async Task<WorkspaceDto> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
        {
            var workspace = await _workspaceRepository.GetbyIdAsync(request.Id);
            return new WorkspaceDto(workspace.Id, workspace.Name, workspace.Description, workspace.Members.Select(x => new WorkspaceMemberDto(x.UserId, x.Fullname, x.Email, x.ProfilePicture, (WorkspaceMemberRoleEntityEnum)x.Role)).ToList());
        }
    }
}
