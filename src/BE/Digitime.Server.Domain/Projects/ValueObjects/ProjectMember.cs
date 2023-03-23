namespace Digitime.Server.Domain.Projects.ValueObjects;

public record ProjectMember(string UserId, string Fullname, string Email, string ProfilePicture, MemberRoleEnum MemberRole);
