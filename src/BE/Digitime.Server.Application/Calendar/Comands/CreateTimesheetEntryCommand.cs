using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Domain.Timesheet;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Shared.Contracts.Timesheets;
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

        public CreateTimesheetEntryCommandHandler(IRepository<TimesheetEntity> timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task<TimesheetEntry> Handle(CreateTimesheetEntryCommand request, CancellationToken cancellationToken)
        {
            Timesheet timesheet = null;

            // Request existing timesheet if it exists
            if (request.TimesheetId is not null)
                timesheet = await _timesheetRepository.FindByIdAsync(request.TimesheetId.ToString());

            // If timesheet does not exist, create new timesheet for the current month
            if (timesheet is null)
            {
                timesheet = Timesheet.Create(creatorId: request.UserId, beginDate: request.TimesheetEntry.Date);
                await _timesheetRepository.InsertOneAsync(timesheet);
            }

            // Add timesheet entry to timesheet
            timesheet.AddTimesheetEntry(request.TimesheetEntry.Date,
                                        request.TimesheetEntry.Hours,
                                        request.TimesheetEntry.Project);

            // update timesheet
            await _timesheetRepository.ReplaceOneAsync(timesheet);

            // Return newly created timesheet entry
            return request.TimesheetEntry;
        }
    }

    public static implicit operator CreateTimesheetEntryCommand(CreateTimesheetEntryRequest request) =>
        new()
        {
            TimesheetEntry = request.TimesheetEntry,
            TimesheetId = request.TimesheetId,
            UserId = request.UserId
        };
}
