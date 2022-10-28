using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Digitime.Shared.UI.Components.Common;

public partial class CalendarComponent : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }

    private List<DateTime> _daysOfCurrentfMonth = new();

    protected override Task OnInitializedAsync()
    {
        _daysOfCurrentfMonth = GetAllDaysOfSpecifiedMonth(DateTime.Now);
        var t = GetPublicHolidaysForSpecifiedMonthAndCountry(DateTime.Now, "FR");

        return base.OnInitializedAsync();
    }

    private int CalculateDaysBetweenDates(DateTime startTime, DateTime endTime)
    {
        return (int)(endTime - startTime).TotalDays;
    }

    private List<DateTime> GetAllDaysOfSpecifiedMonth(DateTime dateTime)
    {
        var days = new List<DateTime>();
        var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var daysInMonth = CalculateDaysBetweenDates(firstDayOfMonth, lastDayOfMonth);
        for (int i = 0; i <= daysInMonth; i++)
        {
            days.Add(firstDayOfMonth.AddDays(i));
        }
        return days;
    }

    public async Task<List<DateTime>> GetPublicHolidaysForSpecifiedMonthAndCountry(DateTime dateTime, string country)
    {
        var publicHolidays = new List<DateTime>();
        var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var daysInMonth = CalculateDaysBetweenDates(firstDayOfMonth, lastDayOfMonth);
        var requestUri = $"https://date.nager.at/api/v3/PublicHolidays/{dateTime.Year}/{country}";
        var response = await HttpClient.GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var publicHolidaysResponse = JsonSerializer.Deserialize<List<PublicHoliday>>(responseContent);
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
            var statuscode = response.StatusCode;
            throw new Exception(error);
        }
        return null;
    }
}

public class PublicHoliday
{
    public DateTime Date { get; set; }
    public string LocalName { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
    public string Fixed { get; set; }
    public string Global { get; set; }
    public string Counties { get; set; }
    public string LaunchYear { get; set; }
    public string Type { get; set; }
}
