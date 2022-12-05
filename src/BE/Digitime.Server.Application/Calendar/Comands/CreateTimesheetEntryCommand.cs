using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Shared.Contracts.Timesheets;
using MediatR;
using MongoDB.Driver.Linq;

namespace Digitime.Server.Application.Calendar.Comands;

public record CreateTimesheetEntryCommand(string TimesheetId, string ProjectId, float Hours, DateTime Date, string UserId) : IRequest<CreateTimesheetEntryReponse>
{
    public class CreateTimesheetEntryCommandHandler : IRequestHandler<CreateTimesheetEntryCommand, CreateTimesheetEntryReponse>
    {
        private readonly IRepository<TimesheetEntity> _timesheetRepository;
        private readonly IRepository<ProjectEntity> _projectRepository;
        private readonly IRepository<UserEntity> _userRepository;

        public CreateTimesheetEntryCommandHandler(IRepository<TimesheetEntity> timesheetRepository, IRepository<ProjectEntity> projectRepository, IRepository<UserEntity> userRepository)
        {
            _timesheetRepository = timesheetRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateTimesheetEntryReponse> Handle(CreateTimesheetEntryCommand request, CancellationToken cancellationToken)
        {
            Timesheet timesheet = null;

            // Request existing timesheet if it exists
            if (request.TimesheetId is not null)
                timesheet = await _timesheetRepository.FindByIdAsync(request.TimesheetId);

            // If timesheet does not exist, create new timesheet for the current month
            if (timesheet is null)
                timesheet = await CreateTimesheet(request);

            // Check the project exists
            var project = await _projectRepository.FindByIdAsync(request.ProjectId);
            if (project is null)
                throw new InvalidOperationException($"Project {request.ProjectId} not found, aborting timesheet entry creation.");

            // All the reviewers of a project can review the timesheet entries
            var reviewers = project.Members.Where(x => x.MemberRole == MemberRoleEntityEnum.Reviewer).Select(x => (Reviewer)x).ToList();
            
            // Add timesheet entry to timesheet
            var entry = TimesheetEntry.Create(null, request.Date, request.Hours, project, reviewers, TimesheetStatus.Draft);
            timesheet.AddEntry(entry);

            // update timesheet
            await _timesheetRepository.ReplaceOneAsync(timesheet);

            // Return newly created timesheet entry
            return entry;
        }

        private async Task<Timesheet> CreateTimesheet(CreateTimesheetEntryCommand request)
        {
            var workerUser = await _userRepository.FindByIdAsync(request.UserId);
            if (workerUser is null)
                throw new InvalidOperationException($"User with id {request.UserId} not found");

            var worker = new Worker(workerUser.Id.ToString(), workerUser.Firstname, workerUser.Lastname, workerUser.Email, workerUser.ProfilePicture);
            var timesheet = Timesheet.Create(null, worker, DateTime.Now, DateTime.Now, null);

            await _timesheetRepository.InsertOneAsync(timesheet);
            return timesheet;
        }
    }

    public static implicit operator CreateTimesheetEntryCommand(CreateTimesheetEntryRequest request) =>
        new(request.TimesheetId, request.ProjectId, request.Hours, request.Date, request.UserId);
}
