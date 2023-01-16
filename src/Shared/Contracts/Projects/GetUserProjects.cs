namespace Digitime.Shared.Contracts.Projects;

public record GetUserProjectsResponse(List<UserProject> Projects);
public record UserProject(string Id, string Title, string Code, string Description, string WorkspaceId, int MemberCount);
