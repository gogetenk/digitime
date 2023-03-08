using Digitime.Server.Application.Abstractions;
using Digitime.Server.Application.Indicators.Queries;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Users;
using static Digitime.Server.Application.Indicators.Queries.GetIndicatorsQuery;

namespace Digitime.Server.Application.UnitTests;

public class GetIndicatorsQueryTests
{
    [Fact]
    public async Task GetIndicatorsQueryHandler_NominalCase_ShouldReturnIndicators()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var query = new GetIndicatorsQuery(userId);
        var timesheets = new Fixture().Create<List<Timesheet>>();
        //timesheets.ForEach(x => x.TimesheetEntries.);
        var projects = new Fixture().Create<List<Project>>();

        var timesheetRepo = new Mock<ITimesheetRepository>();
        timesheetRepo
            .Setup(x => x.GetbyUserAndMonthOfyear(userId, DateTime.UtcNow.Month, DateTime.UtcNow.Year))
            .ReturnsAsync(timesheets);

        var projectRepo = new Mock<IProjectRepository>();
        projectRepo.Setup(x => x.GetProjectsByUserId(userId))
            .ReturnsAsync(projects);

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(x => x.GetbyExternalIdAsync(userId))
        .ReturnsAsync(new User(userId));

        var handler = new GetIndicatorsQueryHandler(userRepo.Object, projectRepo.Object, timesheetRepo.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.FirstOrDefault(x => x.Title == "Total Projects").Value.Should().Be(projects.Count().ToString());
        result.FirstOrDefault(x => x.Title == "Pending Hours this month").Value.Should().NotBe("0");
        //result.FirstOrDefault(x => x.Title == "Validated Hours this month").Value.Should().NotBe("0");
    }

    [Fact]
    public async Task GetIndicatorsQueryHandler_WhenUserDoesntHaveAnyDataYet_ShouldReturn0Indicators()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var query = new GetIndicatorsQuery(userId);
        var timesheetRepo = new Mock<ITimesheetRepository>();
        timesheetRepo
            .Setup(x => x.GetbyUserAndMonthOfyear(userId, DateTime.UtcNow.Month, DateTime.UtcNow.Year))
            .ReturnsAsync(value: null);

        var projectRepo = new Mock<IProjectRepository>();
        projectRepo.Setup(x => x.GetProjectsByUserId(userId))
            .ReturnsAsync(value: null);

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(x => x.GetbyExternalIdAsync(userId))
        .ReturnsAsync(new User(userId));

        var handler = new GetIndicatorsQueryHandler(userRepo.Object, projectRepo.Object, timesheetRepo.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.FirstOrDefault(x => x.Title == "Total Projects").Value.Should().Be("0");
        result.FirstOrDefault(x => x.Title == "Pending Hours this month").Value.Should().Be("0");
        result.FirstOrDefault(x => x.Title == "Validated Hours this month").Value.Should().Be("0");
    }
}
