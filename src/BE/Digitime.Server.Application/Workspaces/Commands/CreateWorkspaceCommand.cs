using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Workspaces;
using Digitime.Shared.Contracts.Workspaces;
using MediatR;

namespace Digitime.Server.Application.Workspaces.Commands;

public record CreateWorkspaceCommand(string Name, string Description, string PricingTierId) : IRequest<CreateWorkspaceResponse>
{
    public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResponse>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        //private readonly IPricingTierRepository _pricingTierRepository;

        public CreateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository/*, IPricingTierRepository pricingTierRepository*/)
        {
            _workspaceRepository = workspaceRepository;
            //_pricingTierRepository = pricingTierRepository;
        }
        public async Task<CreateWorkspaceResponse> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            // TODO: gérer la souscription au moment de la création du workspace
            //var subscription = new Subscription(request.PricingTierId, workspace.Id);
            //await _subscriptionRepository.AddAsync(subscription);
            var workspace = Workspace.Create(null, request.Name, request.Description, null);
            var createdWorkspace= await _workspaceRepository.Insert(workspace);
            return new CreateWorkspaceResponse(createdWorkspace.Id, createdWorkspace.Name, createdWorkspace.Description, null); // TODO real subscription instead of null
        }
    }
}