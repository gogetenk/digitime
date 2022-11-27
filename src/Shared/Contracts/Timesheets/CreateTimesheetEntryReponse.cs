using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts.Timesheets;
public record CreateTimesheetEntryReponse
{
    public TimesheetEntryDto TimesheetEntry { get; set; }
}
