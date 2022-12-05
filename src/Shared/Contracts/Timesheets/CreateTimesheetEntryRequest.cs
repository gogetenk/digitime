namespace Digitime.Shared.Contracts.Timesheets;

public record CreateTimesheetEntryRequest(string TimesheetId, string ProjectId, float Hours, DateTime Date, string UserId);
