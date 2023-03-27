using Digitime.Server.Application.Abstractions;
using Digitime.Server.Application.Projects.Commands;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Projects.ValueObjects;
using Digitime.Server.Domain.Users;
using Digitime.Server.Domain.Workspaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Digitime.Server.Application.Projects.Commands.SendInvitationCommand;

namespace Digitime.Server.Application.UnitTests;

public class SendInvitationCommandHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IEmailRepository> _mockEmailRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<IWorkspaceRepository> _mockWorkspaceRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IPublisher> _mockPublisher;
    private readonly Mock<ILogger<SendInvitationCommand>> _mockLogger;

    public SendInvitationCommandHandlerTests()
    {
        _fixture = new Fixture();

        _mockEmailRepository = new Mock<IEmailRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockWorkspaceRepository = new Mock<IWorkspaceRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockPublisher = new Mock<IPublisher>();
        _mockLogger = new Mock<ILogger<SendInvitationCommand>>();

        _mockConfiguration.SetupGet(x => x["JwtSettings:Secret"]).Returns("BqRPie0968kjiS2PK0BOORxEqb37FXn8lCNf64PZfyI=");
        _mockConfiguration.SetupGet(x => x["JwtSettings:Issuer"]).Returns("myissuer");
        _mockConfiguration.SetupGet(x => x["JwtSettings:Expiration"]).Returns("1");
        _mockConfiguration.SetupGet(x => x["BackendUrl"]).Returns("https://mybackendurl.com");
    }

    [Fact]
    public async Task Handle_InvitesUnregisteredUserAndSendsEmail_WhenInviteeIsNotRegistered()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var workspace = _fixture.Create<Workspace>();
        var project = _fixture.Create<Project>();
        project.WorkspaceId = workspace.Id;
        project.AddMember(new ProjectMember(inviter.Id, $"{inviter.Firstname} {inviter.Lastname}", inviter.Email, inviter.ProfilePicture, MemberRoleEnum.ProjectAdmin));

        var command = new SendInvitationCommand(project.Id, inviter.Id, project.Members.Last().Email);

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);
        _mockUserRepository.Setup(x => x.GetByEmail(command.InviteeEmail)).ReturnsAsync((User)null);
        _mockWorkspaceRepository.Setup(x => x.GetbyIdAsync(project.WorkspaceId)).ReturnsAsync(workspace);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailRepository.Verify(x => x.SendEmailAsync(command.InviteeEmail, $"{inviter.Firstname} {inviter.Lastname} invited you to join {project.Title} on Digitime!", $"You have been invited by {inviter.Firstname} {inviter.Lastname} to join {project.Title} on Digitime. Please click the following link to register and join the project: https://mybackendurl.com/invitation?token={It.IsAny<string>()}"), Times.Once);
        project.Members.Should().HaveCount(5);
        project.Members.Last().Email.Should().Be(command.InviteeEmail);
        project.Members.Last().MemberRole.Should().Be(MemberRoleEnum.Pending);
    }

    [Fact]
    public async Task Handle_InvitesRegisteredUserAndSendsEmail_WhenInviteeIsRegisteredButNotInProject()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var workspace = _fixture.Create<Workspace>();
        var project = _fixture.Create<Project>();
        project.WorkspaceId = workspace.Id;
        project.AddMember(new ProjectMember(inviter.Id, $"{inviter.Firstname} {inviter.Lastname}", inviter.Email, inviter.ProfilePicture, MemberRoleEnum.ProjectAdmin));
        var invitee = _fixture.Create<User>();

        var command = new SendInvitationCommand(project.Id, inviter.Id, invitee.Email);

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);
        _mockUserRepository.Setup(x => x.GetByEmail(command.InviteeEmail)).ReturnsAsync(invitee);
        _mockWorkspaceRepository.Setup(x => x.GetbyIdAsync(project.WorkspaceId)).ReturnsAsync(workspace);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailRepository.Verify(x => x.SendEmailAsync(invitee.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        project.Members.Should().HaveCount(5);
        project.Members.Last().UserId.Should().Be(invitee.Id);
        project.Members.Last().Email.Should().Be(invitee.Email);
        project.Members.Last().Fullname.Should().Be($"{invitee.Firstname} {invitee.Lastname}");
        project.Members.Last().MemberRole.Should().Be(MemberRoleEnum.Pending);
    }

    [Fact]
    public async Task Handle_InvitesRegisteredUserAndSendsEmail_WhenInviteeIsRegisteredAndInProject()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var invitee = _fixture.Create<User>();
        var project = _fixture.Create<Project>();
        var workspace = _fixture.Create<Workspace>();
        project.AddMember(new ProjectMember(inviter.Id, $"{inviter.Firstname} {inviter.Lastname}", inviter.Email, inviter.ProfilePicture, MemberRoleEnum.ProjectAdmin));

        var command = new SendInvitationCommand(project.Id, inviter.Id, invitee.Email);

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);
        _mockUserRepository.Setup(x => x.GetByEmail(command.InviteeEmail)).ReturnsAsync(invitee);
        _mockWorkspaceRepository.Setup(x => x.GetbyIdAsync(project.WorkspaceId)).ReturnsAsync(workspace);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailRepository.Verify(x => x.SendEmailAsync(invitee.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        project.Members.Should().HaveCount(5);
        project.Members.Last().UserId.Should().Be(invitee.Id);
        project.Members.Last().Email.Should().Be(invitee.Email);
        project.Members.Last().Fullname.Should().Be($"{invitee.Firstname} {invitee.Lastname}");
        project.Members.Last().MemberRole.Should().Be(MemberRoleEnum.Pending);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenProjectNotFound()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var command = new SendInvitationCommand(Guid.NewGuid().ToString(), inviter.Id, _fixture.Create<string>());

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(command.ProjectId)).ReturnsAsync((Project)null);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenInviterNotFound()
    {
        // Arrange
        var command = new SendInvitationCommand(_fixture.Create<string>(), Guid.NewGuid().ToString(), _fixture.Create<string>());

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(command.InviterUserId)).ReturnsAsync((User)null);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenInviterIsNotMemberOfProject()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var project = _fixture.Create<Project>();
        var command = new SendInvitationCommand(project.Id, inviter.Id, _fixture.Create<string>());

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SendsEmailAndAddsMember_WhenInviteeIsNotRegistered()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var workspace = _fixture.Create<Workspace>();
        var project = _fixture.Create<Project>();
        project.WorkspaceId = workspace.Id;
        project.AddMember(new ProjectMember(inviter.Id, $"{inviter.Firstname} {inviter.Lastname}", inviter.Email, inviter.ProfilePicture, MemberRoleEnum.ProjectAdmin));
        var command = new SendInvitationCommand(project.Id, inviter.Id, _fixture.Create<string>());

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);
        _mockUserRepository.Setup(x => x.GetByEmail(command.InviteeEmail)).ReturnsAsync((User)null);
        _mockWorkspaceRepository.Setup(x => x.GetbyIdAsync(project.WorkspaceId)).ReturnsAsync(workspace);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailRepository.Verify(x => x.SendEmailAsync(command.InviteeEmail, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        project.Members.Should().HaveCount(5);
        project.Members.Last().Email.Should().Be(command.InviteeEmail);
        project.Members.Last().MemberRole.Should().Be(MemberRoleEnum.Pending);
    }

    [Fact]
    public async Task Handle_SendsEmailAndAddsMember_WhenInviteeIsRegisteredButNotInProject()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var project = _fixture.Create<Project>();
        project.AddMember(new ProjectMember(inviter.Id, $"{inviter.Firstname} {inviter.Lastname}", inviter.Email, inviter.ProfilePicture, MemberRoleEnum.ProjectAdmin));
        var invitee = _fixture.Create<User>();
        var workspace = _fixture.Create<Workspace>();

        var command = new SendInvitationCommand(project.Id, inviter.Id, invitee.Email);

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);
        _mockUserRepository.Setup(x => x.GetByEmail(command.InviteeEmail)).ReturnsAsync(invitee);
        _mockWorkspaceRepository.Setup(x => x.GetbyIdAsync(project.WorkspaceId)).ReturnsAsync(workspace);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailRepository.Verify(x => x.SendEmailAsync(command.InviteeEmail, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        project.Members.Should().HaveCount(5);
        project.Members.Last().UserId.Should().Be(invitee.Id);
        project.Members.Last().Fullname.Should().Be($"{invitee.Firstname} {invitee.Lastname}");
        project.Members.Last().Email.Should().Be(invitee.Email);
        project.Members.Last().MemberRole.Should().Be(MemberRoleEnum.Pending);
    }

    [Fact]
    public async Task Handle_SendsEmailAndAddsMember_WhenInviteeIsRegisteredAndInProject()
    {
        // Arrange
        var inviter = _fixture.Create<User>();
        var project = _fixture.Create<Project>();
        project.AddMember(new ProjectMember(inviter.Id, $"{inviter.Firstname} {inviter.Lastname}", inviter.Email, inviter.ProfilePicture, MemberRoleEnum.ProjectAdmin));
        var invitee = _fixture.Create<User>();
        var workspace = _fixture.Create<Workspace>();

        var command = new SendInvitationCommand(project.Id, inviter.Id, invitee.Email);

        _mockUserRepository.Setup(x => x.GetbyExternalIdAsync(inviter.Id)).ReturnsAsync(inviter);
        _mockProjectRepository.Setup(x => x.FindByIdAsync(project.Id)).ReturnsAsync(project);
        _mockUserRepository.Setup(x => x.GetByEmail(command.InviteeEmail)).ReturnsAsync(invitee);
        _mockWorkspaceRepository.Setup(x => x.GetbyIdAsync(project.WorkspaceId)).ReturnsAsync(workspace);

        var handler = new SendInvitationCommandHandler(_mockEmailRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object, _mockConfiguration.Object, _mockWorkspaceRepository.Object, _mockPublisher.Object, _mockLogger.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailRepository.Verify(x => x.SendEmailAsync(command.InviteeEmail, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        project.Members.Should().HaveCount(5);
        project.Members.Last().UserId.Should().Be(invitee.Id);
        project.Members.Last().Fullname.Should().Be($"{invitee.Firstname} {invitee.Lastname}");
        project.Members.Last().Email.Should().Be(invitee.Email);
        project.Members.Last().MemberRole.Should().Be(MemberRoleEnum.Pending);
    }

    public static class TestHelpers
    {
        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("JwtSettings:Issuer", "issuer"),
                new KeyValuePair<string, string>("JwtSettings:Secret", "some-secret"),
                new KeyValuePair<string, string>("JwtSettings:Expiration", "30m"),
                new KeyValuePair<string, string>("BackendUrl", "https://mybackendurl.com")
            })
            .Build();
        }
    }
}