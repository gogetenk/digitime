using Digitime.Shared.Contracts.Timesheets;

namespace Digitime.Shared.Dto;
public class TimesheetEntryDto
{
    public ProjectContract Project { get; set; }
    public float Hours { get; set; }
    public string Status { get; set; }
    public string ReviewDate { get; set; }
    public bool IsAutomated { get; set; }
}
