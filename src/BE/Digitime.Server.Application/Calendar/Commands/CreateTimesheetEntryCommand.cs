using System;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Server.Domain.Users;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Shared.Contracts.Timesheets;
using EasyCaching.Core;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Calendar.Commands;

public record CreateTimesheetEntryCommand(string TimesheetId, string ProjectId, float Hours, DateTime Date, string UserId) : IRequest<CreateTimesheetEntryReponse>
{
    public class CreateTimesheetEntryCommandHandler : IRequestHandler<CreateTimesheetEntryCommand, CreateTimesheetEntryReponse>
    {
        private readonly IRepository<TimesheetEntity> _timesheetRepository;
        private readonly IRepository<ProjectEntity> _projectRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IEasyCachingProvider _cachingProvider;

        public CreateTimesheetEntryCommandHandler(
            IRepository<TimesheetEntity> timesheetRepository, 
            IRepository<ProjectEntity> projectRepository, 
            IRepository<UserEntity> userRepository, 
            IEasyCachingProvider cachingProvider)
        {
            _timesheetRepository = timesheetRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _cachingProvider = cachingProvider;
        }

        public async Task<CreateTimesheetEntryReponse> Handle(CreateTimesheetEntryCommand request, CancellationToken cancellationToken)
        {
            Timesheet timesheet = null;

            // Request existing timesheet if it exists
            if (request.TimesheetId is not null)
                timesheet = (await _timesheetRepository.FindByIdAsync(request.TimesheetId)).Adapt<Timesheet>();

            // If timesheet does not exist, create new timesheet for the current month
            if (timesheet is null)
                timesheet = await CreateTimesheet(request);

            // Check if the project exists
            var project = (await _projectRepository.FindByIdAsync(request.ProjectId)).Adapt<Domain.Projects.Project>();
            if (project is null)
                throw new InvalidOperationException($"Project with id {request.ProjectId} not found, aborting timesheet entry creation.");

            // Add timesheet entry to timesheet
            var entry = TimesheetEntry.Create(null, request.Date, request.Hours, project.Adapt<Project>(), TimesheetStatus.Draft);
            timesheet.AddEntry(entry);

            // update timesheet
            await _timesheetRepository.ReplaceOneAsync(timesheet.Adapt<TimesheetEntity>());

            // invalid the cache for the user 
            await _cachingProvider.RemoveAsync($"Calendar_{request.Date.Month}_{request.Date.Year}_{request.UserId}");

            // Return newly created timesheet entry
            return entry.Adapt<CreateTimesheetEntryReponse>();
        }

        private async Task<Timesheet> CreateTimesheet(CreateTimesheetEntryCommand request)
        {
            var workerUser = (await _userRepository.FindByIdAsync(request.UserId)).Adapt<User>();
            if (workerUser is null)
                throw new InvalidOperationException($"User with id {request.UserId} not found, aborting timesheet entry creation.");

            var worker = new Worker(workerUser.Id.ToString(), workerUser.Firstname, workerUser.Lastname, workerUser.Email, workerUser.ProfilePicture);
            var timesheet = new Timesheet(null, worker, DateTime.Now, DateTime.Now, null);

            await _timesheetRepository.InsertOneAsync(timesheet.Adapt<TimesheetEntity>());
            return timesheet;
        }
    }
}
