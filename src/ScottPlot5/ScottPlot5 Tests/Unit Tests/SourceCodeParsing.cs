using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ScottPlotTests;

internal static class SourceCodeParsing
{
    public static readonly string RepoFolder = GetRepoFolder();
    public static readonly string SourceFolder = Path.Combine(GetRepoFolder(), "src/ScottPlot5/ScottPlot5");
    //private static readonly char[] _methodNameDelimiters = ['(', '<'];

    private static string GetRepoFolder()
    {
        string defaultFolder = Path.GetFullPath(TestContext.CurrentContext.TestDirectory);
        string? repoFolder = defaultFolder;

        while (repoFolder is not null)
        {
            if (File.Exists(Path.Combine(repoFolder, "LICENSE")))
            {
                return repoFolder;
            }

            repoFolder = Path.GetDirectoryName(repoFolder);
        }

        throw new InvalidOperationException($"repository folder not found in any folder above {defaultFolder}");
    }

    [Test]
    public static void TestRepoFolderIsAccurate()
    {
        Directory.Exists(RepoFolder).Should().BeTrue();

        string licensePath = Path.Combine(RepoFolder, "LICENSE");
        File.Exists(licensePath).Should().BeTrue();

        Directory.Exists(SourceFolder).Should().BeTrue();
        string csFilePath = Path.Combine(SourceFolder, "PlottableAdder.cs");

        File.Exists(csFilePath).Should().BeTrue();
    }

    private static string ReadSourceFile(string path)
    {
        return File.ReadAllText(Path.Combine(SourceFolder, path));
    }

    public static List<string> GetMethodNames(string path)
    {
        string[] lines = ReadSourceFile(path).Replace('\r', '\n')
                                             .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return MethodNamesLocal().ToList();

        IEnumerable<string> MethodNamesLocal()
        {
            foreach (string line in lines)
            {
                if (TryParseMethodName(line, out string? methodName))
                {
                    yield return methodName;
                }
            }
        }

        static bool TryParseMethodName(string line, [NotNullWhen(true)] out string? methodName)
        {
            methodName = null;

            if (!line.StartsWith("public ") || line.Contains("class"))
            {
                return false;
            }

            int parenIndex = line.IndexOf('(');

            if (parenIndex < 0 || (line.Contains('{') && line.IndexOf('{') < parenIndex))
            {
                return false;
            }

            if (line.Contains('<') && line.IndexOf('<') < parenIndex)
            {
                line = Regex.Replace(line, "<[^<>]*>", "");
            }

            Debug.Assert(!line.Contains(">("));

            while (line.Contains(" ("))
            {
                line = line.Replace(" (", "(");
            }

            string[] words = line.Split(' ', 5, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
            {
                return false;
            }

            foreach (string word in words.Skip(2))
            {
                int parenPos = word.IndexOf('(');

                if (parenPos > 0)
                {
                    methodName = word[..parenPos];

                    return true;
                }
            }

            return false;
        }
    }

    // TODO: cache source file paths and their contents for quicker searching by multiple tests
    public static string[] GetSourceFilePaths() => Directory.GetFiles(SourceFolder, "*.cs", SearchOption.AllDirectories);
}
