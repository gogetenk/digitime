using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;
using MediatR;

namespace Digitime.Server.Application.Timesheets.Queries;

public record GetTimesheetsToReviewQuery(string? UserId = null) : IRequest<GetTimesheetForUserAndDateResponse>
{
    public class GetTimesheetsToReviewQueryHandler : IRequestHandler<GetTimesheetsToReviewQuery, GetTimesheetForUserAndDateResponse>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IProjectRepository _projectRepository;

        public GetTimesheetsToReviewQueryHandler(ITimesheetRepository timesheetRepository, IProjectRepository projectRepository)
        {
            _timesheetRepository = timesheetRepository;
            _projectRepository = projectRepository;
        }

        public async Task<GetTimesheetForUserAndDateResponse> Handle(GetTimesheetsToReviewQuery request, CancellationToken cancellationToken)
        {
            // First we get the projects the user is a reviewer of
            var projects = await _projectRepository.GetProjectsByReviewerId(request.UserId);

            // Then we get all the timesheets waiting for approval in those projects
            var timesheets = await _timesheetRepository.GetTimesheetsFromProjectIds(projects.Select(x => x.Id).ToArray());

            // Get all the workers that have timesheets waiting for approval
            var timesheetWorkers = timesheets.Select(x => x.Worker).Distinct().ToList();
            var timesheetsToReturn = timesheets
                            .SelectMany(y => y.TimesheetEntries)
                            .Select(y => new TimesheetEntryDto()
                            {
                                ReviewDate = y.Date.ToString("yyyy-MM-dd"),
                                Hours = y.Hours,
                                Status = Enum.GetName(y.Status),
                                Project = new ProjectContract(y.Project.Id, y.Project.Title, y.Project.Code)
                            })
                            .ToList();

            return new GetTimesheetForUserAndDateResponse()
            {
                Workers = timesheetWorkers.Select(x
                    => new TimesheetWorker
                    {
                        Email = x.Email,
                        Fullname = $"{x.FirstName} {x.LastName.ToUpper()}",
                        Id = x.UserId,
                        ProfilePicture = x.ProfilePicture,
                        TotalHours = timesheetsToReturn.Sum(y => y.Hours),
                        TimesheetEntries = timesheetsToReturn
                    })
                .ToList(),
            };
        }
    }
}
