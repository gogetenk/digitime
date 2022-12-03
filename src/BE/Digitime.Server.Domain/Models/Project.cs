using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;
public class Project : AggregateRoot<string>
{
    public Project(string id) : base(id)
    {
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Code { get; private set; }
    public string WorkspaceId { get; private set; }
    public List<Member> Members { get; private set; }
}
