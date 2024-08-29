namespace ScottPlotTests;

public static class Paths
{
    public static readonly string OutputFolder = GetOutputFolder();
    public static readonly string RepoFolder = GetRepoFolder();

    private static string GetOutputFolder() => Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");

    public static string GetScottPlotXmlFilePath()
    {
        string buildFolder = Path.Combine(RepoFolder, "src/ScottPlot5/ScottPlot5/bin");
        string[] files = Directory.GetFiles(buildFolder, "ScottPlot.xml", SearchOption.AllDirectories);

        return files.Length != 0 ? files[0] : throw new FileNotFoundException("ScottPlot.xml not found in build folder");
    }

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

    public static string[] GetTtfFilePaths()
    {
        string ttfFolder = Path.Combine(RepoFolder, "src/ScottPlot5/ScottPlot5 Demos/ScottPlot5 WinForms Demo/Fonts");

        return Directory.GetFiles(ttfFolder, "*.ttf", SearchOption.AllDirectories);
    }
}
