namespace Digitime.Shared.Contracts.Projects;

public record CreateProjectResponse(string Id, string Title, string Code, string Description, string WorkspaceId, List<ProjectMemberDto> Members);
public record ProjectMemberDto(string UserId, string Fullname, string Email, string ProfilePicture, MemberRoleEnum MemberRole);
public enum MemberRoleEnum
{
    Worker,
    Reviewer,
    ProjectAdmin,
    WorkspaceAdmin
}
