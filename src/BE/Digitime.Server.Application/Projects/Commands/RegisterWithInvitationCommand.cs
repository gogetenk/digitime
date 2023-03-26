using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Digitime.Server.Application.Projects.Commands;
public record RegisterWithInvitationCommand(string InvitationToken, string Id) : IRequest
{
    public class RegisterWithInvitationCommandHandler : IRequestHandler<RegisterWithInvitationCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IConfiguration _configuration;

        public RegisterWithInvitationCommandHandler(IUserRepository userRepository, IProjectRepository projectRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _configuration = configuration;
        }

        public async Task Handle(RegisterWithInvitationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetbyExternalIdAsync(request.Id);

            // Validate and decode the invitation token
            var (workspaceId, projectId, invitedUserEmail) = DecodeInvitationToken(request.InvitationToken);

            //// We verify that the email of the user who is registering is the same as the one in the invitation token
            //if (user.Email != invitedUserEmail)
            //    throw new Exception("The email of the user who is registering is not the same as the one in the invitation token");

            // Update the user information in the workspace and project
            var project = await _projectRepository.FindByIdAsync(projectId);
            var incompleteMember = project.Members.FirstOrDefault(x => x.Email == invitedUserEmail);
            project.EnableIncompleteMember(incompleteMember);

            //var incompleteUser = await _projectRepository.FindMemberByEmailAsync(invitedUserEmail);
            //await _workspaceRepository.UpdateMemberAsync(workspaceId, request.UserId, invitedUserEmail); // TODO

            //await _projectRepository.UpdateMemberAsync(projectId, request.UserId, incompleteUser);
        }

        private (string workspaceId, string projectId, string invitedUserEmail) DecodeInvitationToken(string invitationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecretKey = _configuration["JwtSettings:Secret"];

            // Validate the invitation token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(invitationToken, tokenValidationParameters, out SecurityToken validatedToken);

                // Extract claims from the validated token
                var jwtToken = (JwtSecurityToken)validatedToken;
                var workspaceIdClaim = jwtToken.Claims.First(c => c.Type == "workspaceId");
                var projectIdClaim = jwtToken.Claims.First(c => c.Type == "projectId");
                var invitedUserEmailClaim = jwtToken.Claims.First(c => c.Type == "invitedUserEmail");

                // Convert claims to their corresponding types
                var workspaceId = workspaceIdClaim.Value;
                var projectId = projectIdClaim.Value;
                var invitedUserEmail = invitedUserEmailClaim.Value;

                return (workspaceId, projectId, invitedUserEmail);
            }
            catch (Exception ex)
            {
                // Handle the exception according to your application's requirements
                throw new ApplicationException("Invalid invitation token.", ex);
            }
        }
    }
}
