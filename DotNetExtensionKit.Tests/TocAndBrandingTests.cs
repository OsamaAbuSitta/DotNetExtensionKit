namespace DotNetExtensionKit.Tests;

public class TocAndBrandingTests
{
    private static readonly string RepoRoot = FindRepoRoot();

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "docfx.json")))
        {
            dir = dir.Parent;
        }

        Assert.NotNull(dir);
        return dir.FullName;
    }

    // --- TOC structure tests ---

    [Fact]
    public void RootToc_ContainsDocsAndApiEntries()
    {
        var tocContent = File.ReadAllText(Path.Combine(RepoRoot, "toc.yml"));
        Assert.Contains("Docs", tocContent);
        Assert.Contains("API", tocContent);
    }

    [Fact]
    public void DocsToc_ListsIntroductionBeforeGettingStarted()
    {
        var tocContent = File.ReadAllText(Path.Combine(RepoRoot, "docs", "toc.yml"));
        var introIndex = tocContent.IndexOf("Introduction");
        var gettingStartedIndex = tocContent.IndexOf("Getting Started");

        Assert.True(introIndex >= 0, "Introduction entry not found in docs/toc.yml");
        Assert.True(gettingStartedIndex >= 0, "Getting Started entry not found in docs/toc.yml");
        Assert.True(introIndex < gettingStartedIndex, "Introduction should appear before Getting Started");
    }

    // --- Branding asset existence tests ---

    [Fact]
    public void LogoSvg_Exists()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "images", "logo.svg")), "images/logo.svg should exist");
    }

    [Fact]
    public void FaviconIco_Exists()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "images", "favicon.ico")), "images/favicon.ico should exist");
    }

    [Fact]
    public void MainCss_Exists()
    {
        Assert.True(File.Exists(Path.Combine(RepoRoot, "styles", "main.css")), "styles/main.css should exist");
    }
}
