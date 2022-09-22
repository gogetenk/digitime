using Digitime.Server.Dto;
using FluentAssertions;

namespace Digitime.Sever.UnitTests;

public class RelationshipTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Yannis"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Kinga"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Jason"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "François"
            }
        };

        var workspace = new Workspace()
        {
            Id = Guid.NewGuid(),
            Members = null
        };

        var workspaceMembers = new List<WorkspaceMember>()
        {
            new WorkspaceMember()
            {
                Id = Guid.NewGuid(),
                User = users[0],
                Workspace = workspace
            },
            new WorkspaceMember()
            {
                Id = Guid.NewGuid(),
                User = users[1],
                Workspace = workspace
            },
            new WorkspaceMember()
            {
                Id = Guid.NewGuid(),
                User = users[2],
                Workspace = workspace
            }

        };

        workspace.Members = workspaceMembers;


        var projectReviewers = workspaceMembers.Where(x => x.User.Name == "Yannis").ToList();
        var projectEditors = workspaceMembers.Where(x => x.User.Name == "Kinga" || x.User.Name == "François").ToList();

        var project = new Project()
        {
            Id = Guid.NewGuid(),
            Name = "Digitime",
            Owner = users[2], // Jason
            Editors = projectEditors,
            Reviewers = projectReviewers,
            Timesheets = new List<Timesheet> {
               
            }
        };

        var timesheet = new Timesheet()
        {
            Id = Guid.NewGuid(),
            Owner = workspaceMembers[0],
            Project = project,
            SignedBy = workspaceMembers[1]
        };

        project.Timesheets.Add(timesheet);


        // Act



        // Assert
    }
}