using System;
using DotNetExtensionKit;

namespace DotNetExtensionKit.Tests;

public class GeneralExtensions_CallGenericMethodTests
{
    // Helper class with generic methods for testing
    private class TestHelper
    {
        public T Echo<T>(T value) => value;

        public string TypeName<T>() => typeof(T).Name;

        public TResult Convert<TInput, TResult>(TInput input) where TResult : IConvertible
            => (TResult)System.Convert.ChangeType(input!, typeof(TResult))!;

        public static T StaticEcho<T>(T value) => value;

        private T PrivateEcho<T>(T value) => value;

        public T Thrower<T>(T value) => throw new ArgumentException("test error");

        public void NoSuchGeneric(int x) { }
    }

    // --- Requirement 21.1: Type-based overload, successful invocation ---

    [Fact]
    public void CallGenericMethod_Type_InvokesInstanceMethod()
    {
        var helper = new TestHelper();
        var result = typeof(TestHelper).CallGenericMethod(
            "Echo", new[] { typeof(int) }, helper, 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void CallGenericMethod_Type_InvokesStaticMethod()
    {
        var result = typeof(TestHelper).CallGenericMethod(
            "StaticEcho", new[] { typeof(string) }, (object?)null, "hello");

        Assert.Equal("hello", result);
    }

    [Fact]
    public void CallGenericMethod_Type_InvokesMethodWithMultipleTypeArgs()
    {
        var helper = new TestHelper();
        var result = typeof(TestHelper).CallGenericMethod(
            "Convert", new[] { typeof(int), typeof(double) }, helper, 42);

        Assert.Equal(42.0, result);
    }

    [Fact]
    public void CallGenericMethod_Type_InvokesParameterlessGenericMethod()
    {
        var helper = new TestHelper();
        var result = typeof(TestHelper).CallGenericMethod(
            "TypeName", new[] { typeof(int) }, helper);

        Assert.Equal("Int32", result);
    }

    [Fact]
    public void CallGenericMethod_Type_FindsPrivateMethod()
    {
        var helper = new TestHelper();
        var result = typeof(TestHelper).CallGenericMethod(
            "PrivateEcho", new[] { typeof(string) }, helper, "secret");

        Assert.Equal("secret", result);
    }

    // --- Requirement 21.2: Instance-based overload delegates to Type-based ---

    [Fact]
    public void CallGenericMethod_Instance_DelegatesToTypeBased()
    {
        var helper = new TestHelper();
        var result = helper.CallGenericMethod("Echo", new[] { typeof(int) }, 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void CallGenericMethod_Instance_UsesRuntimeType()
    {
        object helper = new TestHelper();
        var result = helper.CallGenericMethod("Echo", new[] { typeof(string) }, "world");

        Assert.Equal("world", result);
    }

    // --- Requirement 21.3: InvalidOperationException when method not found ---

    [Fact]
    public void CallGenericMethod_Type_ThrowsInvalidOperationException_WhenMethodNotFound()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            typeof(TestHelper).CallGenericMethod(
                "NonExistent", new[] { typeof(int) }, (object?)null));

        Assert.Contains("NonExistent", ex.Message);
    }

    [Fact]
    public void CallGenericMethod_Type_ThrowsInvalidOperationException_WhenArityMismatch()
    {
        var helper = new TestHelper();
        // Echo<T> has 1 type param, but we pass 2
        var typeArgs = new Type[] { typeof(int), typeof(string) };
        var ex = Assert.Throws<InvalidOperationException>(() =>
            typeof(TestHelper).CallGenericMethod(
                "Echo", typeArgs, helper, 42));

        Assert.Contains("Echo", ex.Message);
    }

    [Fact]
    public void CallGenericMethod_Type_ThrowsInvalidOperationException_ForNonGenericMethod()
    {
        var helper = new TestHelper();
        var ex = Assert.Throws<InvalidOperationException>(() =>
            typeof(TestHelper).CallGenericMethod(
                "NoSuchGeneric", new[] { typeof(int) }, helper, 42));

        Assert.Contains("NoSuchGeneric", ex.Message);
    }

    // --- Requirement 21.4: TargetInvocationException unwrapping ---

    [Fact]
    public void CallGenericMethod_Type_UnwrapsTargetInvocationException()
    {
        var helper = new TestHelper();
        var ex = Assert.Throws<ArgumentException>(() =>
            typeof(TestHelper).CallGenericMethod(
                "Thrower", new[] { typeof(int) }, helper, 42));

        Assert.Equal("test error", ex.Message);
    }

    [Fact]
    public void CallGenericMethod_Instance_UnwrapsTargetInvocationException()
    {
        var helper = new TestHelper();
        var ex = Assert.Throws<ArgumentException>(() =>
            helper.CallGenericMethod("Thrower", new[] { typeof(string) }, "boom"));

        Assert.Equal("test error", ex.Message);
    }
}
