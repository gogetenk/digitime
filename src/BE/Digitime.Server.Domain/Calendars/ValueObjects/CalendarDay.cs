using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Timesheets.Entities;

namespace Digitime.Server.Domain.Calendars.ValueObjects;
public class CalendarDay : ValueObject
{
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime Date { get; set; }
    public bool IsPublicHoliday { get; set; }
    public bool IsWeekend { get; set; }
    public bool IsWorked => TimesheetEntries.Any();
    public List<TimesheetEntry> TimesheetEntries { get; set; } = new();

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

