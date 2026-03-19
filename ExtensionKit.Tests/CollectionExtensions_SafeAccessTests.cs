using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class CollectionExtensions_SafeAccessTests
{
    // --- FirstOrDefault with default value ---

    [Fact]
    public void FirstOrDefault_EmptySource_ReturnsDefaultValue()
    {
        var source = Enumerable.Empty<int>();
        var result = source.FirstOrDefault(42);
        Assert.Equal(42, result);
    }

    [Fact]
    public void FirstOrDefault_NonEmptySource_ReturnsFirstElement()
    {
        var source = new[] { 10, 20, 30 };
        var result = source.FirstOrDefault(99);
        Assert.Equal(10, result);
    }

    [Fact]
    public void FirstOrDefault_NullSource_ThrowsArgumentNullException()
    {
        IEnumerable<int> source = null!;
        Assert.Throws<ArgumentNullException>(() => source.FirstOrDefault(0));
    }

    [Fact]
    public void FirstOrDefault_EmptyStringSource_ReturnsDefaultString()
    {
        var source = Enumerable.Empty<string>();
        var result = source.FirstOrDefault("fallback");
        Assert.Equal("fallback", result);
    }

    [Fact]
    public void FirstOrDefault_SingleElement_ReturnsThatElement()
    {
        var source = new[] { "only" };
        var result = source.FirstOrDefault("fallback");
        Assert.Equal("only", result);
    }

    // --- GetValueOrDefault ---

    [Fact]
    public void GetValueOrDefault_KeyExists_ReturnsValue()
    {
        var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
        var result = dict.GetValueOrDefault("a", -1);
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetValueOrDefault_KeyMissing_ReturnsDefaultValue()
    {
        var dict = new Dictionary<string, int> { { "a", 1 } };
        var result = dict.GetValueOrDefault("z", 99);
        Assert.Equal(99, result);
    }

    [Fact]
    public void GetValueOrDefault_KeyMissing_NoDefaultSpecified_ReturnsTypeDefault()
    {
        var dict = new Dictionary<string, int> { { "a", 1 } };
        var result = dict.GetValueOrDefault("z");
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetValueOrDefault_NullDictionary_ThrowsArgumentNullException()
    {
        IDictionary<string, int> dict = null!;
        Assert.Throws<ArgumentNullException>(() => dict.GetValueOrDefault("key", 0));
    }

    [Fact]
    public void GetValueOrDefault_EmptyDictionary_ReturnsDefaultValue()
    {
        var dict = new Dictionary<int, string>();
        var result = dict.GetValueOrDefault(1, "missing");
        Assert.Equal("missing", result);
    }
}
