using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class StringExtensions_TruncateTests
{
    // --- Truncate ---

    [Fact]
    public void Truncate_ReturnsUnchanged_WhenLengthWithinLimit()
    {
        Assert.Equal("hello", "hello".Truncate(10));
    }

    [Fact]
    public void Truncate_ReturnsUnchanged_WhenLengthEqualsLimit()
    {
        Assert.Equal("hello", "hello".Truncate(5));
    }

    [Fact]
    public void Truncate_TruncatesToMaxLength()
    {
        Assert.Equal("hel", "hello".Truncate(3));
    }

    [Fact]
    public void Truncate_ReturnsEmpty_WhenMaxLengthIsZero()
    {
        Assert.Equal("", "hello".Truncate(0));
    }

    [Fact]
    public void Truncate_ThrowsArgumentOutOfRange_WhenMaxLengthNegative()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => "hello".Truncate(-1));
        Assert.Equal("maxLength", ex.ParamName);
    }

    [Fact]
    public void Truncate_ReturnsEmpty_WhenEmptyString()
    {
        Assert.Equal("", "".Truncate(5));
    }

    // --- TruncateWithEllipsis ---

    [Fact]
    public void TruncateWithEllipsis_ReturnsUnchanged_WhenWithinLimit()
    {
        Assert.Equal("hello", "hello".TruncateWithEllipsis(10));
    }

    [Fact]
    public void TruncateWithEllipsis_AppendsEllipsis_WhenTruncated()
    {
        Assert.Equal("hel...", "hello world".TruncateWithEllipsis(6));
    }

    [Fact]
    public void TruncateWithEllipsis_ResultNeverExceedsMaxLength()
    {
        var result = "hello world".TruncateWithEllipsis(8);
        Assert.True(result.Length <= 8);
        Assert.Equal("hello...", result);
    }

    [Fact]
    public void TruncateWithEllipsis_TruncatesWithoutEllipsis_WhenMaxLengthLessThan3()
    {
        Assert.Equal("he", "hello".TruncateWithEllipsis(2));
    }

    [Fact]
    public void TruncateWithEllipsis_TruncatesWithoutEllipsis_WhenMaxLengthIs1()
    {
        Assert.Equal("h", "hello".TruncateWithEllipsis(1));
    }

    [Fact]
    public void TruncateWithEllipsis_ReturnsEmpty_WhenMaxLengthIsZero()
    {
        Assert.Equal("", "hello".TruncateWithEllipsis(0));
    }

    [Fact]
    public void TruncateWithEllipsis_ThrowsArgumentOutOfRange_WhenMaxLengthNegative()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => "hello".TruncateWithEllipsis(-1));
        Assert.Equal("maxLength", ex.ParamName);
    }

    [Fact]
    public void TruncateWithEllipsis_ExactlyThree_ReturnsAllEllipsis()
    {
        Assert.Equal("...", "hello".TruncateWithEllipsis(3));
    }

    // --- TruncateWords ---

    [Fact]
    public void TruncateWords_ReturnsUnchanged_WhenFewerWords()
    {
        Assert.Equal("hello world", "hello world".TruncateWords(5));
    }

    [Fact]
    public void TruncateWords_ReturnsUnchanged_WhenExactWordCount()
    {
        Assert.Equal("hello world", "hello world".TruncateWords(2));
    }

    [Fact]
    public void TruncateWords_TruncatesWithDefaultSuffix()
    {
        Assert.Equal("The quick brown...", "The quick brown fox jumps".TruncateWords(3));
    }

    [Fact]
    public void TruncateWords_TruncatesWithCustomSuffix()
    {
        Assert.Equal("The quick [more]", "The quick brown fox".TruncateWords(2, " [more]"));
    }

    [Fact]
    public void TruncateWords_SingleWord()
    {
        Assert.Equal("hello...", "hello world foo".TruncateWords(1));
    }

    [Fact]
    public void TruncateWords_HandlesMultipleSpaces()
    {
        Assert.Equal("hello world...", "hello  world  foo  bar".TruncateWords(2));
    }
}
