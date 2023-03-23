using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Projects.ValueObjects;
using Digitime.Server.Domain.Users;
using Digitime.Server.Domain.Workspaces;
using Digitime.Server.Domain.Workspaces.ValueObjects;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project = Digitime.Server.Domain.Projects.Project;

namespace Digitime.Server.Application.Projects.Commands;
public record SendInvitationCommand(string ProjectId, string InviterUserId, string InviteeEmail) : IRequest
{
    public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand>
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IConfiguration _configuration;
        private readonly IWorkspaceRepository _workspaceRepository;

        public SendInvitationCommandHandler(IEmailRepository emailRepository, IUserRepository userRepository, IProjectRepository projectRepository, IConfiguration configuration, IWorkspaceRepository workspaceRepository)
        {
            _emailRepository = emailRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _configuration = configuration;
            _workspaceRepository = workspaceRepository;
        }

        public async Task<Unit> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
            var inviterTask = _userRepository.GetbyExternalIdAsync(request.InviterUserId);
            var projectTask = _projectRepository.FindByIdAsync(request.ProjectId);

            await Task.WhenAll(inviterTask, projectTask);

            var inviter = inviterTask.Result;
            var project = projectTask.Result;

            if (project is null)
                throw new InvalidOperationException("Project not found.");

            if (inviter is null)
                throw new InvalidOperationException("Inviter not found.");

            var inviterMember = project.Members.FirstOrDefault(x => x.UserId == inviter.Id);
            if (inviterMember is null)
                throw new UnauthorizedAccessException("You are not a member of this project.");

            if (inviterMember.MemberRole != MemberRoleEnum.ProjectAdmin && inviterMember.MemberRole != MemberRoleEnum.WorkspaceAdmin)
                throw new UnauthorizedAccessException("User does not have sufficient permissions.");

            var invitee = await _userRepository.GetByEmail(request.InviteeEmail);
            var workspace = await _workspaceRepository.GetbyIdAsync(project.WorkspaceId);
            if (workspace == null)
                throw new Exception("Workspace not found.");

            if (invitee is null) // Invitee is not registered on Digitime
                await HandleUnregisteredInviteeAsync(request, inviter, workspace, project);
            else if (!workspace.Members.Any(m => m.UserId == invitee.Id)) // Invitee is a registered user but not a member of the workspace
                await HandleRegisteredInviteeNotInWorkspaceAsync(invitee, project, workspace, inviter);
            else // Invitee is a registered user and a member of the workspace
                await HandleRegisteredInviteeInWorkspaceAsync(invitee, project, inviter);

            return Unit.Value;
        }

        private async Task HandleUnregisteredInviteeAsync(SendInvitationCommand request, User inviter, Workspace workspace, Project project)
        {
            var invitationToken = GenerateInvitationToken(inviter, project.WorkspaceId, project.Id);
            var member = new ProjectMember(null, null, request.InviteeEmail, null, MemberRoleEnum.Pending);
            var workspaceMember = new WorkspaceMember(null, null, request.InviteeEmail, null, WorkspaceMemberEnum.Pending);
            project.AddMember(member);
            workspace.AddMember(workspaceMember);
            await _workspaceRepository.UpdateAsync(workspace);
            await _projectRepository.UpdateAsync(project);

            string subject = $"{inviter.Firstname} {inviter.Lastname} invited you to join {project.Title} on Digitime!";
            string content = $"You have been invited by {inviter.Firstname} {inviter.Lastname} to join {project.Title} on Digitime. Please click the following link to register and join the project: <a href=\"{GenerateInvitationUrlForUnknownMember(invitationToken)}\">Click here</a>";

            await _emailRepository.SendEmailAsync(request.InviteeEmail, subject, content);
        }

        private async Task HandleRegisteredInviteeNotInWorkspaceAsync(User invitee, Project project, Workspace workspace, User inviter)
        {
            var projectMember = new ProjectMember(invitee.Id, $"{invitee.Firstname} {invitee.Lastname}", invitee.Email, invitee.ProfilePicture, MemberRoleEnum.Pending);
            var workspaceMember = new WorkspaceMember(invitee.Id, $"{invitee.Firstname} {invitee.Lastname}", invitee.Email, invitee.ProfilePicture, WorkspaceMemberEnum.Pending);
            project.AddMember(projectMember);
            workspace.AddMember(workspaceMember);
            await _workspaceRepository.UpdateAsync(workspace);
            await _projectRepository.UpdateAsync(project);

            string subject = $"{inviter.Firstname} {inviter.Lastname} invited you to join {project.Title} on Digitime!";
            string content = $"You have been invited to join a new workspace and project on Digitime. Please click the following link to accept the invitation and join the workspace and project: <a href=\"{GenerateInvitationUrl(invitee.Id, project.WorkspaceId, project.Id)}\">Click here</a>";

            await _emailRepository.SendEmailAsync(invitee.Email, subject, content);
        }

        private async Task HandleRegisteredInviteeInWorkspaceAsync(User invitee, Project project, User inviter)
        {
            var member = new ProjectMember(invitee.Id, $"{invitee.Firstname} {invitee.Lastname}", invitee.Email, invitee.ProfilePicture, MemberRoleEnum.Pending);
            project.AddMember(member);
            await _projectRepository.UpdateAsync(project);

            string subject = $"{inviter.Firstname} {inviter.Lastname} invited you to join {project.Title} on Digitime!";
            string content = $"You have been invited to join a new project on Digitime. Please click the following link to accept the invitation and join the project: <a href=\"{GenerateInvitationUrl(invitee.Id, project.WorkspaceId, project.Id)}\">Click here</a>";

            await _emailRepository.SendEmailAsync(invitee.Email, subject, content);
        }

        private string GenerateInvitationToken(User user, string workspaceId, string projectId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("workspace_id", workspaceId.ToString()),
                new Claim("project_id", projectId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.Parse(_configuration["JwtSettings:Expiration"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateInvitationUrl(string userId, string workspaceId, string projectId)
        {
            // Implement your URL generation logic here.
            return $"{_configuration["BackendUrl"]}/invitation?userId={userId}&workspaceId={workspaceId}&projectId={projectId}";
        }

        private string GenerateInvitationUrlForUnknownMember(string invitationToken)
        {
            // Implement your URL generation logic here.
            return @$"{_configuration["Authentication:Schemes:Bearer:Authority"]}/authorize?response_type=code&client_id={_configuration["ExternalApis:Auth0ManagementApi:ClientId"]}&redirect_uri={_configuration["BackendUrl"]}/invitation&state={invitationToken}";
        }
    }
}
