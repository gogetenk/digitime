﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Calendars;
using Digitime.Shared.Dto;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Timesheets.Queries;

public record GetCalendarQuery(string Country, int Month, int Year, string? UserId) : IRequest<CalendarDto>, ICacheableRequest
{
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarDto>
    {
        private readonly IObtainPublicHolidays _obtainPublicHolidays;
        private readonly ITimesheetRepository _timesheetRepository;

        public GetCalendarQueryHandler(IObtainPublicHolidays obtainPublicHolidays, ITimesheetRepository timesheetRepository)
        {
            _obtainPublicHolidays = obtainPublicHolidays;
            _timesheetRepository = timesheetRepository;
        }

        public async Task<CalendarDto> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            var requestedDate = new DateTime(request.Year, request.Month, 1);
            var publicHolidays = await _obtainPublicHolidays.GetPublicHolidaysForSpecifiedMonthAndCountry(requestedDate, request.Country);
            var calendar = new Calendar(null, requestedDate, publicHolidays);

            // Get all timesheets for the user and the month
            var timesheets = await _timesheetRepository.GetbyUserAndMonthOfyear(request.UserId, request.Month, request.Year);
            if (timesheets is null || !timesheets.Any())
                return calendar.Adapt<CalendarDto>();

            // Iterate through all the timesheets, and assign IsWorked = true to the associated CalendarDay in Calendar object
            foreach (var timesheet in timesheets)
            {
                foreach (var timesheetEntry in timesheet.TimesheetEntries)
                {
                    var calendarDay = calendar.CalendarDays.FirstOrDefault(x => x?.Date.Year == timesheetEntry.Date.Year && x?.Date.Month == timesheetEntry.Date.Month && x?.Date.Day == timesheetEntry.Date.Day);
                    if (calendarDay is null)
                        continue;

                    calendarDay.TimesheetEntries.Add(timesheetEntry);
                }
            }
            return calendar.Adapt<CalendarDto>();
        }
    }

    public string GetCacheKey()
    {
        return $"Calendar_{Month}_{Year}_{UserId}";
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddDays(1);
    }
}
