using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class CollectionExtensions_NullEmptyTests
{
    [Fact]
    public void IsNullOrEmpty_NullCollection_ReturnsTrue()
    {
        IEnumerable<int>? collection = null;
        Assert.True(collection.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyCollection_ReturnsTrue()
    {
        var collection = Enumerable.Empty<int>();
        Assert.True(collection.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyCollection_ReturnsFalse()
    {
        var collection = new[] { 1, 2, 3 };
        Assert.False(collection.IsNullOrEmpty());
    }

    [Fact]
    public void HasItems_NullCollection_ReturnsFalse()
    {
        IEnumerable<string>? collection = null;
        Assert.False(collection.HasItems());
    }

    [Fact]
    public void HasItems_EmptyCollection_ReturnsFalse()
    {
        var collection = Array.Empty<string>();
        Assert.False(collection.HasItems());
    }

    [Fact]
    public void HasItems_NonEmptyCollection_ReturnsTrue()
    {
        var collection = new[] { "a" };
        Assert.True(collection.HasItems());
    }

    [Fact]
    public void OrEmpty_NullCollection_ReturnsNonNullEmpty()
    {
        IEnumerable<int>? collection = null;
        var result = collection.OrEmpty();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void OrEmpty_NonNullCollection_ReturnsSameCollection()
    {
        var collection = new[] { 1, 2 };
        var result = collection.OrEmpty();
        Assert.Same(collection, result);
    }

    [Fact]
    public void OrEmptyList_NullList_ReturnsNonNullEmptyList()
    {
        IReadOnlyList<int>? list = null;
        var result = list.OrEmptyList();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void OrEmptyList_NonNullList_ReturnsSameList()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 }.AsReadOnly();
        var result = list.OrEmptyList();
        Assert.Same(list, result);
    }
}
