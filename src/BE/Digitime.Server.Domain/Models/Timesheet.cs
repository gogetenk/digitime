using System;
using System.Collections.Generic;

namespace Digitime.Server.Domain.Models;

public class Timesheet : AggregateRoot<Guid>
{
    public Guid ProjectId { get; private set; }
    public TimeSpan Period { get; private set; }
    public List<TimesheetEntry> TimesheetEntries { get; private set; } = new ();
    public int Hours { get; private set; }
    public TimesheetStatusEnum Status { get; private set; }
    public Guid ReviewerId { get; private set; }
    public string ReviewerComment { get; private set; }
    public DateTime? ReviewDate { get; private set; }
    public Guid ApproverId { get; private set; }
    public string ApproverComment { get; private set; }
    public DateTime? ApproveDate { get; private set; }
    public Guid RejecterId { get; private set; }
    public string RejecterComment { get; private set; }
    public DateTime? RejectDate { get; private set; }
    public Guid CancelerId { get; private set; }
    public string CancelerComment { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public Guid CreatorId { get; private set; }
    public DateTime? CreateDate { get; private set; }
    public Guid UpdaterId { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    public Timesheet(Guid id) : base(id)
    {
    }

    public enum TimesheetStatusEnum
    {
        Draft = 0,
        Submitted = 1,
        Reviewed = 2,
        Approved = 3,
        Rejected = 4,
        Cancelled = 5
    }

    public void AddTimesheetEntry(TimesheetEntry timesheetEntry)
    {
        TimesheetEntries.Add(timesheetEntry);
    }

    public void RemoveTimesheetEntry(TimesheetEntry timesheetEntry)
    {
        TimesheetEntries.Remove(timesheetEntry);
    }

    public void Submit(Guid userId)
    {
        Status = TimesheetStatusEnum.Submitted;
        UpdaterId = userId;
        UpdateDate = DateTime.UtcNow;

        // Send email to reviewer
        
    }

    public void Review(Guid userId, string comment)
    {
        Status = TimesheetStatusEnum.Reviewed;
        ReviewerId = userId;
        ReviewerComment = comment;
        ReviewDate = DateTime.UtcNow;
        UpdaterId = userId;
        UpdateDate = DateTime.UtcNow;

        // Send email to approver
    }

    public void Approve(Guid userId, string comment)
    {
        Status = TimesheetStatusEnum.Approved;
        ApproverId = userId;
        ApproverComment = comment;
        ApproveDate = DateTime.UtcNow;
        UpdaterId = userId;
        UpdateDate = DateTime.UtcNow;

        // Send email to submitter
    }

    public void Reject(Guid userId, string comment)
    {
        Status = TimesheetStatusEnum.Rejected;
        RejecterId = userId;
        RejecterComment = comment;
        RejectDate = DateTime.UtcNow;
        UpdaterId = userId;
        UpdateDate = DateTime.UtcNow;

        // Send email to submitter
    }

    public void Cancel(Guid userId, string comment)
    {
        Status = TimesheetStatusEnum.Cancelled;
        CancelerId = userId;
        CancelerComment = comment;
        CancelDate = DateTime.UtcNow;
        UpdaterId = userId;
        UpdateDate = DateTime.UtcNow;

        // Send email to submitter
    }

    public void Update(Guid userId)
    {
        UpdaterId = userId;
        UpdateDate = DateTime.UtcNow;
    }

    public void Create(Guid userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow;
    }

    public void UpdateTimesheetEntries(List<TimesheetEntry> timesheetEntries)
    {
        TimesheetEntries = timesheetEntries;
    }
}
