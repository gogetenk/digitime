using Digitime.Server.Domain.Models;
using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts.Timesheets;

public record GetTimesheetForUserAndDateResponse(List<TimesheetDto> Timesheets)
{
  
}

