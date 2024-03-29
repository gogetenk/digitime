﻿using Digitime.Server.Domain.Calendars;

namespace Digitime.Server.Domain.UnitTests;
public class CalendarTests
{
    [Fact]
    public void Calendar_GetForEveryMonthForThe100NextYears_ShouldNotFail()
    {
        // Act
        for (int i = 0; i < 100; i++)
        {
            for (int j = 1; j <= 12; j++)
            {
                var calendar = new Calendar(null, new DateTime(DateTime.Now.Year + i, j, 1), new List<DateTime>());
            }
        }
    }
}
