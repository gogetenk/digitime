using System.Diagnostics.Metrics;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Client.Infrastructure.ViewModels;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Dto;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Digitime.Client.Infrastructure;

public class DataStore : IDataStore
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public DataStore(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
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
            Console.WriteLine(exc);
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
            Console.WriteLine(exc);
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
            Console.WriteLine(exc);
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
            Console.WriteLine(exc);
            return null;
        }
    }
}
