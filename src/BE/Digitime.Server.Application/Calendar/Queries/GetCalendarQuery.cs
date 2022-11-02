using System;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Ports;
using MediatR;

namespace Digitime.Server.Queries;

public class GetCalendarQuery : IRequest<Calendar>, ICacheableRequest
{
    public string Country { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public GetCalendarQuery()
    {
    }

    public GetCalendarQuery(string country, int month, int year)
    {
        Country = country;
        Month = month;
        Year = year;
    }

    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, Calendar>
    {
        private readonly IObtainPublicHolidays _obtainPublicHolidays;

        public GetCalendarQueryHandler(IObtainPublicHolidays obtainPublicHolidays)
        {
            _obtainPublicHolidays = obtainPublicHolidays;
        }

        public async Task<Calendar> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            var requestedDate = new DateTime(request.Year, request.Month, 1);
            var publicHolidays = await _obtainPublicHolidays.GetPublicHolidaysForSpecifiedMonthAndCountry(requestedDate, request.Country);

            return new Calendar(Guid.NewGuid(), requestedDate, publicHolidays);
        }
    }

    public string GetCacheKey()
    {
        return $"Calendar_{Country}_{Month}_{Year}";
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddDays(1);
    }
}
