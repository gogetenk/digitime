namespace Digitime.Server.Dto;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Workspace Workspace { get; set; }
    public List<WorkspaceMember> Reviewers { get; set; }
    public List<WorkspaceMember> Editors { get; set; }
    public User Owner { get; set; }
    public List<Timesheet> Timesheets { get; set; }
}
