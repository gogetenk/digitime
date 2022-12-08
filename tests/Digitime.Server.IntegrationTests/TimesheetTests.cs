using System.Net;
using System.Net.Http.Json;
using Digitime.Shared.Contracts.Timesheets;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Digitime.Server.IntegrationTests;

public class TimesheetTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string _BaseEndpointUri = "/api/timesheets/entry";

    public TimesheetTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Fact]
    public async Task CreateTimesheetEntry_NominalCase_Expect201()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreateTimesheetEntryRequest
        {
            UserId = "638e0687ebcdd6848cbbf52f",
            TimesheetId = "6392737298425fc69e63839a",
            ProjectId = "6389b9592dd24486a037096a",
            Date = DateTime.UtcNow,
            Hours = 8
        };

        // Act
        var response = await client.PostAsJsonAsync($"{_BaseEndpointUri}", command);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}