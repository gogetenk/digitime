using Digitime.Server.Application.Calendar.Commands;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Server.Mappings;
using EasyCaching.Core;
using Mapster;
using MapsterMapper;
using static Digitime.Server.Application.Calendar.Commands.CreateTimesheetEntryCommand;

namespace Digitime.Server.Application.UnitTests;

public class CreateTimesheetEntryCommandTests
{
    public static Mapper GetMapper()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingProfile).Assembly);
        return new Mapper(config);
    }

    [Fact]
    public async Task Handle_ExpectsTimesheetEntryCreatedAndCacheRemoved()
    {
        var mapper = GetMapper();

        // Arrange
        var fixture = new Fixture();
        var timesheetId = fixture.Create<string>();
        var command = fixture.Create<CreateTimesheetEntryCommand>();
        var timesheet = fixture.Create<Timesheet>();
        var user = fixture.Create<UserEntity>();
        var project = fixture.Create<ProjectEntity>();
        var project2 = mapper.Map<Domain.Projects.Project>(project);
        var timesheetEntity = timesheet.Adapt<TimesheetEntity>();

        var userRepository = new Mock<IRepository<UserEntity>>();
        userRepository
            .Setup(x => x.FindByIdAsync(command.UserId))
            .ReturnsAsync(user);
        var projectRepository = new Mock<IRepository<ProjectEntity>>();
        projectRepository
            .Setup(x => x.FindByIdAsync(command.ProjectId))
            .ReturnsAsync(project);
        var timesheetRepository = new Mock<IRepository<TimesheetEntity>>();
        timesheetRepository
            .Setup(x => x.FindByIdAsync(timesheetId))
            .ReturnsAsync(timesheetEntity);
        var cachingProvider = new Mock<IEasyCachingProvider>();

        var sut = new CreateTimesheetEntryCommandHandler(timesheetRepository.Object, projectRepository.Object, userRepository.Object, cachingProvider.Object);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        timesheetRepository.Verify(x => x.ReplaceOneAsync(It.IsAny<TimesheetEntity>()), Times.Once);
        cachingProvider.Verify(x => x.RemoveAsync($"Calendar_{command.Date.Month}_{command.Date.Year}_{command.UserId}", CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectDoesNotExist_ExpectsInvalidOperationException()
    {
        // Arrange
        var fixture = new Fixture();
        var command = fixture.Create<CreateTimesheetEntryCommand>();
        var userRepository = new Mock<IRepository<UserEntity>>();
        userRepository.Setup(x => x.FindByIdAsync(command.UserId)).ReturnsAsync(fixture.Create<UserEntity>());
        var projectRepository = new Mock<IRepository<ProjectEntity>>();
        var timesheetRepository = new Mock<IRepository<TimesheetEntity>>();
        var cachingProvider = new Mock<IEasyCachingProvider>();

        var sut = new CreateTimesheetEntryCommandHandler(timesheetRepository.Object, projectRepository.Object, userRepository.Object, cachingProvider.Object);

        // Act
        Func<Task> act = async () => await sut.Handle(command, CancellationToken.None);

        // Assert
        var exc = await act.Should().ThrowAsync<InvalidOperationException>();
        exc.WithMessage($"Project with id {command.ProjectId} not found, aborting timesheet entry creation.");
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ExpectsInvalidOperationException()
    {
        // Arrange
        var fixture = new Fixture();
        var command = fixture.Create<CreateTimesheetEntryCommand>();
        var userRepository = new Mock<IRepository<UserEntity>>();
        var projectRepository = new Mock<IRepository<ProjectEntity>>();
        projectRepository.Setup(x => x.FindByIdAsync(command.ProjectId)).ReturnsAsync(fixture.Create<ProjectEntity>());
        var timesheetRepository = new Mock<IRepository<TimesheetEntity>>();
        var cachingProvider = new Mock<IEasyCachingProvider>();

        var sut = new CreateTimesheetEntryCommandHandler(timesheetRepository.Object, projectRepository.Object, userRepository.Object, cachingProvider.Object);

        // Act
        Func<Task> act = async () => await sut.Handle(command, CancellationToken.None);

        // Assert
        var exc = await act.Should().ThrowAsync<InvalidOperationException>();
        exc.WithMessage($"User with id {command.UserId} not found, aborting timesheet entry creation.");
    }
}