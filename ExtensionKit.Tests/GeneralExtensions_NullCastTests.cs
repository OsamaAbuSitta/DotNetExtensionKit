using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class GeneralExtensions_NullCastTests
    {
        // IsNull tests
        [Fact]
        public void IsNull_NullReference_ReturnsTrue()
        {
            string? value = null;
            Assert.True(value.IsNull());
        }

        [Fact]
        public void IsNull_NonNullReference_ReturnsFalse()
        {
            string value = "hello";
            Assert.False(value.IsNull());
        }

        // IsNotNull tests
        [Fact]
        public void IsNotNull_NonNullReference_ReturnsTrue()
        {
            string value = "hello";
            Assert.True(value.IsNotNull());
        }

        [Fact]
        public void IsNotNull_NullReference_ReturnsFalse()
        {
            string? value = null;
            Assert.False(value.IsNotNull());
        }

        // IsNull and IsNotNull are complementary
        [Fact]
        public void IsNull_IsNotNull_AlwaysComplementary_NonNull()
        {
            object value = new object();
            Assert.NotEqual(value.IsNull(), value.IsNotNull());
        }

        [Fact]
        public void IsNull_IsNotNull_AlwaysComplementary_Null()
        {
            object? value = null;
            Assert.NotEqual(value.IsNull(), value.IsNotNull());
        }

        // As<T> tests
        [Fact]
        public void As_CompatibleType_ReturnsCastValue()
        {
            object value = "hello";
            var result = value.As<string>();
            Assert.Equal("hello", result);
        }

        [Fact]
        public void As_IncompatibleType_ReturnsNull()
        {
            object value = "hello";
            var result = value.As<System.Uri>();
            Assert.Null(result);
        }

        [Fact]
        public void As_NullInput_ReturnsNull()
        {
            object? value = null;
            var result = value.As<string>();
            Assert.Null(result);
        }

        [Fact]
        public void As_DerivedType_ReturnsCastValue()
        {
            object value = new System.ArgumentException("test");
            var result = value.As<System.Exception>();
            Assert.NotNull(result);
            Assert.Equal("test", result!.Message);
        }

        [Fact]
        public void As_BaseToUnrelatedDerived_ReturnsNull()
        {
            System.Exception value = new System.Exception("test");
            var result = ((object)value).As<System.ArgumentException>();
            Assert.Null(result);
        }
    }
}
