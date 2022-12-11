using Digitime.Shared.Contracts.Timesheets;

namespace Digitime.Shared.Dto;
public class TimesheetEntryDto
{
    public ProjectDto Project { get; set; }
    public int Hours { get; set; }
    public string Status { get; set; }
    public string ReviewDate { get; set; }
    public bool IsAutomated { get; set; }
}
