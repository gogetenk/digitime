using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;

namespace Digitime.Shared.Dto;

public record TimesheetDto(string Id, Worker worker, DateTime UpdateDate, DateTime CreateDate, DateTime BeginDate, DateTime EndDate, float TotalHours, List<TimesheetEntry> TimesheetEntries);
