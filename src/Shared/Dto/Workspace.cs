namespace Digitime.Shared.Dto;
public class Workspace
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Member> Members { get; set; }
}
