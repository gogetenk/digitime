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

    public CalendarDay()
    {
    }

    public CalendarDay(DayOfWeek dayOfWeek, DateTime date, bool isPublicHoliday, bool isWeekend)
    {
        DayOfWeek = dayOfWeek;
        Date = date;
        IsPublicHoliday = isPublicHoliday;
        IsWeekend = isWeekend;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

