using Digitime.Server.Domain.Models;
using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts.Timesheets;
public record CreateTimesheetEntryReponse(TimesheetEntryDto TimesheetEntry)
{
    public static implicit operator CreateTimesheetEntryReponse(TimesheetEntry timesheetEntry) =>
        new((TimesheetEntryDto)timesheetEntry);
}
