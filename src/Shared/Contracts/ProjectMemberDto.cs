namespace Digitime.Shared.Contracts;

public record ProjectMemberDto(string UserId, string Fullname, string Email, string ProfilePicture, MemberRoleEnum MemberRole);

public enum MemberRoleEnum
{
    Worker,
    Reviewer,
    ProjectAdmin,
    WorkspaceAdmin
}
