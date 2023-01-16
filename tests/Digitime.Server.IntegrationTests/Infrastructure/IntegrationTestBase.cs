using System.Net;
using AutoFixture;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using RichardSzalay.MockHttp;

namespace Digitime.Server.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    protected readonly WebApplicationFactory<Program> Factory;
    private static MongoDbRunner _runner;
    private static MongoClient _mongoClient;

    public IntegrationTestBase(string userRole)
    {
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Staging");
            //builder.ConfigureServices(services => services.AddHttpClient("PublicHolidaysClient", client => CreateMockHttpClient()));
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, WorkerTestAuthHandler>(
                        "TestScheme", options => { });

                services.PostConfigure<MongoDbSettings>((config) => config.ConnectionString = _runner.ConnectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        _runner = MongoDbRunner.Start();

        // We instantiate the mongo client only once for all the tests
        if (_mongoClient is null)
            _mongoClient = new MongoClient(_runner.ConnectionString);

        // Insert a user in the db
        var db = _mongoClient.GetDatabase("DigitimeApp");
        UserEntity user = new Fixture().Create<UserEntity>();
        TimesheetEntity timesheet = new Fixture().Create<TimesheetEntity>();
        ProjectEntity project = new Fixture().Create<ProjectEntity>();
        user.Id = ObjectId.GenerateNewId().ToString();
        user.ExternalId = "github|31359382";
        project.Id = "6389b9592dd24486a037096a";
        project.Members.Add(new ProjectMemberEntity(user.Id, "Toto", "Toto", "ProfilePicture", MemberRoleEntityEnum.Reviewer));
        timesheet.Id = "6392737298425fc69e63839a";
        timesheet.TimesheetEntries.ForEach(x => x.Id = ObjectId.GenerateNewId().ToString());

        try
        {
            await db.GetCollection<UserEntity>("users").InsertOneAsync(user);
            await db.GetCollection<TimesheetEntity>("timesheets").InsertOneAsync(timesheet);
            await db.GetCollection<ProjectEntity>("projects").InsertOneAsync(project);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        //// Deleting test collection to keep data consistency
        //var db = _mongoClient.GetDatabase("DigitimeDb");
        //await db.DropCollectionAsync(_timesheetCollectionName);
        //await db.DropCollectionAsync(_projectsCollectionName);
        //await db.DropCollectionAsync(_usersCollectionName);
    }

    public HttpClient CreateMockHttpClient()
    {
        var config = Factory.Services.GetService<IConfiguration>();
        var mockHttp = new MockHttpMessageHandler();
        // Mocking calls to public holidays api
        mockHttp.When($"{config["ExternalApis:PublicHolidaysApi:Url"]}/api/v3/PublicHolidays/2022/FR")
                .Respond(HttpStatusCode.OK);

        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri("http://toto");
        client.MaxResponseContentBufferSize = 32;
        return client;
    }
}
