using System;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;
using Digitime.Shared.Contracts.Timesheets;
using EasyCaching.Core;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Timesheets.Commands;

public record CreateTimesheetEntryCommand(string TimesheetId, string ProjectId, float Hours, DateTime Date, string UserId) : IRequest<CreateTimesheetEntryReponse>
{
    public class CreateTimesheetEntryCommandHandler : IRequestHandler<CreateTimesheetEntryCommand, CreateTimesheetEntryReponse>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEasyCachingProvider _cachingProvider;

        public CreateTimesheetEntryCommandHandler(
            ITimesheetRepository timesheetRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IEasyCachingProvider cachingProvider)
        {
            _timesheetRepository = timesheetRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _cachingProvider = cachingProvider;
        }

        public async Task<CreateTimesheetEntryReponse> Handle(CreateTimesheetEntryCommand request, CancellationToken cancellationToken)
        {
            Timesheet? timesheet = null;

            // Request existing timesheet if it exists
            if (request.TimesheetId is not null)
                timesheet = await _timesheetRepository.GetbyIdAsync(request.TimesheetId);

            // If timesheet does not exist, create new timesheet for the current month
            if (timesheet is null)
                timesheet = await CreateTimesheet(request);

            // Check if the project exists
            var project = await _projectRepository.FindByIdAsync(request.ProjectId);
            if (project is null)
                throw new InvalidOperationException($"Project with id {request.ProjectId} not found, aborting timesheet entry creation.");

            // Add timesheet entry to timesheet
            var entry = TimesheetEntry.Create(null, request.Date.Date, request.Hours, project.Adapt<Project>(), TimesheetStatus.Draft);
            timesheet.AddEntry(entry);

            // update timesheet
            var updatedTimesheet = await _timesheetRepository.UpdateAsync(timesheet);

            // invalid the cache for the user 
            await _cachingProvider.RemoveAsync($"Calendar_{request.Date.Month}_{request.Date.Year}_{request.UserId}");

            // Return newly created timesheet entry
            return entry.Adapt<CreateTimesheetEntryReponse>();
        }

        private async Task<Timesheet> CreateTimesheet(CreateTimesheetEntryCommand request)
        {
            var workerUser = await _userRepository.GetbyExternalIdAsync(request.UserId);
            if (workerUser is null)
                throw new InvalidOperationException($"User with id {request.UserId} not found, aborting timesheet entry creation.");

            var worker = new Worker(request.UserId, workerUser.Firstname, workerUser.Lastname, workerUser.Email, workerUser.ProfilePicture);
            var timesheet = new Timesheet(null, worker, DateTime.Now, DateTime.Now, null);
            return await _timesheetRepository.CreateAsync(timesheet);
        }
    }
}
