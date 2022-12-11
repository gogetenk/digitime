using System.Diagnostics;
using Digitime.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using CalendarDto = Digitime.Shared.Dto.CalendarDto;

namespace Digitime.Shared.UI.Components.Common;

public partial class CalendarComponent : ComponentBase
{
    [Inject] HttpClient _httpClient { get; set; }

    public CalendarDto CurrentMonthCalendarDays = new();
    public CalendarDto NextMonthCalendarDays = new();
    public List<TimesheetEntryDto> CurrentDayTimesheetEntries = new();

    protected override async Task OnInitializedAsync()
    {
        var sw = new Stopwatch();
        sw.Start();
        var currentMonthTask = GetCurrentMonthCalendar();
        var nextMonthTask = GetNextMonthCalendar();
        await Task.WhenAll(currentMonthTask, nextMonthTask);
        sw.Stop();

        Console.WriteLine($"Calendar rendering took {sw.ElapsedMilliseconds} ms");
    }

    private async Task GetCurrentMonthCalendar()
    {
        var response = await _httpClient.GetAsync($"api/dashboard/calendar?country=fr&month={DateTime.Now.Month}&year={DateTime.Now.Year}&userId=638e0687ebcdd6848cbbf52f");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                CurrentMonthCalendarDays = JsonConvert.DeserializeObject<CalendarDto>(responseContent);
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

    private async Task GetNextMonthCalendar()
    {
        var response = await _httpClient.GetAsync($"api/dashboard/calendar?country=fr&month={DateTime.Now.AddMonths(1).Month}&year={DateTime.Now.AddMonths(1).Year}&userId=638e0687ebcdd6848cbbf52f");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                NextMonthCalendarDays = JsonConvert.DeserializeObject<CalendarDto>(responseContent);
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



    private void OnDayClick(CalendarDayDto calendarDay)
    {
        CurrentDayTimesheetEntries = calendarDay.TimesheetEntries;
    }
}
