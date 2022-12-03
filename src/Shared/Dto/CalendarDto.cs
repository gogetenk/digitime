using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Digitime.Shared.Dto;

public class CalendarDto
{
    public CalendarDayDto[,] CalendarDays { get; set; } = new CalendarDayDto[6, 7];
}
 