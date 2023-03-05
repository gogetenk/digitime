namespace Digitime.Shared.Contracts.Projects;

public record CreateProjectResponse(string Id, string Title, string Code, string Description, string WorkspaceId, List<ProjectMemberDto> Members);
