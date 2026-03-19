using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetExtensionKit
{
    /// <summary>
    /// Provides extension methods for collections and LINQ helpers including
    /// null/empty checks, batching, iteration, and safe element access.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns true if the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns><c>true</c> if <paramref name="collection"/> is <c>null</c> or empty; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// List&lt;int&gt;? items = null;
        /// bool result = items.IsNullOrEmpty(); // true
        ///
        /// items = new List&lt;int&gt; { 1, 2 };
        /// result = items.IsNullOrEmpty(); // false
        /// </code>
        /// </example>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Returns true if the collection is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns><c>true</c> if <paramref name="collection"/> is not <c>null</c> and contains at least one element; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// var names = new List&lt;string&gt; { "Alice" };
        /// bool result = names.HasItems(); // true
        ///
        /// List&lt;string&gt;? empty = null;
        /// result = empty.HasItems(); // false
        /// </code>
        /// </example>
        public static bool HasItems<T>(this IEnumerable<T>? collection)
        {
            return !collection.IsNullOrEmpty();
        }

        /// <summary>
        /// Returns the collection if non-null, or an empty enumerable otherwise.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to return, or <c>null</c>.</param>
        /// <returns>The original <paramref name="collection"/> if non-null; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        /// <example>
        /// <code>
        /// IEnumerable&lt;int&gt;? numbers = null;
        /// foreach (var n in numbers.OrEmpty())
        /// {
        ///     // safely iterates — no NullReferenceException
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T>? collection)
        {
            return collection ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Returns the list if non-null, or an empty read-only list otherwise.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to return, or <c>null</c>.</param>
        /// <returns>The original <paramref name="list"/> if non-null; otherwise, an empty <see cref="IReadOnlyList{T}"/>.</returns>
        /// <example>
        /// <code>
        /// IReadOnlyList&lt;string&gt;? names = null;
        /// IReadOnlyList&lt;string&gt; safe = names.OrEmptyList();
        /// int count = safe.Count; // 0
        /// </code>
        /// </example>
        public static IReadOnlyList<T> OrEmptyList<T>(this IReadOnlyList<T>? list)
        {
            return list ?? (IReadOnlyList<T>)Array.Empty<T>();
        }

        /// <summary>
        /// Splits the source sequence into batches of at most <paramref name="batchSize"/> elements.
        /// Uses deferred execution; the source is enumerated lazily.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source.</typeparam>
        /// <param name="source">The source sequence to batch.</param>
        /// <param name="batchSize">The maximum number of elements per batch.</param>
        /// <returns>A sequence of batches, each containing at most <paramref name="batchSize"/> elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="batchSize"/> is zero or negative.</exception>
        /// <example>
        /// <code>
        /// var numbers = new[] { 1, 2, 3, 4, 5 };
        /// var batches = numbers.Batch(2);
        /// // batches: { {1, 2}, {3, 4}, {5} }
        /// </code>
        /// </example>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), batchSize, "Batch size must be greater than zero.");

            return BatchIterator(source, batchSize);
        }

        private static IEnumerable<IEnumerable<T>> BatchIterator<T>(IEnumerable<T> source, int batchSize)
        {
            var batch = new List<T>(batchSize);

            foreach (var item in source)
            {
                batch.Add(item);

                if (batch.Count == batchSize)
                {
                    yield return batch.ToArray();
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                yield return batch.ToArray();
            }
        }

        /// <summary>
        /// Executes the specified action on each element of the source sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source.</typeparam>
        /// <param name="source">The source sequence to iterate.</param>
        /// <param name="action">The action to execute on each element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is null.</exception>
        /// <example>
        /// <code>
        /// var names = new[] { "Alice", "Bob" };
        /// names.ForEach(name => Console.WriteLine(name));
        /// </code>
        /// </example>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Executes the specified action on each element of the source sequence, providing the zero-based index.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source.</typeparam>
        /// <param name="source">The source sequence to iterate.</param>
        /// <param name="action">The action to execute on each element with its zero-based index.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is null.</exception>
        /// <example>
        /// <code>
        /// var names = new[] { "Alice", "Bob" };
        /// names.ForEach((name, index) => Console.WriteLine($"{index}: {name}"));
        /// </code>
        /// </example>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            int index = 0;
            foreach (var item in source)
            {
                action(item, index);
                index++;
            }
        }

        /// <summary>
        /// Returns distinct elements from the source sequence based on a key selector,
        /// keeping the first occurrence of each key.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source.</typeparam>
        /// <typeparam name="TKey">The type of the key used for distinct comparison.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">A function to extract the key from each element.</param>
        /// <returns>A sequence of elements with unique keys, preserving the first occurrence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <example>
        /// <code>
        /// var people = new[] { ("Alice", 30), ("Bob", 25), ("Alice", 28) };
        /// var distinct = people.DistinctBy(p => p.Item1);
        /// // distinct: { ("Alice", 30), ("Bob", 25) }
        /// </code>
        /// </example>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctByIterator(source, keySelector);
        }

        private static IEnumerable<T> DistinctByIterator<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var seen = new HashSet<TKey>();

            foreach (var item in source)
            {
                if (seen.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Returns the first element of the sequence, or the specified default value if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="defaultValue">The value to return if the sequence is empty.</param>
        /// <returns>The first element if the sequence is non-empty; otherwise, <paramref name="defaultValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <example>
        /// <code>
        /// var empty = Enumerable.Empty&lt;int&gt;();
        /// int result = empty.FirstOrDefault(-1); // -1
        ///
        /// var numbers = new[] { 10, 20 };
        /// result = numbers.FirstOrDefault(-1); // 10
        /// </code>
        /// </example>
        public static T FirstOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    return enumerator.Current;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the value associated with the specified key, or the specified default value if the key is not found.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="T">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key to look up.</param>
        /// <param name="defaultValue">The value to return if the key is not found.</param>
        /// <returns>The value for the key if found; otherwise, <paramref name="defaultValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
        /// <example>
        /// <code>
        /// var map = new Dictionary&lt;string, int&gt; { ["a"] = 1 };
        /// int val = map.GetValueOrDefault("b", 42); // 42
        /// val = map.GetValueOrDefault("a", 42); // 1
        /// </code>
        /// </example>
        public static T GetValueOrDefault<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, T defaultValue = default!)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
