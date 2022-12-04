namespace Digitime.Shared.Dto;

public record CalendarDayDto(DayOfWeek DayOfWeek, DateTime Date, bool IsPublicHoliday, bool IsWeekend, List<TimesheetEntryDto> TimesheetEntries);
