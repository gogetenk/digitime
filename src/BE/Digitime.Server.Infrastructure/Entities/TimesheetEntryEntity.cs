using Digitime.Server.Domain.Models;

namespace Digitime.Server.Infrastructure.Entities;
public class TimesheetEntryEntity
{
    public TimesheetEntryProjectEntity Project { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }

    public static implicit operator TimesheetEntryEntity(TimesheetEntry timesheetEntry)
        => new TimesheetEntryEntity
        {
            Date = timesheetEntry.Date,
            Hours = timesheetEntry.Hours,
            Project = timesheetEntry.Project
        };

    public static implicit operator TimesheetEntry(TimesheetEntryEntity timesheetEntryEntity)
        => new TimesheetEntry(
            timesheetEntryEntity.Date,
            timesheetEntryEntity.Hours,
            timesheetEntryEntity.Project);
}
