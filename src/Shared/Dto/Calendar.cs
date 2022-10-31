using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Digitime.Shared.Dto;

public class Calendar
{
    public CalendarDay[,] CalendarDays { get; set; } = new CalendarDay[6, 7];
}
