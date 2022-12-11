using AutoFixture;
using Bunit;
using Digitime.Shared.Dto;
using Digitime.Shared.UI.Components.Common;
using Digitime.Shared.UITests.Helpers;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Digitime.Shared.UITests;

public class CalendarComponentTests
{
    [Fact]
    public async Task GetPublicHolidaysForSpecifiedMonthAndCountry_ExpectsPublicHolidays()
    {
        // Arrange
        var dateTime = new DateTime(2021, 12, 1);
        var country = "FR";
        using var ctx = new TestContext();
        var httpClientMock = ctx.Services.AddMockHttpClient();
        httpClientMock.When("/*").RespondJson(new Fixture().Create<CalendarDto>());

        // Act
        var calendarComponent = ctx.RenderComponent<CalendarComponent>();

        // Assert
        calendarComponent.Instance.CurrentDayTimesheetEntries.Should().NotBeNull();
        calendarComponent.Instance.NextMonthCalendarDays.Should().NotBeNull();
        calendarComponent.Instance.CurrentDayTimesheetEntries.Should().BeNull();
    }
}