using System;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Calendar.Queries;
public record GetTimesheetForUserAndMonthQuery(string UserId, DateTime Date) : IRequest<Timesheet>, ICacheableRequest
{
    public class GetTimeSheetForMonthQueryHandler : IRequestHandler<GetTimesheetForUserAndMonthQuery, Timesheet>
    {
        private readonly IRepository<TimesheetEntity> _timesheetRepository;

        public GetTimeSheetForMonthQueryHandler(IRepository<TimesheetEntity> timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task<Timesheet> Handle(GetTimesheetForUserAndMonthQuery request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.FindOneAsync(x => x.Worker.UserId == request.UserId && x.CreateDate == request.Date);
            //TimesheetDto dto = timesheet;
            return timesheet.Adapt<Timesheet>();
        }
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddDays(1);
    }

    public string GetCacheKey()
    {
        return $"Timesheet_{UserId}_{Date}";
    }
}
