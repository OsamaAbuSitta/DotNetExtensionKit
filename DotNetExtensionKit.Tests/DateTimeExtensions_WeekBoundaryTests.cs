using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class DateTimeExtensions_WeekBoundaryTests
{
    [Fact]
    public void StartOfWeek_DefaultsToMonday()
    {
        // Wednesday 2024-07-17
        var input = new DateTime(2024, 7, 17, 14, 30, 0, DateTimeKind.Local);
        var result = input.StartOfWeek();

        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
        Assert.Equal(new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Local), result);
    }

    [Fact]
    public void StartOfWeek_OnStartDay_ReturnsSameDate()
    {
        // Monday 2024-07-15
        var input = new DateTime(2024, 7, 15, 10, 0, 0, DateTimeKind.Utc);
        var result = input.StartOfWeek(DayOfWeek.Monday);

        Assert.Equal(new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), result);
    }

    [Fact]
    public void StartOfWeek_WithSunday_FindsMostRecentSunday()
    {
        // Wednesday 2024-07-17
        var input = new DateTime(2024, 7, 17, 8, 0, 0, DateTimeKind.Local);
        var result = input.StartOfWeek(DayOfWeek.Sunday);

        Assert.Equal(DayOfWeek.Sunday, result.DayOfWeek);
        Assert.Equal(new DateTime(2024, 7, 14, 0, 0, 0, 0, DateTimeKind.Local), result);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void StartOfWeek_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 7, 17, 14, 30, 0, kind);
        var result = input.StartOfWeek();

        Assert.Equal(kind, result.Kind);
    }

    [Fact]
    public void StartOfWeek_ReturnsAtMidnight()
    {
        var input = new DateTime(2024, 7, 17, 23, 59, 59, DateTimeKind.Local);
        var result = input.StartOfWeek();

        Assert.Equal(TimeSpan.Zero, result.TimeOfDay);
    }

    [Fact]
    public void EndOfWeek_ReturnsSixDaysAfterStartOfWeek()
    {
        // Wednesday 2024-07-17, week starts Monday 2024-07-15
        var input = new DateTime(2024, 7, 17, 14, 30, 0, DateTimeKind.Local);
        var result = input.EndOfWeek();

        // End of week should be Sunday 2024-07-21 at end of day
        Assert.Equal(DayOfWeek.Sunday, result.DayOfWeek);
        Assert.Equal(2024, result.Year);
        Assert.Equal(7, result.Month);
        Assert.Equal(21, result.Day);
    }

    [Fact]
    public void EndOfWeek_HasEndOfDayTime()
    {
        var input = new DateTime(2024, 7, 17, 14, 30, 0, DateTimeKind.Local);
        var result = input.EndOfWeek();

        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
        Assert.Equal(TimeSpan.TicksPerDay - 1, result.TimeOfDay.Ticks);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void EndOfWeek_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 7, 17, 14, 30, 0, kind);
        var result = input.EndOfWeek();

        Assert.Equal(kind, result.Kind);
    }

    [Fact]
    public void EndOfWeek_IsExactlySixDaysAfterStartOfWeek()
    {
        var input = new DateTime(2024, 7, 17, 14, 30, 0, DateTimeKind.Utc);
        var start = input.StartOfWeek();
        var end = input.EndOfWeek();

        Assert.Equal(6, (end.Date - start.Date).Days);
    }

    [Fact]
    public void WeekBoundary_WithSaturdayStart()
    {
        // Wednesday 2024-07-17
        var input = new DateTime(2024, 7, 17, 12, 0, 0, DateTimeKind.Local);
        var start = input.StartOfWeek(DayOfWeek.Saturday);
        var end = input.EndOfWeek(DayOfWeek.Saturday);

        Assert.Equal(DayOfWeek.Saturday, start.DayOfWeek);
        Assert.Equal(new DateTime(2024, 7, 13, 0, 0, 0, 0, DateTimeKind.Local), start);
        Assert.Equal(6, (end.Date - start.Date).Days);
    }
}
