//using System.Net;
//using Digitime.Server.IntegrationTests.Infrastructure;
//using Digitime.Shared.Contracts.Projects;
//using Newtonsoft.Json;

//namespace Digitime.Server.IntegrationTests;
//public class ProjectsTests : IntegrationTestBase
//{
//    private readonly string _BaseEndpointUri = "/api/projects";

//    public ProjectsTests() : base()
//    {
//    }

//    [Fact]
//    public async Task GetUserProjects_NominalCase_Expect200()
//    {
//        // Arrange
//        var client = Factory.CreateClient();

//        // Act
//        var response = await client.GetAsync(_BaseEndpointUri);
//        var result = JsonConvert.DeserializeObject<GetUserProjectsResponse>(await response.Content.ReadAsStringAsync());

//        // Assert
//        response.StatusCode.Should().Be(HttpStatusCode.OK);
//        result.Projects.Should().NotBeNullOrEmpty();
//    }

//    //[Fact]
//    //public async Task GetUserProjects_WhenNoProjectsFound_Expect404()
//    //{
//    //    // Arrange
//    //    var client = Factory.CreateClient();

//    //    // Act
//    //    var response = await client.GetAsync(_BaseEndpointUri);
//    //    var content = await response.Content.ReadAsStringAsync();
//    //    var result = JsonConvert.DeserializeObject<string>(content);

//    //    // Assert
//    //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//    //    result.Should().Be("No project has been found for the current user.");
//    //}
//}
