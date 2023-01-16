using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Shared.Contracts.Projects;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Projects;

public record GetProjectsQuery(string UserId) : IRequest<GetUserProjectsResponse>, ICacheableRequest
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, GetUserProjectsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<GetUserProjectsResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetbyExternalIdAsync(request.UserId);
            if (user is null)
                throw new ApplicationException("User not found.");

            var projects = await _projectRepository.GetProjectsByUserId(user.Id);
            return new GetUserProjectsResponse(projects.Adapt<List<UserProject>>());
        }
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddHours(1);
    }

    public string GetCacheKey()
    {
        return $"Projects_{UserId}";
    }
}
