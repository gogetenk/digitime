namespace Digitime.Server.Dto;

public class Workspace
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<WorkspaceMember> Members {get;set;}
}