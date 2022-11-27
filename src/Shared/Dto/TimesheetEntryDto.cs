namespace Digitime.Shared.Dto;
public class TimesheetEntryDto
{
    public string ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
}
