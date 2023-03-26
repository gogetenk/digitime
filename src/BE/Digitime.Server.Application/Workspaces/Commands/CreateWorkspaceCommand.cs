using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Workspaces;
using Digitime.Server.Domain.Workspaces.ValueObjects;
using Digitime.Shared.Contracts.Workspaces;
using MediatR;

namespace Digitime.Server.Application.Workspaces.Commands;

public record CreateWorkspaceCommand(string Name, string Description, string PricingTierId, string? UserId) : IRequest<CreateWorkspaceResponse>
{
    public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResponse>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IUserRepository _userRepository;

        //private readonly IPricingTierRepository _pricingTierRepository;

        public CreateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository/*, IPricingTierRepository pricingTierRepository*/, IUserRepository userRepository)
        {
            _workspaceRepository = workspaceRepository;
            _userRepository = userRepository;
            //_pricingTierRepository = pricingTierRepository;
        }
        public async Task<CreateWorkspaceResponse> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetbyExternalIdAsync(request.UserId);
            // TODO: gérer la souscription au moment de la création du workspace
            //var subscription = new Subscription(request.PricingTierId, workspace.Id);
            //await _subscriptionRepository.AddAsync(subscription);
            var workspace = Workspace.Create(null, request.Name, request.Description, null, null);
            workspace.AddMember(new WorkspaceMember(owner.Id, $"{owner.Firstname} {owner.Lastname}", owner.Email, owner.ProfilePicture, WorkspaceMemberEnum.Admin));
            var createdWorkspace= await _workspaceRepository.Insert(workspace);
            return new CreateWorkspaceResponse(createdWorkspace.Id, createdWorkspace.Name, createdWorkspace.Description, null); // TODO real subscription instead of null
        }
    }
}