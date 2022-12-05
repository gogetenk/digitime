using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;

namespace Digitime.Shared.Contracts.Timesheets;

public record CreateTimesheetEntryReponse(string Id, DateTime Date, float Hours, Project Project, List<Reviewer> Reviewers, TimesheetStatus Status)
{
    public static implicit operator CreateTimesheetEntryReponse(TimesheetEntry timesheetEntry)
        => new CreateTimesheetEntryReponse(
            timesheetEntry.Id,
            timesheetEntry.Date,
            timesheetEntry.Hours,
            timesheetEntry.Project,
            timesheetEntry.Reviewers,
            timesheetEntry.Status);
}