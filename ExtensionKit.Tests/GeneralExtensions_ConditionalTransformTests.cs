using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class GeneralExtensions_ConditionalTransformTests
    {
        // If<T> tests

        [Fact]
        public void If_TrueCondition_ReturnsValue()
        {
            var result = "hello".If(true);
            Assert.Equal("hello", result);
        }

        [Fact]
        public void If_FalseCondition_ReturnsDefault()
        {
            var result = "hello".If(false);
            Assert.Null(result);
        }

        [Fact]
        public void If_TrueCondition_ValueType_ReturnsValue()
        {
            var result = 42.If(true);
            Assert.Equal(42, result);
        }

        [Fact]
        public void If_FalseCondition_ValueType_ReturnsDefault()
        {
            var result = 42.If(false);
            Assert.Equal(0, result);
        }

        // IfNot<T> tests

        [Fact]
        public void IfNot_FalseCondition_ReturnsValue()
        {
            var result = "hello".IfNot(false);
            Assert.Equal("hello", result);
        }

        [Fact]
        public void IfNot_TrueCondition_ReturnsDefault()
        {
            var result = "hello".IfNot(true);
            Assert.Null(result);
        }

        [Fact]
        public void IfNot_FalseCondition_ValueType_ReturnsValue()
        {
            var result = 42.IfNot(false);
            Assert.Equal(42, result);
        }

        [Fact]
        public void IfNot_TrueCondition_ValueType_ReturnsDefault()
        {
            var result = 42.IfNot(true);
            Assert.Equal(0, result);
        }

        // If and IfNot symmetry: If(value, cond) == IfNot(value, !cond)

        [Fact]
        public void If_IfNot_Symmetry_TrueCondition()
        {
            var value = "test";
            Assert.Equal(value.If(true), value.IfNot(false));
        }

        [Fact]
        public void If_IfNot_Symmetry_FalseCondition()
        {
            var value = "test";
            Assert.Equal(value.If(false), value.IfNot(true));
        }

        // Transform<T, TResult> tests

        [Fact]
        public void Transform_AppliesFunction()
        {
            var result = 5.Transform(x => x * 2);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Transform_StringToInt()
        {
            var result = "hello".Transform(s => s.Length);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Transform_ChangesType()
        {
            var result = 42.Transform(x => x.ToString());
            Assert.Equal("42", result);
        }

        // Also<T> tests

        [Fact]
        public void Also_ReturnsOriginalValue()
        {
            var result = "hello".Also(s => { });
            Assert.Equal("hello", result);
        }

        [Fact]
        public void Also_ExecutesAction()
        {
            string? captured = null;
            "hello".Also(s => captured = s);
            Assert.Equal("hello", captured);
        }

        [Fact]
        public void Also_ValueType_ReturnsOriginal()
        {
            int sideEffect = 0;
            var result = 42.Also(x => sideEffect = x);
            Assert.Equal(42, result);
            Assert.Equal(42, sideEffect);
        }
    }
}
