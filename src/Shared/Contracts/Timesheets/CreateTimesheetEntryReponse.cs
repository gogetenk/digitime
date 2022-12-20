using Digitime.Server.Domain.Timesheets.Entities;
using Digitime.Server.Domain.Timesheets.ValueObjects;

namespace Digitime.Shared.Contracts.Timesheets;

public record CreateTimesheetEntryReponse(string Id, DateTime Date, float Hours, ProjectContract Project, TimesheetStatus Status);