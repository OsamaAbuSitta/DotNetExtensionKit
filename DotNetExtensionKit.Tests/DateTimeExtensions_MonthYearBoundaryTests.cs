using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class DateTimeExtensions_MonthYearBoundaryTests
{
    [Fact]
    public void StartOfMonth_ReturnsFirstDayAtMidnight()
    {
        var input = new DateTime(2024, 7, 15, 14, 30, 45, DateTimeKind.Local);
        var result = input.StartOfMonth();

        Assert.Equal(new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Local), result);
    }

    [Fact]
    public void StartOfMonth_AlreadyFirstDay_ReturnsMidnight()
    {
        var input = new DateTime(2024, 3, 1, 10, 20, 30, DateTimeKind.Utc);
        var result = input.StartOfMonth();

        Assert.Equal(new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), result);
    }

    [Fact]
    public void EndOfMonth_ReturnsLastDayAtEndOfDay()
    {
        var input = new DateTime(2024, 7, 15, 14, 30, 45, DateTimeKind.Local);
        var result = input.EndOfMonth();

        Assert.Equal(31, result.Day);
        Assert.Equal(7, result.Month);
        Assert.Equal(2024, result.Year);
        Assert.Equal(TimeSpan.TicksPerDay - 1, result.TimeOfDay.Ticks);
    }

    [Fact]
    public void EndOfMonth_February_LeapYear_Returns29()
    {
        var input = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc);
        var result = input.EndOfMonth();

        Assert.Equal(29, result.Day);
    }

    [Fact]
    public void EndOfMonth_February_NonLeapYear_Returns28()
    {
        var input = new DateTime(2023, 2, 10, 0, 0, 0, DateTimeKind.Utc);
        var result = input.EndOfMonth();

        Assert.Equal(28, result.Day);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void StartOfMonth_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 10, 30, 0, kind);
        var result = input.StartOfMonth();

        Assert.Equal(kind, result.Kind);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void EndOfMonth_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 10, 30, 0, kind);
        var result = input.EndOfMonth();

        Assert.Equal(kind, result.Kind);
    }

    [Fact]
    public void StartOfYear_ReturnsJanuary1AtMidnight()
    {
        var input = new DateTime(2024, 7, 15, 14, 30, 45, DateTimeKind.Local);
        var result = input.StartOfYear();

        Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), result);
    }

    [Fact]
    public void EndOfYear_ReturnsDecember31AtEndOfDay()
    {
        var input = new DateTime(2024, 7, 15, 14, 30, 45, DateTimeKind.Utc);
        var result = input.EndOfYear();

        Assert.Equal(12, result.Month);
        Assert.Equal(31, result.Day);
        Assert.Equal(2024, result.Year);
        Assert.Equal(TimeSpan.TicksPerDay - 1, result.TimeOfDay.Ticks);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void StartOfYear_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 10, 30, 0, kind);
        var result = input.StartOfYear();

        Assert.Equal(kind, result.Kind);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void EndOfYear_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 10, 30, 0, kind);
        var result = input.EndOfYear();

        Assert.Equal(kind, result.Kind);
    }
}
