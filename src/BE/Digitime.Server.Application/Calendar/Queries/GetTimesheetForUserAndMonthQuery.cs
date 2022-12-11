using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Ports;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Calendar.Queries;
public record GetTimesheetForUserAndMonthQuery(string UserId, int Month, int Year, string Country) : IRequest<CalendarDto>/*, ICacheableRequest*/
{
    public class GetTimeSheetForMonthQueryHandler : IRequestHandler<GetTimesheetForUserAndMonthQuery, CalendarDto>
    {
        private readonly IObtainPublicHolidays _obtainPublicHolidays;
        private readonly IRepository<TimesheetEntity> _timesheetRepository;

        public GetTimeSheetForMonthQueryHandler(IRepository<TimesheetEntity> timesheetRepository, IObtainPublicHolidays obtainPublicHolidays)
        {
            _timesheetRepository = timesheetRepository;
            _obtainPublicHolidays = obtainPublicHolidays;
        }

        public async Task<CalendarDto> Handle(GetTimesheetForUserAndMonthQuery request, CancellationToken cancellationToken)
        {
            //// Get all timesheets for the user and the month
            //var timesheets = (await _timesheetRepository.FilterByAsync(x => x.Worker.UserId == request.UserId /*&& x.CreateDate.Month == request.Month && x.CreateDate.Year == request.Year*/)).ToList();
            //if (timesheets is null || !timesheets.Any())
            //    return null;

            //// Create a calendar for the month
            //var calendar = await GetCalendar(request.Year, request.Month, request.Country);

            //// Iterate through all days in the calendar, and assign the right timesheet entries for each
            //foreach (var day in calendar.CalendarDays)
            //{
            //    if (day is null)
            //        continue;

            //    var timesheetEntry = timesheets.SelectMany(x => x.TimesheetEntries).FirstOrDefault(x => x.Date.Year == day.Date.Year && x.Date.Month == day.Date.Month && x.Date.Day == day.Date.Day);
            //    if (timesheetEntry is null)
            //        continue;

            //    day.IsWorked = true;
            //    //day.TimesheetEntry = timesheetEntry.Adapt<TimesheetEntry>();
            //}

            //return calendar.Adapt<CalendarDto>();
            return null;
        }

        private async Task<Domain.Timesheets.Entities.Calendar> GetCalendar(int year, int month, string country)
        {
            var requestedDate = new DateTime(year, month, 1);
            var publicHolidays = await _obtainPublicHolidays.GetPublicHolidaysForSpecifiedMonthAndCountry(requestedDate, country);

            return new Domain.Timesheets.Entities.Calendar(Guid.NewGuid(), requestedDate, publicHolidays);
        }
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddDays(1);
    }

    public string GetCacheKey()
    {
        return $"Timesheet_{UserId}_{Month}_{Year}";
    }
}
