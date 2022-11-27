using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Digitime.Server.Domain.Models;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using MediatR;

namespace Digitime.Server.Application.Calendar.Comands;

public class CreateTimesheetEntryCommand : IRequest<TimesheetEntry>
{
    public TimesheetEntry TimesheetEntry { get; set; }
    public string? TimesheetId { get; set; }
    public string UserId { get; set; }

    public CreateTimesheetEntryCommand()
    {
    }

    public class CreateTimesheetEntryCommandHandler : IRequestHandler<CreateTimesheetEntryCommand, TimesheetEntry>
    {
        private readonly IRepository<TimesheetEntity> _timesheetRepository;
        private readonly IMapper _mapper;

        public CreateTimesheetEntryCommandHandler(IRepository<TimesheetEntity> timesheetRepository, IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        public async Task<TimesheetEntry> Handle(CreateTimesheetEntryCommand request, CancellationToken cancellationToken)
        {
            Timesheet? timesheet = null;

            // Request existing timesheet if it exists
            if (request.TimesheetId is not null)
                timesheet = _mapper.Map<Timesheet>(await _timesheetRepository.FindByIdAsync(request.TimesheetId.ToString()));

            // If timesheet does not exist, create new timesheet for the current month
            if (timesheet is null)
            {
                timesheet = new Timesheet(request.UserId, request.TimesheetEntry.Date);
                await _timesheetRepository.InsertOneAsync(_mapper.Map<TimesheetEntity>(timesheet));
            }

            // Add timesheet entry to timesheet
            timesheet.AddTimesheetEntry(request.TimesheetEntry.Date,
                                        request.TimesheetEntry.Hours,
                                        request.TimesheetEntry.ProjectId,
                                        request.TimesheetEntry.ProjectTitle);

            // update timesheet
            await _timesheetRepository.ReplaceOneAsync(_mapper.Map<TimesheetEntity>(timesheet));

            // Return newly created timesheet entry
            return request.TimesheetEntry;
        }
    }
}
