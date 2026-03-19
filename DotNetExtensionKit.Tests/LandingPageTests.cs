namespace DotNetExtensionKit.Tests;

public class LandingPageTests
{
    private static readonly string IndexContent = LoadIndexMd();

    private static string LoadIndexMd()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "docfx.json")))
        {
            dir = dir.Parent;
        }

        Assert.NotNull(dir);
        return File.ReadAllText(Path.Combine(dir.FullName, "index.md"));
    }

    [Fact]
    public void IndexMd_ContainsH1Heading_DotNetExtensionKit()
    {
        Assert.Contains("# DotNetExtensionKit", IndexContent);
    }

    [Fact]
    public void IndexMd_ContainsTaglineAfterHeading()
    {
        // The tagline is the sentence immediately following the h1 heading
        var lines = IndexContent.Split('\n');
        var headingIndex = Array.FindIndex(lines, l => l.TrimEnd() == "# DotNetExtensionKit");
        Assert.True(headingIndex >= 0, "H1 heading not found");

        // Find the next non-empty line after the heading
        var taglineFound = false;
        for (var i = headingIndex + 1; i < lines.Length; i++)
        {
            var trimmed = lines[i].Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;
            // Tagline should be a prose sentence (not a markdown heading or HTML tag)
            Assert.False(trimmed.StartsWith('#'), "Expected tagline, found another heading");
            Assert.True(trimmed.Length > 20, "Tagline should be a meaningful sentence");
            taglineFound = true;
            break;
        }

        Assert.True(taglineFound, "No tagline found after the h1 heading");
    }

    [Theory]
    [InlineData("String Extensions")]
    [InlineData("DateTime Extensions")]
    [InlineData("Collection Extensions")]
    [InlineData("General Extensions")]
    public void IndexMd_ContainsFeatureCardSection(string featureTitle)
    {
        Assert.Contains(featureTitle, IndexContent);
    }

    [Fact]
    public void IndexMd_ContainsInstallSnippet()
    {
        Assert.Contains("dotnet add package DotNetExtensionKit", IndexContent);
    }

    [Fact]
    public void IndexMd_ContainsGettingStartedLink()
    {
        Assert.Contains("docs/getting-started.md", IndexContent);
    }

    [Fact]
    public void IndexMd_ContainsApiReferenceLink()
    {
        Assert.Contains("api/", IndexContent);
    }
}
