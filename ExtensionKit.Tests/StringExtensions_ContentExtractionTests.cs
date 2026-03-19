using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class StringExtensions_ContentExtractionTests
    {
        [Fact]
        public void DigitsOnly_ReturnsOnlyDigits()
        {
            Assert.Equal("123456", "abc123def456".DigitsOnly());
        }

        [Fact]
        public void DigitsOnly_NoDigits_ReturnsEmpty()
        {
            Assert.Equal("", "abcdef".DigitsOnly());
        }

        [Fact]
        public void DigitsOnly_AllDigits_ReturnsSameString()
        {
            Assert.Equal("12345", "12345".DigitsOnly());
        }

        [Fact]
        public void DigitsOnly_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".DigitsOnly());
        }

        [Fact]
        public void LettersOnly_ReturnsOnlyLetters()
        {
            Assert.Equal("abcdef", "abc123def456".LettersOnly());
        }

        [Fact]
        public void LettersOnly_NoLetters_ReturnsEmpty()
        {
            Assert.Equal("", "123456".LettersOnly());
        }

        [Fact]
        public void LettersOnly_AllLetters_ReturnsSameString()
        {
            Assert.Equal("abcdef", "abcdef".LettersOnly());
        }

        [Fact]
        public void LettersOnly_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".LettersOnly());
        }

        [Fact]
        public void AlphaNumericOnly_ReturnsOnlyLettersAndDigits()
        {
            Assert.Equal("helloworld123", "hello-world_123!".AlphaNumericOnly());
        }

        [Fact]
        public void AlphaNumericOnly_NoAlphaNumeric_ReturnsEmpty()
        {
            Assert.Equal("", "!@#$%^&*()".AlphaNumericOnly());
        }

        [Fact]
        public void AlphaNumericOnly_AllAlphaNumeric_ReturnsSameString()
        {
            Assert.Equal("abc123", "abc123".AlphaNumericOnly());
        }

        [Fact]
        public void AlphaNumericOnly_EmptyString_ReturnsEmpty()
        {
            Assert.Equal("", "".AlphaNumericOnly());
        }

        [Fact]
        public void DigitsOnly_PreservesOrder()
        {
            Assert.Equal("9081", "a9b0c8d1".DigitsOnly());
        }

        [Fact]
        public void LettersOnly_PreservesOrder()
        {
            Assert.Equal("abcd", "a9b0c8d1".LettersOnly());
        }

        [Fact]
        public void AlphaNumericOnly_PreservesOrder()
        {
            Assert.Equal("a9b0c8d1", "a9!b0@c8#d1$".AlphaNumericOnly());
        }
    }
}
