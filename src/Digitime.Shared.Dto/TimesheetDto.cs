using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;

public class TimesheetDto : IMapFrom<Timesheet>
{
    public Guid Id { get; set; }
    public List<TimesheetEntryDto> TimesheetEntries { get; set; } = new();
    public Guid ProjectId { get; private set; }
    public DateTime BeginDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public TimeSpan Period { get; set; }
    public int Hours => TimesheetEntries.Sum(x => x.Hours);
    public TimesheetStatusEnum Status { get; private set; }
    public Guid CreatorId { get; private set; }
    public Guid ApproverId { get; private set; }
    public DateTime? ApproveDate { get; private set; }
    public DateTime? CreateDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    public TimesheetDto()
    {
    }

    public enum TimesheetStatusEnum
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2
    }
}
