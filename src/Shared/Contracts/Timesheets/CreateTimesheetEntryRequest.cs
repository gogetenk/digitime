using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts.Timesheets;

public record CreateTimesheetEntryRequest(TimesheetEntryDto TimesheetEntry, string UserId, string? TimesheetId);