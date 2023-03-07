using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Projects;
using Digitime.Shared.Contracts;
using Digitime.Shared.Contracts.Projects;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Projects.Commands;

public record CreateProjectCommand(string? UserId, string Title, string Code, string? Description, string WorkspaceId) : IRequest<CreateProjectResponse>
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetbyExternalIdAsync(request.UserId);
            if (user is null)
                throw new ApplicationException("User not found.");

            var member = new Domain.Projects.ValueObjects.ProjectMember(user.Id, user.Firstname + " " + user.Lastname, user.Email, user.ProfilePicture, Domain.Projects.ValueObjects.MemberRoleEnum.ProjectAdmin);
            var project = new Project(null, request.Title, request.Code, request.Description, request.WorkspaceId);
            project.AddMember(member);
            var result = await _projectRepository.InsertOneAsync(project);
            return new CreateProjectResponse(result.Id, result.Title, result.Code, result.Description, result.WorkspaceId, result.Members.Adapt<List<ProjectMemberDto>>());
        }
    }
}