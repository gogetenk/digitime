using Digitime.Shared.Contracts.Timesheets;

namespace Digitime.Shared.Dto;

public record CalendarDayDto(DayOfWeek DayOfWeek, DateTime Date, bool IsPublicHoliday, bool IsWeekend, bool IsWorked, List<TimesheetEntryDto> TimesheetEntries);
