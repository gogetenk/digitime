using Digitime.Server.Domain.Models;
using static Digitime.Shared.Dto.TimesheetDto;

namespace Digitime.Shared.Dto;

public record TimesheetDto(
    string Id, 
    List<TimesheetEntryDto> TimesheetEntries, 
    DateTime BeginDate, 
    DateTime EndDate, 
    TimeSpan Period, 
    int Hours,
    TimesheetStatusEnum Status, 
    string CreatorId, 
    string ApproverId, 
    DateTime? ApproveDate,
    DateTime? CreateDate, 
    DateTime? UpdateDate)
{

    public enum TimesheetStatusEnum
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2
    }
     
    public static implicit operator TimesheetDto(Timesheet timesheet) 
        => new TimesheetDto(timesheet.Id, 
            timesheet.TimesheetEntries.Select(x => (TimesheetEntryDto)x).ToList(),
            timesheet.BeginDate, 
            timesheet.EndDate, 
            timesheet.Period, 
            timesheet.Hours, 
            (TimesheetStatusEnum)timesheet.Status, 
            timesheet.CreatorId, 
            timesheet.ApproverId, 
            timesheet.ApproveDate, 
            timesheet.CreateDate, 
            timesheet.UpdateDate);
}
