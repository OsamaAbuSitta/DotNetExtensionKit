using DotNetExtensionKit;
using Xunit;
using System;

namespace DotNetExtensionKit.Tests;

public class StringExtensions_LeftRightTests
{
    // Left tests

    [Fact]
    public void Left_ReturnsFirstNChars_WhenLengthLessThanStringLength()
    {
        Assert.Equal("Hel", "Hello".Left(3));
    }

    [Fact]
    public void Left_ReturnsFullString_WhenLengthEqualsStringLength()
    {
        Assert.Equal("Hello", "Hello".Left(5));
    }

    [Fact]
    public void Left_ReturnsFullString_WhenLengthExceedsStringLength()
    {
        Assert.Equal("Hello", "Hello".Left(100));
    }

    [Fact]
    public void Left_ReturnsEmptyString_WhenLengthIsZero()
    {
        Assert.Equal("", "Hello".Left(0));
    }

    [Fact]
    public void Left_ReturnsEmptyString_WhenInputIsEmpty()
    {
        Assert.Equal("", "".Left(5));
    }

    [Fact]
    public void Left_ThrowsArgumentOutOfRangeException_WhenLengthIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "Hello".Left(-1));
    }

    // Right tests

    [Fact]
    public void Right_ReturnsLastNChars_WhenLengthLessThanStringLength()
    {
        Assert.Equal("llo", "Hello".Right(3));
    }

    [Fact]
    public void Right_ReturnsFullString_WhenLengthEqualsStringLength()
    {
        Assert.Equal("Hello", "Hello".Right(5));
    }

    [Fact]
    public void Right_ReturnsFullString_WhenLengthExceedsStringLength()
    {
        Assert.Equal("Hello", "Hello".Right(100));
    }

    [Fact]
    public void Right_ReturnsEmptyString_WhenLengthIsZero()
    {
        Assert.Equal("", "Hello".Right(0));
    }

    [Fact]
    public void Right_ReturnsEmptyString_WhenInputIsEmpty()
    {
        Assert.Equal("", "".Right(5));
    }

    [Fact]
    public void Right_ThrowsArgumentOutOfRangeException_WhenLengthIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "Hello".Right(-1));
    }
}
