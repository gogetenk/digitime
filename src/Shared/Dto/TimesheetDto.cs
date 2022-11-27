namespace Digitime.Shared.Dto;

public record TimesheetDto
{
    public string Id { get; set; }
    public List<TimesheetEntryDto> TimesheetEntries { get; set; } = new();
    public string ProjectId { get; private set; }
    public DateTime BeginDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public TimeSpan Period { get; set; }
    public int Hours => TimesheetEntries.Sum(x => x.Hours);
    public TimesheetStatusEnum Status { get; private set; }
    public string CreatorId { get; private set; }
    public string ApproverId { get; private set; }
    public DateTime? ApproveDate { get; private set; }
    public DateTime? CreateDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    public enum TimesheetStatusEnum
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2
    }
}
