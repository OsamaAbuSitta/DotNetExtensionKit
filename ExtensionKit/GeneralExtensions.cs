using System;
using System.Linq;
using System.Reflection;

namespace DotNetExtensionKit
{
    /// <summary>
    /// Provides general-purpose utility extension methods including null checks,
    /// safe casting, conditional execution, transformations, and reflection helpers.
    /// </summary>
    public static class GeneralExtensions
    {
        /// <summary>
        /// Returns true if the value is null.
        /// </summary>
        /// <typeparam name="T">The reference type of the value.</typeparam>
        /// <param name="value">The value to check for null.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is null; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// string? name = null;
        /// bool result = name.IsNull(); // true
        /// </code>
        /// </example>
        public static bool IsNull<T>(this T? value) where T : class
            => value is null;

        /// <summary>
        /// Returns true if the value is not null.
        /// </summary>
        /// <typeparam name="T">The reference type of the value.</typeparam>
        /// <param name="value">The value to check for non-null.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is not null; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// string name = "Alice";
        /// bool result = name.IsNotNull(); // true
        /// </code>
        /// </example>
        public static bool IsNotNull<T>(this T? value) where T : class
            => value is object;

        /// <summary>
        /// Performs a safe cast, returning null if the value is not compatible with <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target reference type to cast to.</typeparam>
        /// <param name="value">The object to cast.</param>
        /// <returns>The value cast to <typeparamref name="T"/>, or <c>null</c> if the cast is not valid.</returns>
        /// <example>
        /// <code>
        /// object obj = "hello";
        /// string? text = obj.As&lt;string&gt;(); // "hello"
        /// </code>
        /// </example>
        public static T? As<T>(this object? value) where T : class
            => value as T;

        /// <summary>
        /// Returns the value when the condition is true, or default(T) otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to return when the condition is met.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>The original <paramref name="value"/> if <paramref name="condition"/> is <c>true</c>; otherwise, <c>default(T)</c>.</returns>
        /// <example>
        /// <code>
        /// int score = 100;
        /// int result = score.If(score > 50); // 100
        /// </code>
        /// </example>
#nullable disable
        public static T If<T>(this T value, bool condition)
            => condition ? value : default;
#nullable restore

        /// <summary>
        /// Returns the value when the condition is false, or default(T) otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to return when the condition is not met.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>The original <paramref name="value"/> if <paramref name="condition"/> is <c>false</c>; otherwise, <c>default(T)</c>.</returns>
        /// <example>
        /// <code>
        /// int score = 100;
        /// int result = score.IfNot(score > 200); // 100
        /// </code>
        /// </example>
#nullable disable
        public static T IfNot<T>(this T value, bool condition)
            => condition ? default : value;
#nullable restore

        /// <summary>
        /// Applies the specified function to the value and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The value to transform.</param>
        /// <param name="transformer">The function to apply to the value.</param>
        /// <returns>The result of applying <paramref name="transformer"/> to <paramref name="value"/>.</returns>
        /// <example>
        /// <code>
        /// int length = "hello".Transform(s => s.Length); // 5
        /// </code>
        /// </example>
        public static TResult Transform<T, TResult>(this T value, Func<T, TResult> transformer)
            => transformer(value);

        /// <summary>
        /// Executes the specified action on the value and returns the original value unchanged.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to pass to the action.</param>
        /// <param name="action">The action to execute on the value.</param>
        /// <returns>The original <paramref name="value"/>, unchanged.</returns>
        /// <example>
        /// <code>
        /// var list = new List&lt;int&gt; { 1, 2, 3 }
        ///     .Also(l => Console.WriteLine(l.Count)); // prints 3, returns the list
        /// </code>
        /// </example>
        public static T Also<T>(this T value, Action<T> action)
        {
            action(value);
            return value;
        }

        /// <summary>
        /// Returns true if the value is within the inclusive range [min, max].
        /// </summary>
        /// <typeparam name="T">The comparable type of the value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is between <paramref name="min"/> and <paramref name="max"/> inclusive; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// bool inRange = 5.IsBetween(1, 10); // true
        /// </code>
        /// </example>
        public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable<T>
            => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;

        /// <summary>
        /// Returns true if the value equals any element in the specified set.
        /// </summary>
        /// <typeparam name="T">The type of the value and set elements.</typeparam>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The set of values to search within.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is found in <paramref name="values"/>; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// bool found = "b".IsIn("a", "b", "c"); // true
        /// </code>
        /// </example>
        public static bool IsIn<T>(this T value, params T[] values)
            => Array.IndexOf(values, value) >= 0;

        /// <summary>
        /// Locates an open generic method by name and arity on the specified type,
        /// closes it with the provided type arguments, invokes it, and returns the result.
        /// </summary>
        /// <param name="type">The type on which to search for the generic method.</param>
        /// <param name="methodName">The name of the generic method to invoke.</param>
        /// <param name="typeArguments">The type arguments to close the generic method with.</param>
        /// <param name="instance">The object instance on which to invoke the method, or <c>null</c> for static methods.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <returns>The return value of the invoked method.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a method with the specified name and generic arity is not found on the type.</exception>
        /// <example>
        /// <code>
        /// var result = typeof(MyClass).CallGenericMethod(
        ///     "MyMethod", new[] { typeof(int) }, myInstance, 42);
        /// </code>
        /// </example>
#nullable disable
        public static object CallGenericMethod(this Type type, string methodName, Type[] typeArguments, object instance, params object[] parameters)
#nullable restore
        {
            var openMethod = type
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .FirstOrDefault(m => m.Name == methodName
                    && m.IsGenericMethodDefinition
                    && m.GetGenericArguments().Length == typeArguments.Length);

            if (openMethod == null)
            {
                throw new InvalidOperationException(
                    $"Method '{methodName}' with {typeArguments.Length} generic parameter(s) not found on type '{type.FullName}'.");
            }

            var closedMethod = openMethod.MakeGenericMethod(typeArguments);

            try
            {
                return closedMethod.Invoke(instance, parameters);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw; // unreachable, but satisfies compiler
            }
        }

        /// <summary>
        /// Locates an open generic method by name and arity on the instance's runtime type,
        /// closes it with the provided type arguments, invokes it, and returns the result.
        /// </summary>
        /// <param name="instance">The object instance on which to invoke the method.</param>
        /// <param name="methodName">The name of the generic method to invoke.</param>
        /// <param name="typeArguments">The type arguments to close the generic method with.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <returns>The return value of the invoked method.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a method with the specified name and generic arity is not found on the instance's type.</exception>
        /// <example>
        /// <code>
        /// var result = myInstance.CallGenericMethod(
        ///     "MyMethod", new[] { typeof(string) }, "hello");
        /// </code>
        /// </example>
#nullable disable
        public static object CallGenericMethod(this object instance, string methodName, Type[] typeArguments, params object[] parameters)
#nullable restore
        {
            return instance.GetType().CallGenericMethod(methodName, typeArguments, instance, parameters);
        }
    }
}
