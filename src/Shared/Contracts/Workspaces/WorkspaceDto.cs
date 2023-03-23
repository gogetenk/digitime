namespace Digitime.Shared.Contracts.Workspaces;

public record WorkspaceDto(string Id, string Name, string Description, List<WorkspaceMemberDto> Members);
public record WorkspaceMemberDto(string UserId, string Fullname, string Email, string ProfilePicture, WorkspaceMemberRoleEntityEnum MemberRole);
public enum WorkspaceMemberRoleEntityEnum
{
    Worker,
    Reviewer,
    ProjectAdmin,
    WorkspaceAdmin,
    Pending
}