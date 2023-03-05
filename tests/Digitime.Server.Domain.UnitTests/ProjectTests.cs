using AutoFixture;
using Digitime.Server.Domain.Projects;
using Digitime.Server.Domain.Projects.ValueObjects;
using FluentAssertions;

namespace Digitime.Server.Domain.UnitTests;

public class ProjectTests
{
    [Fact]
    public void AddMember_ExpectsMemberAdded()
    {
        // Arrange
        var fixture = new Fixture();
        var project = fixture.Create<Project>();
        var member = fixture.Create<ProjectMember>();

        // Act
        project.AddMember(member);

        // Assert
        project.Members.Should().Contain(member);
    }
}