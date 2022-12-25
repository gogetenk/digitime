using System.Net;
using System.Net.Http.Json;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Server.IntegrationTests.Infrastructure;
using Digitime.Shared.Contracts.Timesheets;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Digitime.Server.IntegrationTests;

public class TimesheetTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime  
{
    private readonly WebApplicationFactory<Program> _factory;
    private const string _BaseEndpointUri = "/api/timesheets/entry";
    private string _timesheetCollectionName;
    private string _projectsCollectionName;
    private IConfiguration _configuration;

    private static MongoClient _mongoClient;

    public TimesheetTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Staging");
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, WorkerTestAuthHandler>(
                        "TestScheme", options => { });

                services.PostConfigure<IMongoDbSettings>((config) =>
                {
                    config.TimesheetsCollectionName = _timesheetCollectionName;
                    config.ProjectsCollectionName = _projectsCollectionName;
                });
                _configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            });
        });

     

    }

    [Fact]
    public async Task CreateTimesheetEntry_WhenUserIsAWorker_Expect201()
    {
        // Arrange
        var client = _factory.CreateClient();

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
        // We instantiate the mongo client only once for all the tests
        if (_mongoClient is null)
            _mongoClient = new MongoClient(_configuration["DatabaseConfiguration:ConnectionString"]);

        _timesheetCollectionName = Guid.NewGuid().ToString();
        _projectsCollectionName = Guid.NewGuid().ToString();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        // Deleting test collection to keep data consistency
        var db = _mongoClient.GetDatabase(_configuration["DatabaseConfiguration:DatabaseName"]);
        await db.DropCollectionAsync(_timesheetCollectionName);
        await db.DropCollectionAsync(_projectsCollectionName);
    }
}