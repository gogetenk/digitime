using System;
using System.Collections.Generic;
using Digitime.Server.Domain.Models;

namespace Digitime.Server.Domain.Timesheets.ValueObjects;
public class CalendarDay : ValueObject
{
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime Date { get; set; }
    public bool IsPublicHoliday { get; set; }
    public bool IsWeekend { get; set; }
    public bool IsWorked { get; set; }

    public CalendarDay()
    {
    }

    public CalendarDay(DayOfWeek dayOfWeek, DateTime date, bool isPublicHoliday, bool isWeekend, bool isWorked)
    {
        DayOfWeek = dayOfWeek;
        Date = date;
        IsPublicHoliday = isPublicHoliday;
        IsWeekend = isWeekend;
        IsWorked = isWorked;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

