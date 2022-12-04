using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Projects.ValueObjects;

namespace Digitime.Server.Domain.Projects;
public class Project : AggregateRoot<string>
{
    private readonly List<Member> _members = new();
    private readonly List<string> _timesheetIds = new();

    public string Title { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyList<Member> Members { get; private set; }
    public IReadOnlyList<string> TimesheetIds { get; private set; } 

    private Project(string id, string title, string code, string description) : base(id)
    {
        Title = title;
        Code = code;
        Description = description;
    }

    public static Project Create(string id, string title, string code, string description)
        => new (id, title, code, description);

    public void AddMember(Member member)
    {
        if (_members.Any(x => x.UserId == member.UserId))
            throw new InvalidOperationException($"Member with id {member.UserId} already exists");

        _members.Add(member);
    }

    public void RemoveMember(Member member)
    {
        if (!_members.Contains(member))
            throw new InvalidOperationException($"Member with id {member.UserId} does not exist");

        _members.Remove(member);
    }

    public void AddTimesheet(string timesheetId)
    {
        if (_timesheetIds.Contains(timesheetId))
            throw new InvalidOperationException($"Timesheet with id {timesheetId} already exists");

        _timesheetIds.Add(timesheetId);
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }
}
