namespace Digitime.Server.Domain.Projects.ValueObjects;
public record Member(string UserId, string Fullname, string Email, string ProfilePicture, MemberRoleEnum MemberRole);
