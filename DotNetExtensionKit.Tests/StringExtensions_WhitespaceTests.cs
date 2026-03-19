using System.Linq;
using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class StringExtensions_WhitespaceTests
    {
        // RemoveWhitespace tests

        [Fact]
        public void RemoveWhitespace_RemovesSpaces()
        {
            Assert.Equal("helloworld", "hello world".RemoveWhitespace());
        }

        [Fact]
        public void RemoveWhitespace_RemovesTabs()
        {
            Assert.Equal("ab", "a\tb".RemoveWhitespace());
        }

        [Fact]
        public void RemoveWhitespace_RemovesNewlines()
        {
            Assert.Equal("ab", "a\nb".RemoveWhitespace());
        }

        [Fact]
        public void RemoveWhitespace_RemovesMixedWhitespace()
        {
            Assert.Equal("abc", " a\t b \r\n c ".RemoveWhitespace());
        }

        [Fact]
        public void RemoveWhitespace_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".RemoveWhitespace());
        }

        [Fact]
        public void RemoveWhitespace_NoWhitespace_ReturnsSame()
        {
            Assert.Equal("hello", "hello".RemoveWhitespace());
        }

        // CollapseWhitespace tests

        [Fact]
        public void CollapseWhitespace_CollapsesMultipleSpaces()
        {
            Assert.Equal("hello world", "hello   world".CollapseWhitespace());
        }

        [Fact]
        public void CollapseWhitespace_CollapsesTabs()
        {
            Assert.Equal("a b", "a\t\tb".CollapseWhitespace());
        }

        [Fact]
        public void CollapseWhitespace_CollapsesMixedWhitespace()
        {
            Assert.Equal(" a b c ", " a \t b \r\n c ".CollapseWhitespace());
        }

        [Fact]
        public void CollapseWhitespace_SingleSpaces_Unchanged()
        {
            Assert.Equal("a b c", "a b c".CollapseWhitespace());
        }

        [Fact]
        public void CollapseWhitespace_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".CollapseWhitespace());
        }

        // SplitLines tests

        [Fact]
        public void SplitLines_SplitsOnNewline()
        {
            var result = "a\nb\nc".SplitLines().ToArray();
            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void SplitLines_SplitsOnCarriageReturn()
        {
            var result = "a\rb\rc".SplitLines().ToArray();
            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void SplitLines_SplitsOnCrLf()
        {
            var result = "a\r\nb\r\nc".SplitLines().ToArray();
            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void SplitLines_MixedLineEndings()
        {
            var result = "a\nb\r\nc\rd".SplitLines().ToArray();
            Assert.Equal(new[] { "a", "b", "c", "d" }, result);
        }

        [Fact]
        public void SplitLines_NoLineBreaks_ReturnsSingleElement()
        {
            var result = "hello".SplitLines().ToArray();
            Assert.Equal(new[] { "hello" }, result);
        }

        [Fact]
        public void SplitLines_EmptyString_ReturnsSingleEmptyElement()
        {
            var result = "".SplitLines().ToArray();
            Assert.Equal(new[] { "" }, result);
        }

        [Fact]
        public void SplitLines_TrailingNewline_IncludesEmptyLast()
        {
            var result = "a\nb\n".SplitLines().ToArray();
            Assert.Equal(new[] { "a", "b", "" }, result);
        }
    }
}
