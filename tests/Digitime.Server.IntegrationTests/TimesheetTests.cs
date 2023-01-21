using System.Net;
using System.Net.Http.Json;
using Digitime.Server.Application.Timesheets.Queries;
using Digitime.Server.IntegrationTests.Infrastructure;
using Digitime.Shared.Contracts.Timesheets;
using Digitime.Shared.Dto;

namespace Digitime.Server.IntegrationTests;

public class TimesheetTests : IntegrationTestBase
{
    private const string _BaseEndpointUri = "/api/timesheets";

    public TimesheetTests() : base(role)
    {
        var role = "Worker";
    }

    [Fact]
    public async Task CreateTimesheetEntry_WhenUserIsAWorker_Expect201()
    {
        // Arrange
        var client = Factory.CreateClient();
        var command = new CreateTimesheetEntryRequest
        {
            TimesheetId = "6392737298425fc69e63839a",
            ProjectId = "6389b9592dd24486a037096a",
            Date = DateTime.UtcNow,
            Hours = 8
        };

        // Act
        var response = await client.PostAsJsonAsync($"{_BaseEndpointUri}/entry", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetCurrentMonthTimesheet_NominalCase_Expect200()
    {
        // Arrange
        var client = Factory.CreateClient();
        var query = new GetCalendarQuery("FR", 12, 2022, "");

        // Act
        var response = await client.GetFromJsonAsync<CalendarDto>($"{_BaseEndpointUri}/calendar?Country={query.Country}&Month={query.Month}&Year={query.Year}");

        // Assert
        response.Should().NotBeNull();
        response.CalendarDays.Should().NotBeNull();
    }
}