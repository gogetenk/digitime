using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitime.Server.Domain.Models;

public class Timesheet : AggregateRoot<string>
{
    private List<TimesheetEntry> _timesheetEntries = new();
    public IReadOnlyList<TimesheetEntry> TimesheetEntries => _timesheetEntries.AsReadOnly();
    public TimeSpan Period => EndDate - BeginDate;
    public string ProjectId { get; private set; }
    public DateTime BeginDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int Hours => _timesheetEntries.Sum(x => x.Hours);
    public TimesheetStatusEnum Status { get; private set; }
    public string CreatorId { get; private set; }
    public string ApproverId { get; private set; }
    public DateTime? ApproveDate { get; private set; }
    public DateTime? CreateDate { get; private set; }
    public DateTime? UpdateDate { get; private set; }

    public Timesheet(string id) : base(id)
    {
    }

    public Timesheet(string id, DateTime beginDate) : base(id)
    {
        BeginDate = beginDate;
        EndDate = beginDate.AddMonths(1).AddDays(-1);
        CreateDate = DateTime.UtcNow;
        UpdateDate = DateTime.UtcNow;
    }

    public enum TimesheetStatusEnum
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2,
    }
    public void AddTimesheetEntry(DateTime date, int hours, string projectId, string projectTitle)
    {
        var timesheetEntry = new TimesheetEntry(date, hours, projectId, projectTitle);
        if (_timesheetEntries.Contains(timesheetEntry))
            throw new InvalidOperationException("Timesheet entry already exists");


        _timesheetEntries.Add(timesheetEntry);
        UpdateDate = DateTime.UtcNow;
    }

    public void RemoveTimesheetEntry(TimesheetEntry timesheetEntry)
    {
        _timesheetEntries.Remove(timesheetEntry);
        UpdateDate = DateTime.UtcNow;
    }

    public void Approve(string userId, string comment)
    {
        if (Status == TimesheetStatusEnum.Approved)
            throw new Exception("Timesheet is already approved");

        Status = TimesheetStatusEnum.Approved;
        ApproverId = userId;
        ApproveDate = DateTime.UtcNow;
        UpdateDate = DateTime.UtcNow;

        // Send email to submitter
    }

    public void Create(string userId)
    {
        Status = TimesheetStatusEnum.Submitted;
        CreatorId = userId;
        CreateDate = DateTime.UtcNow;
    }

    public void UpdateTimesheetEntries(List<TimesheetEntry> timesheetEntries)
    {
        _timesheetEntries = timesheetEntries;
        UpdateDate = DateTime.UtcNow;
    }
}
