using System.Text.RegularExpressions;
using FsCheck;
using FsCheck.Fluent;
using Property = FsCheck.Property;

namespace DotNetExtensionKit.Tests;

/// <summary>
/// Property-based tests for documentation consistency.
/// Uses FsCheck to verify universal properties across all conceptual Markdown files.
/// </summary>
public class DocumentationPropertyTests
{
    private static readonly string RepoRoot = FindRepoRoot();

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "docfx.json")))
        {
            dir = dir.Parent;
        }

        if (dir is null)
            throw new InvalidOperationException("Could not find repo root (no docfx.json found).");

        return dir.FullName;
    }

    /// <summary>
    /// Returns all conceptual Markdown files: all .md files in docs/ and the root index.md.
    /// </summary>
    private static string[] GetConceptualMarkdownFiles()
    {
        var files = new List<string>();

        var docsDir = Path.Combine(RepoRoot, "docs");
        if (Directory.Exists(docsDir))
        {
            files.AddRange(Directory.GetFiles(docsDir, "*.md", SearchOption.AllDirectories));
        }

        var rootIndex = Path.Combine(RepoRoot, "index.md");
        if (File.Exists(rootIndex))
        {
            files.Add(rootIndex);
        }

        return files.ToArray();
    }

    private static Arbitrary<string> ConceptualFileArbitrary()
    {
        var files = GetConceptualMarkdownFiles();
        if (files.Length == 0)
            throw new InvalidOperationException("No conceptual Markdown files found.");

        return Gen.Elements(files).ToArbitrary();
    }

    // -----------------------------------------------------------------------
    // Feature: documentation-enhancement, Property 1: Front matter consistency
    // **Validates: Requirements 4.1**
    //
    // For any Markdown file in the conceptual documentation set (all .md files
    // in docs/ and the root index.md), the file SHALL contain YAML front matter
    // with both a `uid` field and a `title` field.
    // -----------------------------------------------------------------------

    [FsCheck.Xunit.Property(MaxTest = 100)]
    public Property FrontMatter_ContainsUidAndTitle()
    {
        return Prop.ForAll(ConceptualFileArbitrary(), (string filePath) =>
        {
            var content = File.ReadAllText(filePath);
            var relativePath = Path.GetRelativePath(RepoRoot, filePath);

            var hasFrontMatterStart = content.StartsWith("---");
            var closingIndex = content.IndexOf("---", 3);
            var hasFrontMatterEnd = closingIndex > 3;

            var hasUid = false;
            var hasTitle = false;

            if (hasFrontMatterStart && hasFrontMatterEnd)
            {
                var frontMatter = content.Substring(3, closingIndex - 3);
                hasUid = Regex.IsMatch(frontMatter, @"^\s*uid\s*:", RegexOptions.Multiline);
                hasTitle = Regex.IsMatch(frontMatter, @"^\s*title\s*:", RegexOptions.Multiline);
            }

            var result = hasFrontMatterStart && hasFrontMatterEnd && hasUid && hasTitle;
            return Prop.Label(result, $"{relativePath}: must have YAML front matter with uid and title fields");
        });
    }

    // -----------------------------------------------------------------------
    // Feature: documentation-enhancement, Property 2: Library name consistency
    // **Validates: Requirements 4.2, 4.4**
    //
    // For any Markdown file in the conceptual documentation set, every occurrence
    // of the library name SHALL be "DotNetExtensionKit" — the file SHALL NOT
    // contain standalone references to "ExtensionKit" (without the "DotNet" prefix).
    // -----------------------------------------------------------------------

    [FsCheck.Xunit.Property(MaxTest = 100)]
    public Property LibraryName_NeverUsesStandaloneExtensionKit()
    {
        return Prop.ForAll(ConceptualFileArbitrary(), (string filePath) =>
        {
            var content = File.ReadAllText(filePath);
            var relativePath = Path.GetRelativePath(RepoRoot, filePath);

            var standalonePattern = new Regex(@"(?<![Dd]ot[Nn]et)ExtensionKit", RegexOptions.None);
            var matches = standalonePattern.Matches(content);

            return Prop.Label(
                matches.Count == 0,
                $"{relativePath}: found {matches.Count} standalone 'ExtensionKit' reference(s) without 'DotNet' prefix");
        });
    }

    // -----------------------------------------------------------------------
    // Feature: documentation-enhancement, Property 3: Code block language identifiers
    // **Validates: Requirements 4.3**
    //
    // For any fenced code block in any conceptual Markdown file that contains
    // C# code, the opening fence SHALL specify the `csharp` language identifier
    // (not `cs`, `c#`, or an empty identifier).
    // -----------------------------------------------------------------------

    [FsCheck.Xunit.Property(MaxTest = 100)]
    public Property CodeBlocks_CSharpBlocksUseCsharpIdentifier()
    {
        return Prop.ForAll(ConceptualFileArbitrary(), (string filePath) =>
        {
            var content = File.ReadAllText(filePath);
            var relativePath = Path.GetRelativePath(RepoRoot, filePath);
            var lines = content.Split('\n');

            var violations = new List<string>();
            var inCodeBlock = false;
            var currentLang = string.Empty;
            var blockStartLine = 0;
            var blockContent = new List<string>();

            for (var i = 0; i < lines.Length; i++)
            {
                var trimmed = lines[i].TrimEnd();

                if (!inCodeBlock && trimmed.StartsWith("```"))
                {
                    inCodeBlock = true;
                    currentLang = trimmed.Substring(3).Trim();
                    blockStartLine = i + 1;
                    blockContent.Clear();
                }
                else if (inCodeBlock && trimmed.StartsWith("```"))
                {
                    if (IsCSharpContent(blockContent))
                    {
                        if (currentLang != "csharp")
                        {
                            var langDesc = string.IsNullOrEmpty(currentLang) ? "(empty)" : $"'{currentLang}'";
                            violations.Add($"line {blockStartLine}: language is {langDesc}");
                        }
                    }

                    inCodeBlock = false;
                    blockContent.Clear();
                }
                else if (inCodeBlock)
                {
                    blockContent.Add(trimmed);
                }
            }

            return Prop.Label(
                violations.Count == 0,
                $"{relativePath}: {violations.Count} C# code block(s) without 'csharp' identifier: {string.Join("; ", violations)}");
        });
    }

    /// <summary>
    /// Heuristic to detect if a code block contains C# code based on common keywords.
    /// </summary>
    private static bool IsCSharpContent(List<string> lines)
    {
        var csharpKeywords = new[]
        {
            "using ", "var ", "class ", "namespace ", "public ",
            "private ", "protected ", "static ", "void ", "int ",
            "string ", "new ", "return ", "async ", "await "
        };
        var joined = string.Join("\n", lines);

        var matchCount = csharpKeywords.Count(kw => joined.Contains(kw));
        return matchCount >= 2;
    }
}
