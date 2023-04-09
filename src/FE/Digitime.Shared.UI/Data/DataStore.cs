using System.Net.Http.Json;
using Blazored.LocalStorage;
using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Client.Infrastructure.ViewModels;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Contracts.Workspaces;
using Digitime.Shared.Dto;
using Digitime.Shared.Exceptions;
using Digitime.Shared.UI.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Digitime.Shared.UI.Data;

public class DataStore : IDataStore
{
    public ErrorNotification ErrorNotificationComponent { get; set; }

    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    private const string ApiTimesheetsCalendar = "api/timesheets/calendar";
    private const string ApiProjects = "api/projects";
    private const string ApiTimesheetsEntry = "api/timesheets/entry";
    private const string ApiIndicators = "api/indicators";
    private const string ApiProjectsRegister = "api/projects/register";
    private const string ApiNotifications = "api/notifications";
    private const string ApiWorkspaces = "api/workspaces";
    private const string ApiProjectsInvite = "api/projects/invite";

    public DataStore(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<T> ExecuteHttpRequestAsync<T>(Func<Task<HttpResponseMessage>> httpRequest)
    {
        var response = await httpRequest();
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new BackendException(error);
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task ExecuteHttpRequestAsync(Func<Task<HttpResponseMessage>> httpRequest)
    {
        var response = await httpRequest();
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new BackendException(error);
        }
    }

    public Task SynchronizeData()
    {
        throw new NotImplementedException();
    }

    public async Task<CalendarDto> GetCalendar(DateTime date, string country)
    {
        var response = await _httpClient.GetAsync($"{ApiTimesheetsCalendar}?country={country}&month={date.Month}&year={date.Year}");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            var statuscode = response.StatusCode;
            return await _localStorage.GetItemAsync<CalendarDto>($"calendar_{date.Month}_{date.Year}");
        }
        var responseContent = await response.Content.ReadAsStringAsync();
        await _localStorage.SetItemAsStringAsync($"calendar_{date.Month}_{date.Year}", responseContent);
        return JsonConvert.DeserializeObject<CalendarDto>(responseContent);
    }

    public async Task<CreateProjectResponse> CreateProject(CreateProjectVm project)
    {
        return await ExecuteHttpRequestAsync<CreateProjectResponse>(async () => await _httpClient.PostAsJsonAsync(ApiProjects, project));
    }

    public async Task<ProjectDto> GetProjectById(string id)
    {
        return await ExecuteHttpRequestAsync<ProjectDto>(async () => await _httpClient.GetAsync($"{ApiProjects}/{id}"));
    }

    public async Task<GetUserProjectsResponse> GetUserProjects()
    {
        return await ExecuteHttpRequestAsync<GetUserProjectsResponse>(async () => await _httpClient.GetAsync(ApiProjects));
    }

    public async Task<CreateTimesheetEntryReponse> CreateTimesheetEntry(CreateTimesheetEntryRequest request)
    {
        return await ExecuteHttpRequestAsync<CreateTimesheetEntryReponse>(async () => await _httpClient.PostAsJsonAsync($"{ApiTimesheetsEntry}/entry", request));
    }

    public async Task<List<DashboardIndicatorsDto>> GetDashboardIndicators()
    {
        return await ExecuteHttpRequestAsync<List<DashboardIndicatorsDto>>(async () => await _httpClient.GetAsync($"{ApiIndicators}"));
    }

    public async Task RegisterWithInvitation(string invitationToken, string id)
    {
        await ExecuteHttpRequestAsync(async () => await _httpClient.PostAsJsonAsync($"{ApiProjects}/register", new
        {
            invitationToken,
            id
        }));
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync()
    {
        return await ExecuteHttpRequestAsync<List<NotificationDto>>(async () => await _httpClient.GetAsync(ApiNotifications));
    }

    public async Task<WorkspaceDto> GetWorkspaceById(string id)
    {
        return await ExecuteHttpRequestAsync<WorkspaceDto>(async () => await _httpClient.GetAsync($"{ApiWorkspaces}/{id}"));
    }

    public async Task InviteProjectMember(InviteMemberDto inviteMember)
    {
        await ExecuteHttpRequestAsync(async () => await _httpClient.PostAsJsonAsync($"{ApiProjects}/invite", inviteMember));
    }
}
