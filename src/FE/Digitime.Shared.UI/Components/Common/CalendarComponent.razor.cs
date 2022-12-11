using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using CalendarDto = Digitime.Shared.Dto.CalendarDto;
using System.Diagnostics;

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
        var sw = new Stopwatch();
        sw.Start();
        var currentMonthTask = GetCurrentMonthCalendar();
        var nextMonthTask = GetNextMonthCalendar();
        await Task.WhenAll(currentMonthTask, nextMonthTask);
        sw.Stop();

        Console.WriteLine($"Calendar rendering took {sw.ElapsedMilliseconds} ms");

        //_currentDayTimesheetEntries = _currentMonthCalendarDays.Single(x => x.Date == DateTime.Now.Date).FirstOrDefault()?.TimesheetEntries;
    }

    private async Task GetCurrentMonthCalendar()
    {
        var response = await _httpClient.GetAsync($"api/dashboard/calendar?country=fr&month={DateTime.Now.Month}&year={DateTime.Now.Year}&userId=638e0687ebcdd6848cbbf52f");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
            _currentMonthCalendarDays = JsonConvert.DeserializeObject<CalendarDto>(responseContent);
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            var statuscode = response.StatusCode;
            throw new Exception(error);
        }
    }
    
    private async Task GetNextMonthCalendar()
    {
        var response = await _httpClient.GetAsync($"api/dashboard/calendar?country=fr&month={DateTime.Now.AddMonths(1).Month}&year={DateTime.Now.AddMonths(1).Year}&userId=638e0687ebcdd6848cbbf52f");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                _nextMonthCalendarDays = JsonConvert.DeserializeObject<CalendarDto>(responseContent);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            var statuscode = response.StatusCode;
            throw new Exception(error);
        }
    }

    CalendarDto _currentMonthCalendarDays = new();
    CalendarDto _nextMonthCalendarDays = new();
    List<DateTime> _monthDates = new();
    List<DateTime> _publicHolidays = new();
    private List<TimesheetEntryDto> _currentDayTimesheetEntries = new();

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



    private void OnDayClick(CalendarDayDto calendarDay)
    {
        _currentDayTimesheetEntries = calendarDay.TimesheetEntries;
        //foreach (var workedProject in calendarDay.TimesheetEntries)
        //{
        //    _currentDayTimesheetEntries.Add(new TimesheetEntryDto()
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
