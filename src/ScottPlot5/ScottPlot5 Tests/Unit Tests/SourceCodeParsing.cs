namespace ScottPlotTests;

internal static class SourceCodeParsing
{
    public static readonly string RepoFolder = GetRepoFolder();
    public static readonly string SourceFolder = Path.Combine(GetRepoFolder(), "src/ScottPlot5/ScottPlot5");

    private static string GetRepoFolder()
    {
        string defaultFolder = Path.GetFullPath(TestContext.CurrentContext.TestDirectory);
        ;
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
    public static void Test_RepoFolder_IsAccurate()
    {
        Directory.Exists(RepoFolder).Should().BeTrue();

        string licensePath = Path.Combine(RepoFolder, "LICENSE");
        File.Exists(licensePath).Should().BeTrue();

        Directory.Exists(SourceFolder).Should().BeTrue();
        string csFilePath = Path.Combine(SourceFolder, "PlottableAdder.cs");

        File.Exists(csFilePath).Should().BeTrue();
    }

    public static string ReadSourceFile(string path)
    {
        string s = Path.Combine(SourceFolder, path);

        return File.ReadAllText(s);
    }

    public static List<string> GetMethodNames(string path)
    {
        List<string> methodNames = [];

        string[] lines = ReadSourceFile(path).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (string line in lines.Where(static line => line.StartsWith("public ") && !line.Contains("class") && !line.Contains("{ get;")))
        {
            //if (!line.StartsWith("public ") || line.Contains("class") || line.Replace(" ", "").Contains("{get"))
            //if (!line.StartsWith("public ") || line.Contains("class") || line.Contains("{ get;"))
            //{
            //    continue;
            //}

            string[] parts = line.Split(" ", 3);
            //string returnType = parts[1];
            //string methodSignature = parts[2];
            methodNames.Add(parts[2].Split("(")[0].Split("<")[0]);
        }

        return methodNames;
    }

    // TODO: cache source file paths and their contents for quicker searching by multiple tests
    public static string[] GetSourceFilePaths() => Directory.GetFiles(SourceFolder, "*.cs", SearchOption.AllDirectories);
}
