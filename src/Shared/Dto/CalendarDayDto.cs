using Digitime.Server.Domain.Timesheets.Entities;

namespace Digitime.Shared.Dto;

public record CalendarDayDto(DayOfWeek DayOfWeek, DateTime Date, bool IsPublicHoliday, bool IsWeekend, List<TimesheetEntry> TimesheetEntries);
