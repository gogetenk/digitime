using Digitime.Shared.Dto;
using Newtonsoft.Json;

namespace Digitime.Shared.Contracts.Timesheets;

public record GetTimesheetForUserAndDateResponse
{
    [JsonProperty("workers")]
    public List<TimesheetWorker> Workers { get; set; }
}

public record TimesheetWorker
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("fullname")]
    public string Fullname { get; set; }

    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("totalHours")]
    public float TotalHours { get; set; }

    [JsonProperty("timesheetEntries")]
    public List<TimesheetEntryDto> TimesheetEntries { get; set; }
}