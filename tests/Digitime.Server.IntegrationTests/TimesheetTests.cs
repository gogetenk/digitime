using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Digitime.Server.Application.Calendar.Comands;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Digitime.Server.IntegrationTests;

public class TimesheetTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string _BaseEndpointUri = "/timesheet";

    public TimesheetTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Fact]
    public async Task CreateTimesheetEntry_NominalCase_Expect201()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new Fixture().Create<CreateTimesheetEntryCommand>();

        // Act
        var response = await client.PostAsJsonAsync($"{_BaseEndpointUri}", command);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}