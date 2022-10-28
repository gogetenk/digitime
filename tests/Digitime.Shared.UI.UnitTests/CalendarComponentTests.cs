using System.Diagnostics.Metrics;
using AutoFixture;
using Bunit;
using Digitime.Shared.UI.Components.Common;
using Digitime.Shared.UI.UnitTests.Helpers;
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;

namespace Digitime.Shared.UI.UnitTests;

public class CalendarComponentTests
{
    [Fact]
    public async Task GetPublicHolidaysForSpecifiedMonthAndCountry_ExpectsPublicHolidays()
    {
        // Arrange
        var dateTime = new DateTime(2021, 12, 1);
        var country = "DE";
        using var ctx = new TestContext();
        var calendarComponent = ctx.RenderComponent<CalendarComponent>();
        var httpClientMock = ctx.Services.AddMockHttpClient();
        httpClientMock.When("/*").RespondJson(new Fixture().Create<List<PublicHoliday>>());

        // Act
        var publicHolidays = await calendarComponent.Instance.GetPublicHolidaysForSpecifiedMonthAndCountry(dateTime, country);

        // Assert
        Assert.NotEmpty(publicHolidays);
    }
}