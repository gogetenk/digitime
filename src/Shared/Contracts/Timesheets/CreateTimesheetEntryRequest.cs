using System.ComponentModel;

namespace Digitime.Shared.Contracts.Timesheets;

public record CreateTimesheetEntryRequest
{
    [DefaultValue("63961c72bd429e55f90ee4e7")]
    public string? TimesheetId { get; init; }

    [DefaultValue("6389b9592dd24486a037096a")]
    public string ProjectId { get; init; }

    [DefaultValue(8)]
    public float Hours { get; init; }

    [DefaultValue("2023-03-05T07:53:04.126Z")]
    public DateTime Date { get; init; }

    public string? UserId { get; init; }

}
