using DotNetExtensionKit;
using Xunit;

namespace DotNetExtensionKit.Tests
{
    public class StringExtensions_CaseConversionTests
    {
        // --- SplitByCasing ---

        [Theory]
        [InlineData("PascalCase", "Pascal Case")]
        [InlineData("camelCase", "camel Case")]
        [InlineData("XMLParser", "XML Parser")]
        [InlineData("MyPropertyName", "My Property Name")]
        [InlineData("getHTTPResponse", "get HTTP Response")]
        [InlineData("already split", "already split")]
        [InlineData("lowercase", "lowercase")]
        [InlineData("A", "A")]
        [InlineData("", "")]
        [InlineData("ABC", "ABC")]
        public void SplitByCasing_ReturnsExpected(string input, string expected)
        {
            Assert.Equal(expected, input.SplitByCasing());
        }

        // --- ToTitleCaseInvariant ---

        [Theory]
        [InlineData("hello world", "Hello World")]
        [InlineData("HELLO WORLD", "Hello World")]
        [InlineData("a", "A")]
        [InlineData("", "")]
        [InlineData("  multiple   spaces  ", "Multiple Spaces")]
        public void ToTitleCaseInvariant_ReturnsExpected(string input, string expected)
        {
            Assert.Equal(expected, input.ToTitleCaseInvariant());
        }

        // --- ToCamelCase ---

        [Theory]
        [InlineData("Hello World", "helloWorld")]
        [InlineData("my property name", "myPropertyName")]
        [InlineData("HELLO", "hello")]
        [InlineData("a", "a")]
        [InlineData("", "")]
        public void ToCamelCase_ReturnsExpected(string input, string expected)
        {
            Assert.Equal(expected, input.ToCamelCase());
        }

        // --- ToSnakeCase ---

        [Theory]
        [InlineData("MyPropertyName", "my_property_name")]
        [InlineData("XMLParser", "xml_parser")]
        [InlineData("camelCase", "camel_case")]
        [InlineData("already", "already")]
        [InlineData("A", "a")]
        [InlineData("", "")]
        [InlineData("getHTTPResponse", "get_http_response")]
        public void ToSnakeCase_ReturnsExpected(string input, string expected)
        {
            Assert.Equal(expected, input.ToSnakeCase());
        }

        // --- ToKebabCase ---

        [Theory]
        [InlineData("MyPropertyName", "my-property-name")]
        [InlineData("XMLParser", "xml-parser")]
        [InlineData("camelCase", "camel-case")]
        [InlineData("already", "already")]
        [InlineData("A", "a")]
        [InlineData("", "")]
        [InlineData("getHTTPResponse", "get-http-response")]
        public void ToKebabCase_ReturnsExpected(string input, string expected)
        {
            Assert.Equal(expected, input.ToKebabCase());
        }

        // --- ToSlug ---

        [Theory]
        [InlineData("Hello World", "hello-world")]
        [InlineData("Hello World! C# is great.", "hello-world-c-is-great")]
        [InlineData("  multiple   spaces  ", "multiple-spaces")]
        [InlineData("Already-Slugged", "already-slugged")]
        [InlineData("Special @#$ Characters!", "special-characters")]
        [InlineData("", "")]
        [InlineData("123 numbers", "123-numbers")]
        public void ToSlug_ReturnsExpected(string input, string expected)
        {
            Assert.Equal(expected, input.ToSlug());
        }
    }
}
