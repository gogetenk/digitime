using Digitime.Server.Application.Abstractions;
using Digitime.Server.Application.Projects.Queries;
using Digitime.Server.Domain.Projects;
using Digitime.Shared.Contracts.Projects;
using Mapster;
using static Digitime.Server.Application.Projects.Queries.GetProjectsQuery;
using User = Digitime.Server.Domain.Users.User;

namespace Digitime.Server.Application.UnitTests;

public class GetProjectsQueryTests
{
    [Fact]
    public async Task Handle_NominalCase_ReturnsProjects()
    {
        // Arrange
        var query = new Fixture().Create<GetProjectsQuery>();
        var user = new Fixture().Create<User>();
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock
            .Setup(x => x.GetbyExternalIdAsync(query.UserId))
            .ReturnsAsync(user);
        var projectsToReturn = new Fixture().Create<List<Project>>();
        var projectRepoMock = new Mock<IProjectRepository>();
        projectRepoMock
            .Setup(x => x.GetProjectsByUserId(user.Id))
            .ReturnsAsync(projectsToReturn);
        var sut = new GetProjectsQueryHandler(userRepoMock.Object, projectRepoMock.Object);

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Projects.Should().BeEquivalentTo(projectsToReturn.Adapt<List<WorkspaceDto>>());
    }

    [Fact]
    public async Task Handle_WhenNoProjectsReturned_ReturnsNothing()
    {
        // Arrange
        var query = new Fixture().Create<GetProjectsQuery>();
        var user = new Fixture().Create<User>();
        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock
            .Setup(x => x.GetbyExternalIdAsync(query.UserId))
            .ReturnsAsync(user);
        var projectRepoMock = new Mock<IProjectRepository>();
        projectRepoMock
            .Setup(x => x.GetProjectsByUserId(user.Id))
            .ReturnsAsync(value: null);

        var sut = new GetProjectsQueryHandler(userRepoMock.Object, projectRepoMock.Object);

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Projects.Should().BeNull();
    }
}
