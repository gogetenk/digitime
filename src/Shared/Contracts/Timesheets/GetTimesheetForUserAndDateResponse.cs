using Digitime.Shared.Dto;

namespace Digitime.Shared.Contracts.Timesheets;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class CalendarDayDto
{
    public string DayOfWeek { get; set; }
    public string Date { get; set; }
    public bool IsPublicHoliday { get; set; }
    public bool IsWeekend { get; set; }
    public List<TimesheetEntryDto> TimesheetEntries { get; set; }
    public object TimesheetEntry { get; set; }
}

public class ProjectDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
}

public class GetTimesheetForUserAndDateResponse
{
    public List<TimesheetDto> Timesheets { get; set; }
}

public class TimesheetDto
{
    public string Id { get; set; }
    public string BeginDate { get; set; }
    public string EndDate { get; set; }
    public int TotalHours { get; set; }
    public string Status { get; set; }
    public WorkerDto Worker { get; set; }
    public string CreateDate { get; set; }
    public string UpdateDate { get; set; }
    public List<CalendarDayDto> CalendarDays { get; set; }
}


public class WorkerDto
{
    public string UserId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string ProfilePicture { get; set; }
}


