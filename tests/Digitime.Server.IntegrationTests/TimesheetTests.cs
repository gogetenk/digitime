using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Digitime.Shared.Contracts.Timesheets;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Digitime.Server.IntegrationTests;

public class TimesheetTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string _BaseEndpointUri = "/api/timesheets";

    public TimesheetTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Fact]
    public async Task CreateTimesheetEntry_NominalCase_Expect201()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new Fixture().Create<CreateTimesheetEntryRequest>() with { UserId = Guid.NewGuid().ToString(), TimesheetId = null };
            
        // Act
        var response = await client.PostAsJsonAsync($"{_BaseEndpointUri}", command);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}