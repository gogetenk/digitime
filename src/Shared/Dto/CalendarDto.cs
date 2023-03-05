namespace Digitime.Shared.Dto;

public class CalendarDto
{
    public CalendarDayDto[,] CalendarDays { get; set; } = new CalendarDayDto[6, 7];
}
