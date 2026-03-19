using System.Text.Json;

namespace DotNetExtensionKit.Tests;

public class DocfxConfigTests
{
    private static readonly JsonDocument DocfxConfig = LoadDocfxConfig();

    private static JsonDocument LoadDocfxConfig()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "docfx.json")))
        {
            dir = dir.Parent;
        }

        Assert.NotNull(dir);
        var json = File.ReadAllText(Path.Combine(dir.FullName, "docfx.json"));
        return JsonDocument.Parse(json);
    }

    private JsonElement GetBuild() => DocfxConfig.RootElement.GetProperty("build");

    private JsonElement GetGlobalMetadata() => GetBuild().GetProperty("globalMetadata");

    // --- globalMetadata tests ---

    [Fact]
    public void GlobalMetadata_AppTitle_IsDotNetExtensionKit()
    {
        var title = GetGlobalMetadata().GetProperty("_appTitle").GetString();
        Assert.Equal("DotNetExtensionKit", title);
    }

    [Fact]
    public void GlobalMetadata_AppLogoPath_IsImagesLogoSvg()
    {
        var logoPath = GetGlobalMetadata().GetProperty("_appLogoPath").GetString();
        Assert.Equal("images/logo.svg", logoPath);
    }

    [Fact]
    public void GlobalMetadata_AppFaviconPath_IsImagesFaviconIco()
    {
        var faviconPath = GetGlobalMetadata().GetProperty("_appFaviconPath").GetString();
        Assert.Equal("images/favicon.ico", faviconPath);
    }

    [Fact]
    public void GlobalMetadata_AppFooter_ContainsDotNetExtensionKit()
    {
        var footer = GetGlobalMetadata().GetProperty("_appFooter").GetString();
        Assert.NotNull(footer);
        Assert.Contains("DotNetExtensionKit", footer);
    }

    [Fact]
    public void GlobalMetadata_AppFooter_ContainsGitHub()
    {
        var footer = GetGlobalMetadata().GetProperty("_appFooter").GetString();
        Assert.NotNull(footer);
        Assert.Contains("GitHub", footer);
    }

    // --- resource configuration tests ---

    [Fact]
    public void Resource_ContainsImagesGlob()
    {
        var resources = GetBuild().GetProperty("resource");
        var files = GetAllResourceFiles(resources);
        Assert.Contains("images/**", files);
    }

    [Fact]
    public void Resource_ContainsStylesGlob()
    {
        var resources = GetBuild().GetProperty("resource");
        var files = GetAllResourceFiles(resources);
        Assert.Contains("styles/**", files);
    }

    private static List<string> GetAllResourceFiles(JsonElement resourceArray)
    {
        var allFiles = new List<string>();
        foreach (var entry in resourceArray.EnumerateArray())
        {
            if (entry.TryGetProperty("files", out var filesArray))
            {
                foreach (var file in filesArray.EnumerateArray())
                {
                    allFiles.Add(file.GetString()!);
                }
            }
        }
        return allFiles;
    }

    // --- template configuration tests ---

    [Fact]
    public void Template_IncludesCustomParamtableTemplate()
    {
        var templates = GetBuild().GetProperty("template");
        var templateList = new List<string>();
        foreach (var t in templates.EnumerateArray())
        {
            templateList.Add(t.GetString()!);
        }
        Assert.Contains("templates/paramtable", templateList);
    }
}
