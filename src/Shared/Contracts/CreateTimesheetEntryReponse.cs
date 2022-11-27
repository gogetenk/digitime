using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts;
public record CreateTimesheetEntryReponse
{
    public TimesheetEntryDto TimesheetEntry { get; set; }
}
