using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetExtensionKit
{
    /// <summary>
    /// Provides extension methods for string manipulation, validation,
    /// transformation, and encoding.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns true if the string is null or has zero length.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is <c>null</c> or empty; otherwise <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// string? name = null;
        /// bool result = name.IsNullOrEmpty(); // true
        /// bool result2 = "hello".IsNullOrEmpty(); // false
        /// </code>
        /// </example>
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Returns true if the string is null, empty, or contains only whitespace characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is <c>null</c>, empty, or whitespace-only; otherwise <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// bool result = "  ".IsNullOrWhiteSpace(); // true
        /// bool result2 = "hello".IsNullOrWhiteSpace(); // false
        /// </code>
        /// </example>
        public static bool IsNullOrWhiteSpace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        
        /// <summary>
        /// Returns null if the string is null or empty; otherwise returns the original string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>The original string if it is not <c>null</c> or empty; otherwise <c>null</c>.</returns>
        /// <example>
        /// <code>
        /// string? result = "".NullIfEmpty(); // null
        /// string? result2 = "hello".NullIfEmpty(); // "hello"
        /// </code>
        /// </example>
        public static string? NullIfEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// Returns null if the string is null, empty, or contains only whitespace;
        /// otherwise returns the original string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>The original string if it contains at least one non-whitespace character; otherwise <c>null</c>.</returns>
        /// <example>
        /// <code>
        /// string? result = "  ".NullIfWhiteSpace(); // null
        /// string? result2 = "hello".NullIfWhiteSpace(); // "hello"
        /// </code>
        /// </example>
        public static string? NullIfWhiteSpace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        /// Returns an empty string if the string is null; otherwise returns the original string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>An empty string if <paramref name="value"/> is <c>null</c>; otherwise the original string.</returns>
        /// <example>
        /// <code>
        /// string result = ((string?)null).EmptyIfNull(); // ""
        /// string result2 = "hello".EmptyIfNull(); // "hello"
        /// </code>
        /// </example>
        public static string EmptyIfNull(this string? value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Returns the first <paramref name="maxLength"/> characters of the string.
        /// If the string length is at most maxLength, returns the string unchanged.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum number of characters to return.</param>
        /// <returns>A string containing at most <paramref name="maxLength"/> characters from the start of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxLength"/> is negative.</exception>
        /// <example>
        /// <code>
        /// string result = "Hello, World!".Truncate(5); // "Hello"
        /// string result2 = "Hi".Truncate(10); // "Hi"
        /// </code>
        /// </example>
        public static string Truncate(this string value, int maxLength)
        {
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "maxLength must be non-negative.");

            if (value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength);
        }

        /// <summary>
        /// Truncates the string and appends an ellipsis ("...") within the maxLength limit.
        /// If maxLength is less than 3, truncates without ellipsis.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum total length of the returned string, including the ellipsis.</param>
        /// <returns>A string of at most <paramref name="maxLength"/> characters, with trailing "..." if truncation occurred and <paramref name="maxLength"/> is 3 or greater.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxLength"/> is negative.</exception>
        /// <example>
        /// <code>
        /// string result = "Hello, World!".TruncateWithEllipsis(8); // "Hello..."
        /// string result2 = "Hi".TruncateWithEllipsis(10); // "Hi"
        /// </code>
        /// </example>
        public static string TruncateWithEllipsis(this string value, int maxLength)
        {
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "maxLength must be non-negative.");

            if (value.Length <= maxLength)
                return value;

            if (maxLength < 3)
                return value.Substring(0, maxLength);

            return value.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Returns the first <paramref name="maxWords"/> words followed by the specified suffix.
        /// Words are split by whitespace. If the string has fewer or equal words, returns it unchanged.
        /// </summary>
        /// <param name="value">The string to truncate by word count.</param>
        /// <param name="maxWords">The maximum number of words to keep.</param>
        /// <param name="suffix">The suffix appended after truncation. Defaults to "...".</param>
        /// <returns>A string containing at most <paramref name="maxWords"/> words followed by <paramref name="suffix"/> if truncation occurred; otherwise the original string.</returns>
        /// <example>
        /// <code>
        /// string result = "The quick brown fox jumps".TruncateWords(3); // "The quick brown..."
        /// string result2 = "Hello".TruncateWords(5); // "Hello"
        /// </code>
        /// </example>
        public static string TruncateWords(this string value, int maxWords, string suffix = "...")
        {
            var words = value.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length <= maxWords)
                return value;

            return string.Join(" ", words.Take(maxWords)) + suffix;
        }

        /// <summary>
        /// Inserts spaces at casing boundary positions in PascalCase or camelCase strings.
        /// Handles acronyms properly (e.g., "XMLParser" → "XML Parser").
        /// </summary>
        /// <param name="value">The PascalCase or camelCase string to split.</param>
        /// <returns>A new string with spaces inserted before uppercase letters at casing boundaries.</returns>
        /// <example>
        /// <code>
        /// string result = "HelloWorld".SplitByCasing(); // "Hello World"
        /// string result2 = "XMLParser".SplitByCasing(); // "XML Parser"
        /// </code>
        /// </example>
        public static string SplitByCasing(this string value)
        {
            if (value.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(value.Length + 10);
            sb.Append(value[0]);

            for (int i = 1; i < value.Length; i++)
            {
                char current = value[i];
                char previous = value[i - 1];

                if (char.IsUpper(current))
                {
                    // Insert space before uppercase if previous is lowercase
                    if (char.IsLower(previous))
                    {
                        sb.Append(' ');
                    }
                    // Insert space before uppercase if it starts a new word after an acronym
                    // e.g., "XMLParser" → space before 'P' because next char 'a' is lowercase
                    else if (char.IsUpper(previous) && i + 1 < value.Length && char.IsLower(value[i + 1]))
                    {
                        sb.Append(' ');
                    }
                }

                sb.Append(current);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Capitalizes the first letter of each word using invariant culture rules.
        /// </summary>
        /// <param name="value">The string to convert to title case.</param>
        /// <returns>A new string with the first letter of each word capitalized and the remaining letters lowercased.</returns>
        /// <example>
        /// <code>
        /// string result = "hello world".ToTitleCaseInvariant(); // "Hello World"
        /// string result2 = "the QUICK fox".ToTitleCaseInvariant(); // "The Quick Fox"
        /// </code>
        /// </example>
        public static string ToTitleCaseInvariant(this string value)
        {
            if (value.Length == 0)
                return string.Empty;

            var words = value.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder(value.Length);

            for (int w = 0; w < words.Length; w++)
            {
                if (w > 0)
                    sb.Append(' ');

                sb.Append(char.ToUpperInvariant(words[w][0]));
                sb.Append(words[w].Substring(1).ToLowerInvariant());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a camelCase string: first character lowercase, subsequent word-initial characters uppercase.
        /// Splits input by whitespace to identify words.
        /// </summary>
        /// <param name="value">The string to convert to camelCase.</param>
        /// <returns>A camelCase representation of the input string.</returns>
        /// <example>
        /// <code>
        /// string result = "Hello World".ToCamelCase(); // "helloWorld"
        /// string result2 = "the quick fox".ToCamelCase(); // "theQuickFox"
        /// </code>
        /// </example>
        public static string ToCamelCase(this string value)
        {
            if (value.Length == 0)
                return string.Empty;

            var words = value.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(value.Length);

            // First word: all lowercase
            sb.Append(words[0].ToLowerInvariant());

            // Subsequent words: capitalize first letter, lowercase rest
            for (int w = 1; w < words.Length; w++)
            {
                sb.Append(char.ToUpperInvariant(words[w][0]));
                sb.Append(words[w].Substring(1).ToLowerInvariant());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a lowercase string with underscores at casing boundary positions.
        /// Handles acronyms properly (e.g., "XMLParser" → "xml_parser").
        /// </summary>
        /// <param name="value">The string to convert to snake_case.</param>
        /// <returns>A snake_case representation of the input string.</returns>
        /// <example>
        /// <code>
        /// string result = "HelloWorld".ToSnakeCase(); // "hello_world"
        /// string result2 = "XMLParser".ToSnakeCase(); // "xml_parser"
        /// </code>
        /// </example>
        public static string ToSnakeCase(this string value)
        {
            if (value.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(value.Length + 10);
            sb.Append(char.ToLowerInvariant(value[0]));

            for (int i = 1; i < value.Length; i++)
            {
                char current = value[i];
                char previous = value[i - 1];

                if (char.IsUpper(current))
                {
                    if (char.IsLower(previous))
                    {
                        sb.Append('_');
                    }
                    else if (char.IsUpper(previous) && i + 1 < value.Length && char.IsLower(value[i + 1]))
                    {
                        sb.Append('_');
                    }
                }

                sb.Append(char.ToLowerInvariant(current));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a lowercase string with hyphens at casing boundary positions.
        /// Handles acronyms properly (e.g., "XMLParser" → "xml-parser").
        /// </summary>
        /// <param name="value">The string to convert to kebab-case.</param>
        /// <returns>A kebab-case representation of the input string.</returns>
        /// <example>
        /// <code>
        /// string result = "HelloWorld".ToKebabCase(); // "hello-world"
        /// string result2 = "XMLParser".ToKebabCase(); // "xml-parser"
        /// </code>
        /// </example>
        public static string ToKebabCase(this string value)
        {
            if (value.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(value.Length + 10);
            sb.Append(char.ToLowerInvariant(value[0]));

            for (int i = 1; i < value.Length; i++)
            {
                char current = value[i];
                char previous = value[i - 1];

                if (char.IsUpper(current))
                {
                    if (char.IsLower(previous))
                    {
                        sb.Append('-');
                    }
                    else if (char.IsUpper(previous) && i + 1 < value.Length && char.IsLower(value[i + 1]))
                    {
                        sb.Append('-');
                    }
                }

                sb.Append(char.ToLowerInvariant(current));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Masks the middle characters of a string, preserving the specified number of characters
        /// at the start and end. Returns null/empty input unchanged. Result length always equals input length.
        /// </summary>
        /// <param name="value">The string to mask.</param>
        /// <param name="unmaskedStart">Number of characters to preserve at the start.</param>
        /// <param name="unmaskedEnd">Number of characters to preserve at the end.</param>
        /// <param name="maskChar">The character used for masking.</param>
        /// <returns>A masked string with the middle characters replaced by <paramref name="maskChar"/>, or the original string if it is <c>null</c> or empty.</returns>
        /// <example>
        /// <code>
        /// string result = "1234567890".Mask(); // "12******90"
        /// string result2 = "secret".Mask(1, 1, '#'); // "s####t"
        /// </code>
        /// </example>
        public static string Mask(this string value, int unmaskedStart = 2, int unmaskedEnd = 2, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (unmaskedStart + unmaskedEnd >= value.Length)
                return new string(maskChar, value.Length);

            var sb = new StringBuilder(value.Length);
            sb.Append(value, 0, unmaskedStart);
            sb.Append(maskChar, value.Length - unmaskedStart - unmaskedEnd);
            sb.Append(value, value.Length - unmaskedEnd, unmaskedEnd);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the first <paramref name="length"/> characters of the string.
        /// If length exceeds the string length, returns the full string.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="length">The number of characters to return from the start.</param>
        /// <returns>A string containing the first <paramref name="length"/> characters, or the full string if it is shorter than <paramref name="length"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length"/> is negative.</exception>
        /// <example>
        /// <code>
        /// string result = "Hello, World!".Left(5); // "Hello"
        /// string result2 = "Hi".Left(10); // "Hi"
        /// </code>
        /// </example>
        public static string Left(this string value, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "length must be non-negative.");

            if (length >= value.Length)
                return value;

            return value.Substring(0, length);
        }

        /// <summary>
        /// Returns the last <paramref name="length"/> characters of the string.
        /// If length exceeds the string length, returns the full string.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="length">The number of characters to return from the end.</param>
        /// <returns>A string containing the last <paramref name="length"/> characters, or the full string if it is shorter than <paramref name="length"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length"/> is negative.</exception>
        /// <example>
        /// <code>
        /// string result = "Hello, World!".Right(6); // "World!"
        /// string result2 = "Hi".Right(10); // "Hi"
        /// </code>
        /// </example>
        public static string Right(this string value, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "length must be non-negative.");

            if (length >= value.Length)
                return value;

            return value.Substring(value.Length - length);
        }

        /// <summary>
        /// Returns a string containing only digit characters from the input, preserving their original order.
        /// </summary>
        /// <param name="value">The source string to filter.</param>
        /// <returns>A string containing only the digit characters from <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// string result = "abc123def456".DigitsOnly(); // "123456"
        /// </code>
        /// </example>
        public static string DigitsOnly(this string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Returns a string containing only letter characters from the input, preserving their original order.
        /// </summary>
        /// <param name="value">The source string to filter.</param>
        /// <returns>A string containing only the letter characters from <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// string result = "abc123def456".LettersOnly(); // "abcdef"
        /// </code>
        /// </example>
        public static string LettersOnly(this string value)
        {
            return new string(value.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Returns a string containing only letter and digit characters from the input, preserving their original order.
        /// </summary>
        /// <param name="value">The source string to filter.</param>
        /// <returns>A string containing only the letter and digit characters from <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// string result = "hello world! 123".AlphaNumericOnly(); // "helloworld123"
        /// </code>
        /// </example>
        public static string AlphaNumericOnly(this string value)
        {
            return new string(value.Where(char.IsLetterOrDigit).ToArray());
        }

        /// <summary>
        /// Returns the string with all whitespace characters removed.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <returns>A new string with all whitespace characters removed from <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// string result = "hello world".RemoveWhitespace(); // "helloworld"
        /// </code>
        /// </example>
        public static string RemoveWhitespace(this string value)
        {
            return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// Replaces consecutive whitespace characters with a single space.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <returns>A new string where runs of consecutive whitespace in <paramref name="value"/> are replaced by a single space.</returns>
        /// <example>
        /// <code>
        /// string result = "hello   world".CollapseWhitespace(); // "hello world"
        /// </code>
        /// </example>
        public static string CollapseWhitespace(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Returns an enumerable of strings split by line-break characters (\r\n, \r, \n).
        /// </summary>
        /// <param name="value">The source string to split into lines.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings, one per line in <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// var lines = "line1\nline2\nline3".SplitLines(); // ["line1", "line2", "line3"]
        /// </code>
        /// </example>
        public static IEnumerable<string> SplitLines(this string value)
        {
            return value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Returns the UTF-8 Base64-encoded representation of the string.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>A Base64-encoded string representing the UTF-8 bytes of <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// string result = "Hello".ToBase64(); // "SGVsbG8="
        /// </code>
        /// </example>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Decodes a Base64 string back to its original UTF-8 string representation.
        /// </summary>
        /// <param name="value">The Base64-encoded string to decode.</param>
        /// <returns>The decoded UTF-8 string.</returns>
        /// <example>
        /// <code>
        /// string result = "SGVsbG8=".FromBase64(); // "Hello"
        /// </code>
        /// </example>
        public static string FromBase64(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        /// <summary>
        /// Returns a URL-friendly slug: lowercase, only [a-z0-9-], consecutive whitespace collapsed to single hyphen.
        /// </summary>
        /// <param name="value">The string to convert to a URL slug.</param>
        /// <returns>A URL-friendly slug derived from <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// string result = "Hello World!".ToSlug(); // "hello-world"
        /// string result2 = "  Multiple   Spaces  ".ToSlug(); // "multiple-spaces"
        /// </code>
        /// </example>
        public static string ToSlug(this string value)
        {
            if (value.Length == 0)
                return string.Empty;

            // Step 1: Convert to lowercase
            var result = value.ToLowerInvariant();

            // Step 2: Remove non-alphanumeric characters (keep spaces and hyphens)
            result = Regex.Replace(result, @"[^a-z0-9\s-]", "");

            // Step 3: Collapse multiple whitespace to single space and trim
            result = Regex.Replace(result, @"\s+", " ").Trim();

            // Step 4: Replace spaces with hyphens
            result = result.Replace(" ", "-");

            return result;
        }
    }
}
