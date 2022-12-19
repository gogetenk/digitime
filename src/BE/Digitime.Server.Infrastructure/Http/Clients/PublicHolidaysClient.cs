using System.Text.Json;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;

namespace Digitime.Server.Infrastructure.Http.Clients;

public class PublicHolidaysClient : IObtainPublicHolidays
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public PublicHolidaysClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<List<DateTime>> GetPublicHolidaysForSpecifiedMonthAndCountry(DateTime dateTime, string country)
    {
        var publicHolidays = new List<DateTime>();
        var requestUri = $"api/v3/PublicHolidays/{dateTime.Year}/{country}";
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_configuration["ExternalApis:PublicHolidaysApi:Url"]);
        var response = await client.GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var publicHolidaysResponse = JsonSerializer.Deserialize<List<PublicHoliday>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (publicHolidaysResponse != null)
            {
                foreach (var publicHoliday in publicHolidaysResponse)
                    publicHolidays.Add(publicHoliday.Date);
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
