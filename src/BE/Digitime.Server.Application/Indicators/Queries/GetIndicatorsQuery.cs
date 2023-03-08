using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Indicators;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Shared.Dto;
using Mapster;
using MediatR;

namespace Digitime.Server.Application.Indicators.Queries;

public record GetIndicatorsQuery(string UserId) : IRequest<List<DashboardIndicatorsDto>>, ICacheableRequest
{
    public class GetIndicatorsQueryHandler : IRequestHandler<GetIndicatorsQuery, List<DashboardIndicatorsDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITimesheetRepository _timesheetRepository;

        public GetIndicatorsQueryHandler(IUserRepository userRepository, IProjectRepository projectRepository, ITimesheetRepository timesheetRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _timesheetRepository = timesheetRepository;
        }
        public async Task<List<DashboardIndicatorsDto>> Handle(GetIndicatorsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetbyExternalIdAsync(request.UserId);
            if (user is null)
                throw new ApplicationException("User not found.");

            var projectsTask = _projectRepository.GetProjectsByUserId(request.UserId);
            var currentMonthTimesheetsTask = _timesheetRepository.GetbyUserAndMonthOfyear(request.UserId, DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            var lastMonthTimesheetsTask = _timesheetRepository.GetbyUserAndMonthOfyear(request.UserId, DateTime.UtcNow.AddMonths(-1).Month, DateTime.UtcNow.AddMonths(-1).Year);

            var totalProjects = CreateTotalProjectsIndicator(await projectsTask);
            var pendingHours = CreatePendingHoursIndicator(await currentMonthTimesheetsTask);
            var totalHours = CreateTotalHoursIndicator(await currentMonthTimesheetsTask, await lastMonthTimesheetsTask);

            var indicators = new List<Indicator>
            {
                totalProjects,
                pendingHours,
                totalHours
            };

            return indicators.Adapt<List<DashboardIndicatorsDto>>();
        }

        // Total projects : projects the user is a member of
        private static Indicator CreateTotalProjectsIndicator(List<Project>? projects)
        {
            var totalProjectsValue = (projects is not null) ? projects.Count() : 0;
            return new Indicator("Total Projects", totalProjectsValue.ToString(), null, null, null);
        }

        // Pending hours : hours the user submitted but are not validated yet
        private static Indicator CreatePendingHoursIndicator(List<Timesheet> timesheets)
        {
            var pendingHoursValue = 0f;
            if (timesheets is not null && timesheets.Any())
                pendingHoursValue = timesheets
                .Where(t => t.Status == TimesheetStatus.Draft)
                .Sum(t => t.TotalHours);

            return new Indicator("Pending Hours this month", pendingHoursValue.ToString(), null, null, null);
        }

        // Total hours : hours the user submitted and validated this month
        private static Indicator CreateTotalHoursIndicator(List<Timesheet> currentMonthTimesheets, List<Timesheet> lastMonthTimesheets)
        {
            var totalHoursValue = 0f;
            var variation = 0f;

            if (currentMonthTimesheets is not null && currentMonthTimesheets.Any())
                totalHoursValue = currentMonthTimesheets
                    .Where(t => t.Status == TimesheetStatus.Approved)
                    .Sum(t => t.TotalHours);

            if (lastMonthTimesheets is not null && lastMonthTimesheets.Any())
                variation = totalHoursValue - lastMonthTimesheets.Sum(t => t.TotalHours);

            return new Indicator("Validated Hours this month", totalHoursValue.ToString(), null, variation, null);
        }
    }

    public DateTime? GetCacheExpiration()
    {
        return DateTime.UtcNow.AddHours(1);
    }

    public string GetCacheKey()
    {
        return $"GetIndicatorsQuery_{UserId}";
    }
}
