using Digitime.Server.Domain.Models;

namespace Digitime.Server.Infrastructure.Entities;
public class TimesheetEntryEntity
{
    public string ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }

    public static implicit operator TimesheetEntryEntity(TimesheetEntry timesheetEntry)
        => new TimesheetEntryEntity
        {
            ProjectId = timesheetEntry.ProjectId,
            ProjectTitle = timesheetEntry.ProjectTitle,
            Date = timesheetEntry.Date,
            Hours = timesheetEntry.Hours
        };

    public static implicit operator TimesheetEntry(TimesheetEntryEntity timesheetEntryEntity)
        => new TimesheetEntry(
            timesheetEntryEntity.ProjectId,
            timesheetEntryEntity.ProjectTitle,
            timesheetEntryEntity.Date,
            timesheetEntryEntity.Hours
        );
}
