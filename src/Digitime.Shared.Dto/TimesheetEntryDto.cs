namespace Digitime.Shared.Dto;
public class TimesheetEntryDto
{
    public Guid ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
}
