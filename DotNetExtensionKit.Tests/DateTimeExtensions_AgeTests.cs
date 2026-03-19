using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class DateTimeExtensions_AgeTests
{
    [Fact]
    public void Age_WithAsOf_ReturnsWholeYears()
    {
        var birthDate = new DateTime(1990, 6, 15);
        var asOf = new DateTime(2024, 8, 20);

        Assert.Equal(34, birthDate.Age(asOf));
    }

    [Fact]
    public void Age_BirthdayNotYetOccurred_SubtractsOne()
    {
        var birthDate = new DateTime(1990, 12, 25);
        var asOf = new DateTime(2024, 6, 1);

        Assert.Equal(33, birthDate.Age(asOf));
    }

    [Fact]
    public void Age_OnExactBirthday_ReturnsFullYears()
    {
        var birthDate = new DateTime(2000, 3, 10);
        var asOf = new DateTime(2024, 3, 10);

        Assert.Equal(24, birthDate.Age(asOf));
    }

    [Fact]
    public void Age_DayBeforeBirthday_SubtractsOne()
    {
        var birthDate = new DateTime(2000, 3, 10);
        var asOf = new DateTime(2024, 3, 9);

        Assert.Equal(23, birthDate.Age(asOf));
    }

    [Fact]
    public void Age_SameDate_ReturnsZero()
    {
        var date = new DateTime(2024, 1, 1);

        Assert.Equal(0, date.Age(date));
    }

    [Fact]
    public void Age_BirthDateAfterAsOf_ThrowsArgumentOutOfRangeException()
    {
        var birthDate = new DateTime(2025, 1, 1);
        var asOf = new DateTime(2024, 1, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => birthDate.Age(asOf));
    }

    [Fact]
    public void Age_ParameterlessOverload_UsesToday()
    {
        // A birth date far enough in the past that the age is deterministic
        var birthDate = new DateTime(2000, 1, 1);
        var expected = birthDate.Age(DateTime.Today);

        Assert.Equal(expected, birthDate.Age());
    }

    [Fact]
    public void Age_LeapYearBirthday_Feb29()
    {
        var birthDate = new DateTime(2000, 2, 29);
        // March 1 of a non-leap year — birthday has passed
        var asOf = new DateTime(2023, 3, 1);

        Assert.Equal(23, birthDate.Age(asOf));
    }

    [Fact]
    public void Age_LeapYearBirthday_BeforeFeb28()
    {
        var birthDate = new DateTime(2000, 2, 29);
        var asOf = new DateTime(2023, 2, 28);

        Assert.Equal(22, birthDate.Age(asOf));
    }
}
