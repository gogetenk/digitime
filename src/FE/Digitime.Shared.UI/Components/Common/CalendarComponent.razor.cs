using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;
using Digitime.Shared.UI.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using CalendarDto = Digitime.Shared.Dto.CalendarDto;

namespace Digitime.Shared.UI.Components.Common;

public partial class CalendarComponent : ComponentBase
{
    [Inject] IDataStore DataStore { get; set; }
    [Inject] IJSRuntime Js { get; set; }
    [Inject] NavigationManager Navigation { get; set; }

    public CalendarDto CurrentMonthCalendarDays = new();
    public CalendarDto NextMonthCalendarDays = new();
    public List<TimesheetEntryDto> CurrentDayTimesheetEntries = new();
    public List<ProjectDto> ProjectList = new();
    public DateTime SelectedDate = DateTime.Now;
    public bool IsFormVisible;

    protected override async Task OnInitializedAsync()
    {
        var currentMonthTask = DataStore.GetCalendar(DateTime.Now, "FR");
        var projectsTask = DataStore.GetUserProjects();
        await Task.WhenAll(currentMonthTask, projectsTask);
        CurrentMonthCalendarDays = currentMonthTask.Result;
        ProjectList = projectsTask.Result.Projects;

        await SelectCurrentDay();
    }

    private async Task SelectCurrentDay()
    {
        if (CurrentMonthCalendarDays is null)
            return;

        foreach(var day in CurrentMonthCalendarDays.CalendarDays)
        {
            if (day is null) continue;
            if (day.Date.Date == SelectedDate.Date)
            {
                await OnDayClick(day);
                return;
            }
        }
    }

    private async Task OnDayClick(CalendarDayDto calendarDay)
    {
        if (calendarDay is null)
            return;

        SelectedDate = calendarDay.Date.ToLocalTime();
        CurrentDayTimesheetEntries = calendarDay.TimesheetEntries;
        IsFormVisible = false;
        await Js.InvokeVoidAsync("focusElement", "bt-addworktime");
    }

    private async Task AddWorktime()
    {
        IsFormVisible = true;
    }

    private async Task ViewAllProjects()
    {
        Navigation.NavigateTo("/projects");
    }

    private async Task OnAddedWorkTime(AddTimesheetEntryFormVM vm)
    {
        if (vm is null) 
            return;

        await DataStore.CreateTimesheetEntry(new CreateTimesheetEntryRequest()
        {
            Date = SelectedDate,
            Hours = vm.Hours,
            ProjectId = vm.Project.Id
        });
        IsFormVisible = false;
        await OnInitializedAsync();
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
