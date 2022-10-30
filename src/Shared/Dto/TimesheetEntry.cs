namespace Digitime.Shared.Dto;
public class TimesheetEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectTitles { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
}
