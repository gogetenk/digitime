using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts.Timesheets;

public record CreateTimesheetEntryRequest
{
    public TimesheetEntryDto TimesheetEntry { get; set; }
    public string? TimesheetId { get; set; }
    public string UserId { get; set; }
}
