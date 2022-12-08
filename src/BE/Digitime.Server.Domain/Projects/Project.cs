using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Projects.ValueObjects;

namespace Digitime.Server.Domain.Projects;

public class Project : AggregateRoot<string>
{
    private readonly List<ProjectMember> _members = new();

    public string Title { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    public string WorkspaceId { get; private set; }
    public IReadOnlyList<ProjectMember> Members => _members.AsReadOnly();

    public Project(string id, string title, string code, string description, string workspaceId) : base(id)
    {
        Title = title;
        Code = code;
        Description = description;
        WorkspaceId = workspaceId;
    }

    public void AddMember(ProjectMember member)
    {
        if (_members.Any(x => x.UserId == member.UserId))
            throw new InvalidOperationException($"Member with id {member.UserId} already exists");

        _members.Add(member);
    }

    public void RemoveMember(ProjectMember member)
    {
        if (!_members.Contains(member))
            throw new InvalidOperationException($"Member with id {member.UserId} does not exist");

        _members.Remove(member);
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }
}
