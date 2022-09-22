namespace Digitime.Server.Dto;

public class Timesheet
{
    public Guid Id { get; set; }
    public Project Project { get; set; }
    public WorkspaceMember Owner { get; set; }
    public WorkspaceMember SignedBy { get; set; }
}