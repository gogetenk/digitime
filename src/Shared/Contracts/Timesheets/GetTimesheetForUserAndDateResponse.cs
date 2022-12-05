using Digitime.Server.Domain.Timesheets;

namespace Digitime.Shared.Contracts.Timesheets;

public record GetTimesheetForUserAndDateResponse(List<Timesheet> Timesheets)
{

}
