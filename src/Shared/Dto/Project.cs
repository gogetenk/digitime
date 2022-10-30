namespace Digitime.Shared.Dto;
public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public string WorkspaceId { get; set; }
    public List<Member> Members { get; set; }
}
