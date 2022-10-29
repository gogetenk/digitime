using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Digitime.Shared.UI.Components.Common;

public partial class CalendarComponent : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }

    protected override async Task OnInitializedAsync()
    {

        // Get days in current month
        // Get holidays
        // Get worked days for the month from API
        /// Calendar rendering algorithm
        //// Create a 2 dimensions array that will hold all the values and represent
        //// Place all the current month days into that 2D array
        //// Get the first day of the month and place it on a 2 dimension array
        //// Loop from this starting day to add all days until the end of month
        //// Once the calendar is rendered in memory, trigger the UI rendering
        ///
        CurrentMonthCalendarDays = await GetCalendarDaysForMonth(DateTime.Now);
        NextMonthCalendarDays = await GetCalendarDaysForMonth(DateTime.Now.AddMonths(1));
    }

    private async Task<CalendarDay[,]> GetCalendarDaysForMonth(DateTime dateTime)
    {
        MonthDates = GetAllDaysOfSpecifiedMonth(dateTime);
        PublicHolidays = await GetPublicHolidaysForSpecifiedMonthAndCountry(dateTime, "FR");
        var calendarDays = new CalendarDay[6, 7];
        var firstDayPositionInWeek = ((int)MonthDates.FirstOrDefault().DayOfWeek) - 1;
        var weekIndex = 0;
        var weekdayIndex = firstDayPositionInWeek;

        // Iterate through days of the month and fill an inmemory calendar
        foreach (var day in MonthDates)
        {
            if (weekdayIndex == 7)
            {
                weekdayIndex = 0;
                weekIndex++;
            }

            calendarDays[weekIndex, weekdayIndex] = CreateFilledCalendarDay(day);
            weekdayIndex++;
        }
        
        return calendarDays;
    }

    CalendarDay[,] CurrentMonthCalendarDays = new CalendarDay[6,7];
    CalendarDay[,] NextMonthCalendarDays = new CalendarDay[6,7];
    List<DateTime> MonthDates = new List<DateTime>();
    List<DateTime> PublicHolidays = new List<DateTime>();

    class CalendarDay
    {
        public List<WorkedProject> WorkedProjects = new List<WorkedProject>();
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime Date { get; set; }
        public bool IsPublicHoliday { get; set; }
    }

    class WorkedProject
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public float WorkedHours { get; set; }
    }

    CalendarDay CreateFilledCalendarDay(DateTime dateTime)
    {
        return new CalendarDay()
        {
            WorkedProjects = new List<WorkedProject>()
                {
                    new WorkedProject()
                    {
                        Id = Guid.NewGuid(),
                        Title = "CIRAC",
                        WorkedHours = 8
                    }
                },
            Date = dateTime,
            DayOfWeek = dateTime.DayOfWeek,
            IsPublicHoliday = PublicHolidays.Contains(dateTime)
        };
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
        var requestUri = $"https://date.nager.at/api/v3/PublicHolidays/{dateTime.Year}/{country}";
        var response = await HttpClient.GetAsync(requestUri);
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
            var statuscode = response.StatusCode;
            throw new Exception(error);
        }
    }
}

public class PublicHoliday
{
    public DateTime Date { get; set; }
    public string LocalName { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
}
