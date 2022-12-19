using System.Text.Json;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Infrastructure.Entities;

namespace Digitime.Server.Infrastructure.Http;

public class PublicHolidaysRepository : IObtainPublicHolidays
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PublicHolidaysRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<DateTime>> GetPublicHolidaysForSpecifiedMonthAndCountry(DateTime dateTime, string country)
    {
        var publicHolidays = new List<DateTime>();
        var requestUri = $"/{dateTime.Year}/{country}";
        var response = await _httpClientFactory.CreateClient().GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var publicHolidaysResponse = JsonSerializer.Deserialize<List<PublicHoliday>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (publicHolidaysResponse != null)
            {
                foreach (var publicHoliday in publicHolidaysResponse)
                {
                    publicHolidays.Add(publicHoliday.Date);
                }
            }
            return publicHolidays;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
