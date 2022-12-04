using Digitime.Server.Domain.Timesheet;
using static Digitime.Shared.Dto.TimesheetDto;

namespace Digitime.Shared.Dto;

public record TimesheetDto(
       string Id,
       DateTime BeginDate,
       DateTime EndDate,
       int TotalHours,
       TimesheetStatusEnum Status,
       string CreatorId,
       string ReviewerId,
       DateTime? ReviewDate,
       DateTime? CreateDate,
       DateTime? UpdateDate,
       CalendarDto Calendar)
{
    public enum TimesheetStatusEnum
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2
    }

    public static implicit operator TimesheetDto(Timesheet timesheet)
    {
        return new TimesheetDto(
            timesheet.Id,
            timesheet.BeginDate,
            timesheet.EndDate,
            timesheet.TotalHours,
            (TimesheetStatusEnum)timesheet.Status,
            timesheet.CreatorId,
            timesheet.ReviewerId,
            timesheet.ReviewDate,
            timesheet.CreateDate,
            timesheet.UpdateDate,
            null);
    }
}
