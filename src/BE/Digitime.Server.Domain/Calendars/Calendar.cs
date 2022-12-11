using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Calendars.ValueObjects;
using Digitime.Server.Domain.Models;

namespace Digitime.Server.Domain.Calendars;

public class Calendar : AggregateRoot<string>
{
    private readonly CalendarDay[,] _calendarDays = new CalendarDay[6, 7];

    public List<CalendarDay> CalendarDays => _calendarDays.Cast<CalendarDay>().ToList();
    //public List<CalendarDay> CalendarDays { get { var list = _calendarDays.Cast<CalendarDay>().ToList(); list.RemoveAll(x => x == null); return list; } }

    public Calendar(string id, DateTime dateTime, List<DateTime> publicHolidays) : base(id)
    {
        _calendarDays = GetCalendarDaysForMonth(dateTime, publicHolidays);
    }

    public CalendarDay[,] GetCalendarDaysForMonth(DateTime dateTime, List<DateTime> publicHolidays)
    {
        var calendarDays = new CalendarDay[6, 7];
        var monthDates = GetAllDaysOfSpecifiedMonth(dateTime);
        var firstDayPositionInWeek = ((int)monthDates.FirstOrDefault().DayOfWeek) - 1;
        var weekIndex = 0;
        var weekdayIndex = firstDayPositionInWeek;

        // Iterate through days of the month and fill an inmemory calendar
        foreach (var day in monthDates)
        {
            if (weekdayIndex == 7)
            {
                weekdayIndex = 0;
                weekIndex++;
            }

            // TODO : Maybe that's where I should add the timesheet entries ? Instead of IsWorked = false, mettre une prop entries
            calendarDays[weekIndex, weekdayIndex] = new CalendarDay(day.DayOfWeek, day, publicHolidays.Contains(day), day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday);
            weekdayIndex++;
        }

        return calendarDays;
    }

    private int CalculateDaysBetweenDates(DateTime startTime, DateTime endTime)
    {
        return (int)(endTime - startTime).TotalDays;
    }

    private List<DateTime> GetAllDaysOfSpecifiedMonth(DateTime dateTime)
    {
        var days = new List<DateTime>();
        var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var daysInMonth = CalculateDaysBetweenDates(firstDayOfMonth, lastDayOfMonth);
        for (int i = 0; i <= daysInMonth; i++)
        {
            days.Add(firstDayOfMonth.AddDays(i));
        }
        return days;
    }
}
