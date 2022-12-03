using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;

public record CalendarDayDto(DayOfWeek DayOfWeek, DateTime Date, bool IsPublicHoliday, bool IsWeekend)
{
    public static implicit operator CalendarDayDto(CalendarDay calendarDay) =>
        new(calendarDay.DayOfWeek, calendarDay.Date, calendarDay.IsPublicHoliday, calendarDay.IsWeekend);
}
