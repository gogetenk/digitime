using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Workspaces.ValueObjects;

namespace Digitime.Server.Domain.Workspaces;

public class Workspace : AggregateRoot<string>
{
    private readonly List<WorkspaceMember> _members = new();
    private readonly List<Project> _projects = new();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Subscription Subscription { get; private set; }
    public IReadOnlyList<WorkspaceMember> Members => _members.AsReadOnly();

    public Workspace(string id, string name, string description, Subscription subscription) : base(id)
    {
        Name = name;
        Description = description;
        Subscription = subscription;
    }

    public static Workspace Create(string id, string name, string description, Subscription subscription)
        => new(id, name, description, subscription);

    public void Update(string name, string description, Subscription subscription)
    {
        Name = name;
        Description = description;
        Subscription = subscription;
    }

    public void AddMember(WorkspaceMember member)
    {
        if (_members.Any(x => x.UserId == member.UserId))
            throw new InvalidOperationException($"Member with user id {member.UserId} already exists");

        _members.Add(member);
    }
}
