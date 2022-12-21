using System.Text.Json;
using Digitime.Server.Application.Abstractions;
using Digitime.Server.Infrastructure.Entities;
using EasyCaching.Core;
using Microsoft.Extensions.Configuration;

namespace Digitime.Server.Infrastructure.Http;

public class PublicHolidaysClient : IObtainPublicHolidays
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IEasyCachingProvider _cachingProvider;

    public PublicHolidaysClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IEasyCachingProvider cachingProvider)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _cachingProvider = cachingProvider;
    }

    public async Task<List<DateTime>> GetPublicHolidaysForSpecifiedMonthAndCountry(DateTime dateTime, string country)
    {
        // First, check if we have a cached version for the same date and country
        var cacheKey = $"{dateTime:yyyy-MM}-{country}";
        var cachedHolidays = await _cachingProvider.GetAsync<List<DateTime>>(cacheKey);
        if (cachedHolidays.HasValue)
            return cachedHolidays.Value;

        var publicHolidays = new List<DateTime>();
        var requestUri = $"api/v3/PublicHolidays/{dateTime.Year}/{country}";
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_configuration["ExternalApis:PublicHolidaysApi:Url"]);
        var response = await client.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var publicHolidaysResponse = JsonSerializer.Deserialize<List<PublicHoliday>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        if (publicHolidaysResponse != null)
        {
            foreach (var publicHoliday in publicHolidaysResponse)
                publicHolidays.Add(publicHoliday.Date);
        }

        // Add this to the cache so we don't fetch everytime
        await _cachingProvider.SetAsync(cacheKey, publicHolidays, TimeSpan.FromDays(365)); // Basically just store this forever. Public holidays won't change.

        return publicHolidays;
    }
}
