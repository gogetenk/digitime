using System.Net;
using System.Net.Http.Json;
using Digitime.Server.IntegrationTests.Infrastructure;
using Digitime.Shared.Contracts.Timesheets;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

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
    public async Task CreateTimesheetEntry_WhenUserIsAWorker_Expect201()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, WorkerTestAuthHandler>(
                        "TestScheme", options => { });
            });
        }).CreateClient();
        
        var command = new CreateTimesheetEntryRequest
        {
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

    [Fact]
    public async Task CreateTimesheetEntry_WhenUserIsReviewer_Expect401()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, ReviewerTestAuthHandler>(
                        "TestScheme", options => { });
            });
        }).CreateClient();

        var command = new CreateTimesheetEntryRequest
        {
            TimesheetId = "6392737298425fc69e63839a",
            ProjectId = "6389b9592dd24486a037096a",
            Date = DateTime.UtcNow,
            Hours = 8
        };

        // Act
        var response = await client.PostAsJsonAsync($"{_BaseEndpointUri}", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}