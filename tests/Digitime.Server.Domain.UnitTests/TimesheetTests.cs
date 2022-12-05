using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Digitime.Server.Domain.Timesheets;
using Digitime.Server.Domain.Timesheets.Entities;
using FluentAssertions;

namespace Digitime.Server.Domain.UnitTests;
public class TimesheetTests
{
    [Fact]
    public void AddEntry_ExpectsEntryAdded()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Create<TimesheetEntry>();

        // Act
        timesheet.AddEntry(entry);

        // Assert
        timesheet.TimesheetEntries.Should().Contain(entry);
    }

    [Fact]
    public void AddEntry_AlreadyExists_ExpectsInvalidOperationException()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Create<TimesheetEntry>();
        timesheet.AddEntry(entry);

        // Act
        Action act = () => timesheet.AddEntry(entry);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveEntry_ExpectsEntryRemoved()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Create<TimesheetEntry>();
        timesheet.AddEntry(entry);

        // Act
        timesheet.RemoveEntry(entry);

        // Assert
        timesheet.TimesheetEntries.Should().NotContain(entry);
    }

    [Fact]
    public void RemoveEntry_DoesNotExist_ExpectsInvalidOperationException()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Create<TimesheetEntry>();

        // Act
        Action act = () => timesheet.RemoveEntry(entry);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UpdateEntry_ExpectsEntryUpdated()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Create<TimesheetEntry>();
        timesheet.AddEntry(entry);
        var updatedEntry = fixture.Build<TimesheetEntry>()
            .With(x => x.Id, entry.Id)
            .Create();

        // Act
        timesheet.UpdateEntry(updatedEntry);

        // Assert
        timesheet.TimesheetEntries.Should().Contain(updatedEntry);
    }

    [Fact]
    public void UpdateEntry_DoesNotExist_ExpectsInvalidOperationException()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Create<TimesheetEntry>();

        // Act
        Action act = () => timesheet.UpdateEntry(entry);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetEntriesStatus_ExpectsDraft()
    {
        // Arrange
        var fixture = new Fixture();
        var timesheet = fixture.Create<Timesheet>();
        var entry = fixture.Build<TimesheetEntry>()
            .With(x => x.Status, TimesheetStatus.Draft)
            .Create();
        timesheet.AddEntry(entry);

        // Act
        var result = timesheet.Status;

        // Assert
        result.Should().Be(TimesheetStatus.Draft);
    }
}
