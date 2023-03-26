using System.Net.Http.Json;
using Blazored.LocalStorage;
using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Client.Infrastructure.ViewModels;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Contracts.Workspaces;
using Digitime.Shared.Dto;
using Digitime.Shared.UI.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Digitime.Shared.UI.Data;

public class DataStore : IDataStore
{
    public ErrorNotification ErrorNotificationComponent { get; set; }

    private readonly ILogger<DataStore> _logger;
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ErrorNotification _errorNotification;

    public DataStore(ILogger<DataStore> logger, HttpClient httpClient, ILocalStorageService localStorage, ErrorNotification errorNotification)
    {
        _logger = logger;
        _httpClient = httpClient;
        _localStorage = localStorage;
        _errorNotification = errorNotification;
    }

    public async Task<T> ExecuteHttpRequestAsync<T>(Func<Task<HttpResponseMessage>> httpRequest)
    {
        try
        {
            var response = await httpRequest();

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;

                if ((int)statusCode >= 400 && (int)statusCode < 500)
                {
                    _errorNotification.ShowError(statusCode, error);
                }
                else if ((int)statusCode >= 500)
                {
                    _errorNotification.ShowTechnicalError();
                }

                return default(T);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            //ErrorNotificationComponent?.ShowTechnicalError();

            return default(T);
        }
    }

    public Task SynchronizeData()
    {
        throw new NotImplementedException();
    }

    public async Task<CalendarDto> GetCalendar(DateTime date, string country)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/timesheets/calendar?country={country}&month={date.Month}&year={date.Year}");
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
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task<CreateProjectResponse> CreateProject(CreateProjectVm project)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/projects", project);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateProjectResponse>(responseContent);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task<ProjectDto> GetProjectById(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/projects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ProjectDto>(responseContent);
            return resp;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task<GetUserProjectsResponse> GetUserProjects()
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/projects");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<GetUserProjectsResponse>(responseContent);
            return resp;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task<CreateTimesheetEntryReponse> CreateTimesheetEntry(CreateTimesheetEntryRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/timesheets/entry", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<CreateTimesheetEntryReponse>(responseContent);
            return resp;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task<List<DashboardIndicatorsDto>> GetDashboardIndicators()
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/indicators");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<List<DashboardIndicatorsDto>>(responseContent);
            return resp;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task RegisterWithInvitation(string invitationToken, string id)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/projects/register", new
            {
                invitationToken = invitationToken,
                id = id
            });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
        }
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/notifications");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<List<NotificationDto>>(responseContent);
            return resp;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task<WorkspaceDto> GetWorkspaceById(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/workspaces/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var statuscode = response.StatusCode;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<WorkspaceDto>(responseContent);
            return resp;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc);
            return null;
        }
    }

    public async Task InviteProjectMember(InviteMemberDto inviteMember)
    {
        //await ExecuteHttpRequestAsync(async () =>
        //{
        //    return await _httpClient.PostAsJsonAsync($"api/projects/invite", inviteMember);
        //});

        var response = await _httpClient.PostAsJsonAsync($"api/projects/invite", inviteMember);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            var statuscode = response.StatusCode;
            //ErrorNotificationComponent.ShowError(statuscode, error);
            throw new ApplicationException(error);
        }

    }
}
