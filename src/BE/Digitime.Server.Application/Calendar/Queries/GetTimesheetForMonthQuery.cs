using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Shared.Dto;
using MediatR;

namespace Digitime.Server.Application.Calendar.Queries;
public class GetTimeSheetForMonthQuery : IRequest<TimesheetDto>, ICacheableRequest
{
    public DateTime Date { get; set; }
    public string UserId { get; set; }

    public GetTimeSheetForMonthQuery()
    {
    }

    public GetTimeSheetForMonthQuery(DateTime date, string userId)
    {
        Date = date;
        UserId = userId;
    }

    public class GetTimeSheetForMonthQueryHandler : IRequestHandler<GetTimeSheetForMonthQuery, TimesheetDto>
    {
        private readonly IRepository<TimesheetEntity> _timesheetRepository;
        private readonly IMapper _mapper;

        public GetTimeSheetForMonthQueryHandler(IRepository<TimesheetEntity> timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<TimesheetDto> Handle(GetTimeSheetForMonthQuery request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.FindOneAsync(x => x.CreatorId == request.UserId && x.CreateDate == request.Date);
            return _mapper.Map<TimesheetDto>(timesheet);
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
