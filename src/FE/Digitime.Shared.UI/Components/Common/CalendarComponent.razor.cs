using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Shared.Dto;
using Microsoft.AspNetCore.Components;
using CalendarDto = Digitime.Shared.Dto.CalendarDto;

namespace Digitime.Shared.UI.Components.Common;

public partial class CalendarComponent : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] IDataStore DataStore { get; set; }

    public CalendarDto CurrentMonthCalendarDays = new();
    public CalendarDto NextMonthCalendarDays = new();
    public List<TimesheetEntryDto> CurrentDayTimesheetEntries = new();
    public DateTime SelectedDate = DateTime.Now;

    protected override async Task OnInitializedAsync()
    {
        var currentMonthTask = DataStore.GetCalendar(DateTime.Now, "FR");
        var nextMonthTask = DataStore.GetCalendar(DateTime.Now.AddMonths(1), "FR");
        await Task.WhenAll(currentMonthTask, nextMonthTask);
        CurrentMonthCalendarDays = currentMonthTask.Result;
        NextMonthCalendarDays = nextMonthTask.Result;
    }

    private void OnDayClick(CalendarDayDto calendarDay)
    {
        if (calendarDay is null)
            return;

        SelectedDate = calendarDay.Date;
        CurrentDayTimesheetEntries = calendarDay.TimesheetEntries;
    }

    public string FirstCharToUpperAsSpan(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        Span<char> destination = stackalloc char[1];
        input.AsSpan(0, 1).ToUpperInvariant(destination);
        return $"{destination}{input.AsSpan(1)}";
    }
}
