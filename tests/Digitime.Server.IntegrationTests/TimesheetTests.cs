using System.Net;
using System.Net.Http.Json;
using Digitime.Server.IntegrationTests.Infrastructure;
using Digitime.Shared.Contracts.Timesheets;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Digitime.Server.IntegrationTests;

public class TimesheetTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string _BaseEndpointUri = "/api/timesheets/entry";
    private static MongoClient _mongoClient;

    public TimesheetTests()
    {
        // We instantiate the mongo client only once for all the tests
        if (_mongoClient is null)
            _mongoClient = new MongoClient(Factory.Configuration["DatabaseConfiguration:ConnectionString"]);
        
        _factory = new WebApplicationFactory<Program>();
    }

    [Fact]
    public async Task CreateTimesheetEntry_WhenUserIsAWorker_Expect201()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Staging");
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
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    public Task InitializeAsync()
    {
    }

    public Task DisposeAsync()
    {
    }
}