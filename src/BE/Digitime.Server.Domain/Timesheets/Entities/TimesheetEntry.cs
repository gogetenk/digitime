using System;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Timesheets.ValueObjects;

namespace Digitime.Server.Domain.Timesheets.Entities;

public class TimesheetEntry : Entity<string>
{
    public DateTime Date { get; private set; }
    public float Hours { get; private set; }
    public Project Project { get; private set; }
    public TimesheetStatus Status { get; private set; }

    public TimesheetEntry(string id, DateTime date, float hours, Project project, TimesheetStatus status) : base(id)
    {
        Date = date;
        Hours = hours;
        Project = project;
        Status = status;
    }

    public static TimesheetEntry Create(string id, DateTime date, float hours, Project project, TimesheetStatus status)
    {
        return new TimesheetEntry(id, date, hours, project, status);
    }

    public void Submit()
    {
        if (Status is not TimesheetStatus.Draft)
            throw new InvalidOperationException("Timesheet entry is not Draft");

        Status = TimesheetStatus.Submitted;
    }

    public void Approve()
    {
        if (Status is not TimesheetStatus.Submitted)
            throw new InvalidOperationException("Timesheet entry is not submitted");

        Status = TimesheetStatus.Approved;
    }

    public void Reject()
    {
        if (Status is not TimesheetStatus.Submitted)
            throw new InvalidOperationException("Timesheet entry is not submitted");

        Status = TimesheetStatus.Rejected;
    }
}

public enum TimesheetStatus
{
    Draft,
    Submitted,
    Approved,
    Rejected
}
