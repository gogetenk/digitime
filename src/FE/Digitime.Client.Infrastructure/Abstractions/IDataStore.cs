using Digitime.Client.Infrastructure.ViewModels;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;

namespace Digitime.Client.Infrastructure.Abstractions;

public interface IDataStore
{
    Task SynchronizeData();
    Task<CalendarDto> GetCalendar(DateTime date, string country);
    Task<CreateProjectResponse> CreateProject(CreateProjectVm project);
    Task<ProjectDto> GetProjectById(string id);
    Task<GetUserProjectsResponse> GetUserProjects();
    Task<CreateTimesheetEntryReponse> CreateTimesheetEntry(CreateTimesheetEntryRequest request);
    Task<List<DashboardIndicatorsDto>> GetDashboardIndicators();
    Task RegisterWithInvitation(string invitationToken, string id);
}
