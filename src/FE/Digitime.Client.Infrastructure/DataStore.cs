using Blazored.LocalStorage;
using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Shared.Dto;
using Newtonsoft.Json;

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
}
