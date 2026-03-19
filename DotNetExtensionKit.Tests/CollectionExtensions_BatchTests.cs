using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class CollectionExtensions_BatchTests
{
    [Fact]
    public void Batch_NullSource_ThrowsArgumentNullException()
    {
        IEnumerable<int> source = null!;
        var ex = Assert.Throws<ArgumentNullException>(() => source.Batch(3));
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public void Batch_ZeroBatchSize_ThrowsArgumentOutOfRangeException()
    {
        var source = new[] { 1, 2, 3 };
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => source.Batch(0));
        Assert.Equal("batchSize", ex.ParamName);
    }

    [Fact]
    public void Batch_NegativeBatchSize_ThrowsArgumentOutOfRangeException()
    {
        var source = new[] { 1, 2, 3 };
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => source.Batch(-1));
        Assert.Equal("batchSize", ex.ParamName);
    }

    [Fact]
    public void Batch_EvenlyDivisible_ReturnsFullBatches()
    {
        var source = new[] { 1, 2, 3, 4, 5, 6 };
        var batches = source.Batch(3).Select(b => b.ToArray()).ToArray();

        Assert.Equal(2, batches.Length);
        Assert.Equal(new[] { 1, 2, 3 }, batches[0]);
        Assert.Equal(new[] { 4, 5, 6 }, batches[1]);
    }

    [Fact]
    public void Batch_NotEvenlyDivisible_LastBatchIsSmaller()
    {
        var source = new[] { 1, 2, 3, 4, 5 };
        var batches = source.Batch(3).Select(b => b.ToArray()).ToArray();

        Assert.Equal(2, batches.Length);
        Assert.Equal(new[] { 1, 2, 3 }, batches[0]);
        Assert.Equal(new[] { 4, 5 }, batches[1]);
    }

    [Fact]
    public void Batch_EmptySource_ReturnsNoBatches()
    {
        var source = Array.Empty<int>();
        var batches = source.Batch(3).ToArray();

        Assert.Empty(batches);
    }

    [Fact]
    public void Batch_SingleElement_ReturnsSingleBatch()
    {
        var source = new[] { 42 };
        var batches = source.Batch(5).Select(b => b.ToArray()).ToArray();

        Assert.Single(batches);
        Assert.Equal(new[] { 42 }, batches[0]);
    }

    [Fact]
    public void Batch_BatchSizeOne_EachElementIsOwnBatch()
    {
        var source = new[] { 1, 2, 3 };
        var batches = source.Batch(1).Select(b => b.ToArray()).ToArray();

        Assert.Equal(3, batches.Length);
        Assert.Equal(new[] { 1 }, batches[0]);
        Assert.Equal(new[] { 2 }, batches[1]);
        Assert.Equal(new[] { 3 }, batches[2]);
    }

    [Fact]
    public void Batch_BatchSizeLargerThanSource_ReturnsSingleBatch()
    {
        var source = new[] { 1, 2, 3 };
        var batches = source.Batch(100).Select(b => b.ToArray()).ToArray();

        Assert.Single(batches);
        Assert.Equal(new[] { 1, 2, 3 }, batches[0]);
    }

    [Fact]
    public void Batch_PreservesOrder()
    {
        var source = Enumerable.Range(1, 10);
        var flattened = source.Batch(3).SelectMany(b => b).ToArray();

        Assert.Equal(Enumerable.Range(1, 10).ToArray(), flattened);
    }

    [Fact]
    public void Batch_DeferredExecution_DoesNotEnumerateEagerly()
    {
        var enumerated = false;
        IEnumerable<int> LazySource()
        {
            enumerated = true;
            yield return 1;
        }

        var batches = LazySource().Batch(2);
        // The call to Batch itself should not enumerate the source;
        // only iterating the result should.
        // Note: enumerated is set when LazySource body runs.
        // We need a fresh call to verify deferred execution.
        var enumeratedBeforeIteration = false;
        IEnumerable<int> TrackingSource()
        {
            enumeratedBeforeIteration = true;
            yield return 1;
        }

        var result = TrackingSource().Batch(2);
        Assert.False(enumeratedBeforeIteration);

        // Now iterate
        var _ = result.ToArray();
        Assert.True(enumeratedBeforeIteration);
    }

    [Fact]
    public void Batch_ArgumentValidation_IsEager()
    {
        // Null source should throw immediately, not when iterating
        IEnumerable<int> source = null!;
        Assert.Throws<ArgumentNullException>(() => source.Batch(3));

        // Invalid batch size should throw immediately
        Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Batch(0));
    }
}
