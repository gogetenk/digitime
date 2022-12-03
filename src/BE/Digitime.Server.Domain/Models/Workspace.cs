using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;

public class Workspace : AggregateRoot<string>
{
    public Workspace(string id) : base(id)
    {
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public List<Member> Members { get; private set; } = new();
    public List<Project> Projects { get; private set; } = new();

    public void AddMember(Member member)
    {
        Members.Add(member);
    }

    public void RemoveMember(Member member)
    {
        Members.Remove(member);
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void AddProject(Project project)
    {
        Projects.Add(project);
    }

    public void RemoveProject(Project project)
    {
        Projects.Remove(project);
    }

    public void UpdateProject(Project project)
    {
        Projects.Remove(project);
        Projects.Add(project);
    }
    
}
