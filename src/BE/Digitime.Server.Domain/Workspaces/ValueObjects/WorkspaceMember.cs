namespace Digitime.Server.Domain.Workspaces.ValueObjects;

public record WorkspaceMember(string UserId, string Fullname, string Email, WorkspaceMemberEnum Role);