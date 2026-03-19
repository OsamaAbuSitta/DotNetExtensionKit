using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class DateTimeExtensions_DayBoundaryTests
{
    [Fact]
    public void StartOfDay_ReturnsDateAtMidnight()
    {
        var input = new DateTime(2024, 7, 15, 14, 30, 45, 123, DateTimeKind.Local);
        var result = input.StartOfDay();

        Assert.Equal(new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Local), result);
    }

    [Fact]
    public void StartOfDay_AlreadyAtMidnight_ReturnsSameValue()
    {
        var input = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var result = input.StartOfDay();

        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void StartOfDay_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 10, 30, 0, kind);
        var result = input.StartOfDay();

        Assert.Equal(kind, result.Kind);
    }

    [Fact]
    public void StartOfDay_IsIdempotent()
    {
        var input = new DateTime(2024, 3, 20, 18, 45, 30, DateTimeKind.Local);
        var first = input.StartOfDay();
        var second = first.StartOfDay();

        Assert.Equal(first, second);
    }

    [Fact]
    public void EndOfDay_ReturnsDateAtEndOfDay()
    {
        var input = new DateTime(2024, 7, 15, 14, 30, 45, 123, DateTimeKind.Local);
        var result = input.EndOfDay();

        Assert.Equal(2024, result.Year);
        Assert.Equal(7, result.Month);
        Assert.Equal(15, result.Day);
        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
        Assert.Equal(TimeSpan.TicksPerDay - 1, result.TimeOfDay.Ticks);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void EndOfDay_PreservesDateTimeKind(DateTimeKind kind)
    {
        var input = new DateTime(2024, 6, 15, 10, 30, 0, kind);
        var result = input.EndOfDay();

        Assert.Equal(kind, result.Kind);
    }

    [Fact]
    public void EndOfDay_IsIdempotent()
    {
        var input = new DateTime(2024, 3, 20, 18, 45, 30, DateTimeKind.Utc);
        var first = input.EndOfDay();
        var second = first.EndOfDay();

        Assert.Equal(first, second);
    }

    [Fact]
    public void DayBoundary_Ordering_StartOfDay_LessOrEqual_Input_LessOrEqual_EndOfDay()
    {
        var input = new DateTime(2024, 8, 10, 12, 0, 0, DateTimeKind.Local);

        Assert.True(input.StartOfDay() <= input);
        Assert.True(input <= input.EndOfDay());
    }
}
