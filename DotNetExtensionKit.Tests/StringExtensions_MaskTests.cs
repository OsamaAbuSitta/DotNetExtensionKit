using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class StringExtensions_MaskTests
    {
        [Fact]
        public void Mask_NullInput_ReturnsNull()
        {
            string? value = null;
            Assert.Null(value!.Mask());
        }

        [Fact]
        public void Mask_EmptyInput_ReturnsEmpty()
        {
            Assert.Equal("", "".Mask());
        }

        [Fact]
        public void Mask_DefaultParameters_MasksMiddle()
        {
            // "Hello!" => "He**o!" with defaults (unmaskedStart=2, unmaskedEnd=2)
            Assert.Equal("He**o!", "Hello!".Mask());
        }

        [Fact]
        public void Mask_PreservesLength()
        {
            var input = "1234567890";
            var result = input.Mask();
            Assert.Equal(input.Length, result.Length);
        }

        [Fact]
        public void Mask_UnmaskedExceedsLength_ReturnsAllMaskChars()
        {
            // unmaskedStart(2) + unmaskedEnd(2) >= length(3)
            Assert.Equal("***", "abc".Mask());
        }

        [Fact]
        public void Mask_UnmaskedEqualsLength_ReturnsAllMaskChars()
        {
            // unmaskedStart(2) + unmaskedEnd(2) == length(4)
            Assert.Equal("****", "abcd".Mask());
        }

        [Fact]
        public void Mask_CustomMaskChar()
        {
            Assert.Equal("He##o!", "Hello!".Mask(2, 2, '#'));
        }

        [Fact]
        public void Mask_ZeroUnmaskedStart()
        {
            // unmaskedStart=0, unmaskedEnd=2 on "Hello!"
            Assert.Equal("****o!", "Hello!".Mask(0, 2));
        }

        [Fact]
        public void Mask_ZeroUnmaskedEnd()
        {
            Assert.Equal("He****", "Hello!".Mask(2, 0));
        }

        [Fact]
        public void Mask_LongerString()
        {
            // "4111111111111111" with defaults => "41************11"
            var result = "4111111111111111".Mask();
            Assert.Equal("41************11", result);
            Assert.Equal(16, result.Length);
        }
    }
}
