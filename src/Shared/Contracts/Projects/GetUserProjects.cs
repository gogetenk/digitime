using Digitime.Shared.Contracts.Workspaces;

namespace Digitime.Shared.Contracts.Projects;

public record GetUserProjectsResponse(List<ProjectDto> Projects);
