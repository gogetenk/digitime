
using Newtonsoft.Json;

namespace Digitime.Shared.Contracts.Timesheets;

public record TimesheetsToReviewResponse
{
    [JsonProperty("timesheets")]
    public List<TimesheetContract> Timesheets { get; init; }
}

public record ProjectContract
{
    [JsonProperty("id")]
    public string Id { get; init; }

    [JsonProperty("title")]
    public string Title { get; init; }

    [JsonProperty("code")]
    public string Code { get; init; }
}

public record TimesheetContract
{
    [JsonProperty("worker")]
    public WorkerContract Worker { get; init; }

    [JsonProperty("totalHours")]
    public int TotalHours { get; init; }

    [JsonProperty("timesheetEntries")]
    public List<TimesheetEntryContract> TimesheetEntries { get; init; }
}

public record TimesheetEntryContract
{
    [JsonProperty("project")]
    public ProjectContract Project { get; init; }

    [JsonProperty("date")]
    public string Date { get; init; }

    [JsonProperty("hours")]
    public int Hours { get; init; }

    [JsonProperty("status")]
    public string Status { get; init; }
}

public record WorkerContract
{
    [JsonProperty("id")]
    public string Id { get; init; }

    [JsonProperty("fullname")]
    public string Fullname { get; init; }

    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; init; }
}

