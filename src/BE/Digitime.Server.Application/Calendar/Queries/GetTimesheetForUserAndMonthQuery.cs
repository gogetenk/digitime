using System;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Timesheet;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Shared.Dto;
using MediatR;

namespace Digitime.Server.Application.Calendar.Queries;
public record GetTimesheetForUserAndMonthQuery(string UserId, DateTime Date) : IRequest<TimesheetDto>, ICacheableRequest
{
    public class GetTimeSheetForMonthQueryHandler : IRequestHandler<GetTimesheetForUserAndMonthQuery, TimesheetDto>
    {
        private readonly IRepository<TimesheetEntity> _timesheetRepository;

        public GetTimeSheetForMonthQueryHandler(IRepository<TimesheetEntity> timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task<TimesheetDto> Handle(GetTimesheetForUserAndMonthQuery request, CancellationToken cancellationToken)
        {
            Timesheet timesheet = await _timesheetRepository.FindOneAsync(x => x.CreatorId == request.UserId && x.CreateDate == request.Date);
            TimesheetDto dto = timesheet;
            return dto;
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
