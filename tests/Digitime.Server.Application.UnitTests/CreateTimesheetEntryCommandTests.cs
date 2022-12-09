using System.Reflection;
using AutoFixture;
using Digitime.Server.Application.Calendar.Comands;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Server.Mappings;
using Mapster;
using MapsterMapper;
using Moq;
using static Digitime.Server.Application.Calendar.Comands.CreateTimesheetEntryCommand;

namespace Digitime.Server.Application.UnitTests;

public class CreateTimesheetEntryCommandTests
{
    public static Mapper GetMapper()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        return new Mapper(config);
    }
    
    [Fact]
    public async Task Handle_ExpectsTimesheetEntryCreated()
    {
        GetMapper();
        // Arrange
        var fixture = new Fixture();
        var timesheetId = fixture.Create<string>();
        var command = fixture.Create<CreateTimesheetEntryCommand>();
        var timesheet = fixture.Create<Timesheet>();
        var user = fixture.Create<UserEntity>();
        var project = fixture.Create<ProjectEntity>();
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

        var sut = new CreateTimesheetEntryCommandHandler(timesheetRepository.Object, projectRepository.Object, userRepository.Object);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        timesheetRepository.Verify(x => x.ReplaceOneAsync(timesheetEntity), Times.Once);
    }
}