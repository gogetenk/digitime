using System;
using System.Collections.Generic;
using System.Text;

namespace Digitime.Shared.Dto;
public class CalendarDay
{
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime Date { get; set; }
    public bool IsPublicHoliday { get; set; }
    public bool IsWeekend { get; set; }
}
