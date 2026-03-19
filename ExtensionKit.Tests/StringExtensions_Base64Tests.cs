using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class StringExtensions_Base64Tests
    {
        [Fact]
        public void ToBase64_EncodesSimpleString()
        {
            Assert.Equal("SGVsbG8gV29ybGQ=", "Hello World".ToBase64());
        }

        [Fact]
        public void FromBase64_DecodesSimpleString()
        {
            Assert.Equal("Hello World", "SGVsbG8gV29ybGQ=".FromBase64());
        }

        [Fact]
        public void ToBase64_EmptyString_ReturnsEmptyBase64()
        {
            Assert.Equal("", "".ToBase64());
        }

        [Fact]
        public void FromBase64_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal("", "".FromBase64());
        }

        [Fact]
        public void RoundTrip_ReturnsOriginalString()
        {
            var original = "The quick brown fox jumps over the lazy dog!";
            Assert.Equal(original, original.ToBase64().FromBase64());
        }

        [Fact]
        public void RoundTrip_UnicodeString_ReturnsOriginal()
        {
            var original = "Héllo Wörld 日本語";
            Assert.Equal(original, original.ToBase64().FromBase64());
        }
    }
}
