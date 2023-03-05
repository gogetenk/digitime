namespace Digitime.Shared.Contracts.Projects;

public record ProjectDto(string Id, string Title, string Code, string Description, string WorkspaceId, List<ProjectMemberDto> Members);

