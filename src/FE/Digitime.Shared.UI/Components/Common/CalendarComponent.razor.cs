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
        _currentMonthCalendarDays = await GetCalendarDaysForMonth(DateTime.Now);
        _nextMonthCalendarDays = await GetCalendarDaysForMonth(DateTime.Now.AddMonths(1));
        //_currentDayTimesheetEntries = _currentMonthCalendarDays.Single(x => x.Date == DateTime.Now.Date).FirstOrDefault()?.TimesheetEntries;
    }

    private async Task<CalendarDay[,]> GetCalendarDaysForMonth(DateTime dateTime)
    {
        _monthDates = GetAllDaysOfSpecifiedMonth(dateTime);
        _publicHolidays = await GetPublicHolidaysForSpecifiedMonthAndCountry(dateTime, "FR");
        var calendarDays = new CalendarDay[6, 7];
        var firstDayPositionInWeek = ((int)_monthDates.FirstOrDefault().DayOfWeek) - 1;
        var weekIndex = 0;
        var weekdayIndex = firstDayPositionInWeek;

        // Iterate through days of the month and fill an inmemory calendar
        foreach (var day in _monthDates)
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

    CalendarDay[,] _currentMonthCalendarDays = new CalendarDay[6, 7];
    CalendarDay[,] _nextMonthCalendarDays = new CalendarDay[6, 7];
    List<DateTime> _monthDates = new();
    List<DateTime> _publicHolidays = new();
    private List<TimeSheetEntry> _currentDayTimesheetEntries = new();

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
        var workedProjects = new List<WorkedProject>();
        Random rnd = new Random();
        int random = rnd.Next(0, 2);
        if (random is 1)
            workedProjects.Add(new WorkedProject()
            {
                Id = Guid.NewGuid(),
                Title = "CIRAC",
                WorkedHours = 8
            });

        return new CalendarDay()
        {
            WorkedProjects = workedProjects,
            Date = dateTime,
            DayOfWeek = dateTime.DayOfWeek,
            IsPublicHoliday = _publicHolidays.Contains(dateTime)
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

    private void OnDayClick(CalendarDay calendarDay)
    {
        _currentDayTimesheetEntries = new List<TimeSheetEntry>();
        foreach (var workedProject in calendarDay.WorkedProjects)
        {
            _currentDayTimesheetEntries.Add(new TimeSheetEntry()
            {
                Id = workedProject.Id,
                Project = workedProject.Title,
                Hours = workedProject.WorkedHours,
                Date = calendarDay.Date
            });
        }
    }

    public void GetTimesheetEntriesForDay(DateTime dateTime)
    {
        //var requestUri = $"https://localhost:44300/api/timesheetentries?date={dateTime.ToString("yyyy-MM-dd")}";
        //var response = await HttpClient.GetAsync(requestUri);
        //if (response.IsSuccessStatusCode)
        //{
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var timesheetEntries = JsonSerializer.Deserialize<List<TimeSheetEntry>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        //    return timesheetEntries;
        //}
        //else
        //{
        //    var error = await response.Content.ReadAsStringAsync();
        //    var statuscode = response.StatusCode;
        //    throw new Exception(error);
        //}

        // Mock a list of timesheet entries
        var timesheetEntries = new List<TimeSheetEntry>();
        // Add random timesheet entries for testing purpose
        timesheetEntries.Add(new TimeSheetEntry()
        {
            Id = Guid.NewGuid(),
            Project = "CIRAC",
            Date = dateTime,
            Hours = 8
        });
        _currentDayTimesheetEntries = timesheetEntries;
    }
}

public class PublicHoliday
{
    public DateTime Date { get; set; }
    public string LocalName { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
}

public class TimeSheetEntry
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Project { get; set; }
    public float Hours { get; set; }
}
