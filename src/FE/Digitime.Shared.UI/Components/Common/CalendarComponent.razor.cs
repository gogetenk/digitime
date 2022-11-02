using System.Net.Http.Json;
using System.Text.Json;
using Digitime.Shared.Dto;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Calendar = Digitime.Shared.Dto.Calendar;

namespace Digitime.Shared.UI.Components.Common;

public partial class CalendarComponent : ComponentBase
{
    [Inject] HttpClient _httpClient { get; set; }

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

        await GetCurrentMonthCalendar();
        //_currentDayTimesheetEntries = _currentMonthCalendarDays.Single(x => x.Date == DateTime.Now.Date).FirstOrDefault()?.TimesheetEntries;
    }

    private async Task GetCurrentMonthCalendar()
    {
        var response = await _httpClient.GetAsync("api/dashboard/calendar?country=fr&month=1&year=2021");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _currentMonthCalendarDays = JsonConvert.DeserializeObject<Calendar>(responseContent);
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            var statuscode = response.StatusCode;
            throw new Exception(error);
        }
    }

    Calendar _currentMonthCalendarDays = new();
    Calendar _nextMonthCalendarDays = new();
    List<DateTime> _monthDates = new();
    List<DateTime> _publicHolidays = new();
    private List<TimesheetEntry> _currentDayTimesheetEntries = new();

    //CalendarDay CreateFilledCalendarDay(DateTime dateTime)
    //{
    //    var workedProjects = new List<WorkedProject>();
    //    Random rnd = new Random();
    //    int random = rnd.Next(0, 2);
    //    if (random is 1)
    //        workedProjects.Add(new WorkedProject()
    //        {
    //            Id = Guid.NewGuid(),
    //            Title = "CIRAC",
    //            WorkedHours = 8
    //        });

    //    return new CalendarDay()
    //    {
    //        WorkedProjects = workedProjects,
    //        Date = dateTime,
    //        DayOfWeek = dateTime.DayOfWeek,
    //        IsPublicHoliday = _publicHolidays.Contains(dateTime)
    //    };
    //}



    private void OnDayClick(CalendarDay calendarDay)
    {
        _currentDayTimesheetEntries = new List<TimesheetEntry>();
        //foreach (var workedProject in calendarDay.WorkedProjects)
        //{
        //    _currentDayTimesheetEntries.Add(new TimeSheetEntry()
        //    {
        //        Id = workedProject.Id,
        //        Project = workedProject.Title,
        //        Hours = workedProject.WorkedHours,
        //        Date = calendarDay.Date
        //    });
        //}
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
        //var timesheetEntries = new List<TimeSheetEntry>();
        //// Add random timesheet entries for testing purpose
        //timesheetEntries.Add(new TimeSheetEntry()
        //{
        //    Id = Guid.NewGuid(),
        //    Project = "CIRAC",
        //    Date = dateTime,
        //    Hours = 8
        //});
        //_currentDayTimesheetEntries = timesheetEntries;
    }
}
