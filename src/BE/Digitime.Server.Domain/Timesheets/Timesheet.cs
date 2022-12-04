using System;
using System.Collections.Generic;
using System.Linq;
using Digitime.Server.Domain.Models;
using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;

namespace Digitime.Server.Domain.Timesheets;
public class Timesheet : AggregateRoot<string>
{
    private readonly List<TimesheetEntry> _timesheetEntries = new();

    public DateTime BeginDate => new DateTime(CreateDate.Date.Year, CreateDate.Date.Month, CreateDate.Date.Day);
    public DateTime EndDate => BeginDate.AddMonths(1).AddDays(-1);
    public Worker Worker { get; private set; }
    public float TotalHours => _timesheetEntries.Sum(x => x.Hours);
    public IReadOnlyList<TimesheetEntry> TimesheetEntries => _timesheetEntries.AsReadOnly();
    public DateTime UpdateDate { get; set; }
    public DateTime CreateDate { get; set; }
    public TimesheetStatus Status => GetEntriesStatus();

    private Timesheet(string id, Worker worker) : base(id)
    {
        Worker = worker;
    }

    public static Timesheet Create(string id, Worker worker)
        => new Timesheet(id, worker);

    public void AddEntry(TimesheetEntry entry)
    {
        if (_timesheetEntries.Any(x => x.Date == entry.Date))
            throw new InvalidOperationException($"Entry for date {entry.Date} already exists");

        _timesheetEntries.Add(entry);
        UpdateDate = DateTime.UtcNow;
    }

    public void RemoveEntry(TimesheetEntry entry)
    {
        if (!_timesheetEntries.Contains(entry))
            throw new InvalidOperationException($"Entry for date {entry.Date} does not exist");

        _timesheetEntries.Remove(entry);
    }

    public void UpdateEntry(TimesheetEntry entry)
    {
        if (!_timesheetEntries.Contains(entry))
            throw new InvalidOperationException($"Entry for date {entry.Date} does not exist");

        var index = _timesheetEntries.FindIndex(x => x.Date == entry.Date);
        _timesheetEntries[index] = entry;
        UpdateDate = DateTime.UtcNow;
    }

    private TimesheetStatus GetEntriesStatus()
    {
        if (_timesheetEntries.Any(x => x.Status == TimesheetStatus.Draft))
            return TimesheetStatus.Draft;

        if (_timesheetEntries.All(x => x.Status == TimesheetStatus.Submitted))
            return TimesheetStatus.Submitted;

        if (_timesheetEntries.All(x => x.Status == TimesheetStatus.Approved))
            return TimesheetStatus.Approved;

        if (_timesheetEntries.Any(x => x.Status == TimesheetStatus.Rejected))
            return TimesheetStatus.Rejected;

        return TimesheetStatus.Draft;
    }
}
