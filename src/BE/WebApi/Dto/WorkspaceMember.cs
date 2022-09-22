namespace Digitime.Server.Dto;

public class WorkspaceMember
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Workspace Workspace { get; set; }
}