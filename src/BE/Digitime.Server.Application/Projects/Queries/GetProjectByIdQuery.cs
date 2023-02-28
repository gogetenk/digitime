using System;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Shared.Contracts.Projects;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Projects.Queries;

public record GetProjectByIdQuery(string projectId) : IRequest<ProjectDto>, ICacheableRequest
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.FindByIdAsync(request.projectId);
            return projects.Adapt<ProjectDto>();
        }
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddHours(1);
    }

    public string GetCacheKey()
    {
        return $"Projects_{projectId}";
    }
}
