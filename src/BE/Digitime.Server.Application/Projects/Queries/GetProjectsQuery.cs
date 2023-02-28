using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Shared.Contracts.Projects;
using EasyCaching.Core;
using Mapster;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Digitime.Server.Application.Projects.Queries;

public record GetProjectsQuery(string UserId) : IRequest<GetUserProjectsResponse>, ICacheableRequest
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, GetUserProjectsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IUserRepository userRepository, IProjectRepository projectRepository, IDistributedCache cache, IEasyCachingProviderFactory cachingFactory)
        {
            var factory = cachingFactory.GetCachingProvider("memory");
            factory.Set("toto", "titi", TimeSpan.FromMinutes(1));
            var t = cache.GetAsync("toto");
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<GetUserProjectsResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetbyExternalIdAsync(request.UserId);
            if (user is null)
                throw new ApplicationException("User not found.");

            var projects = await _projectRepository.GetProjectsByUserId(user.Id);
            return new GetUserProjectsResponse(projects.Adapt<List<ProjectDto>>());
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
