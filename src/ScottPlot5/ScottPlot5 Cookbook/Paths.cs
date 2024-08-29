namespace ScottPlotCookbook;

internal static class Paths
{
    public static string RepoFolder { get; } = GetRepoFolder();

    public static string OutputFolder { get; } = Path.Combine(GetRepoFolder(), "dev/www/cookbook/5.0");

    public static string OutputImageFolder { get; } = Path.Combine(GetRepoFolder(), "dev/www/cookbook/5.0/images");

    public static string RecipeSourceFolder { get; } = Path.Combine(GetRepoFolder(), "src/ScottPlot5/ScottPlot5 Cookbook/Recipes");

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

    public static string GetScottPlotXmlFilePath()
    {
        string buildFolder = Path.Combine(RepoFolder, "src/ScottPlot5/ScottPlot5/bin");
        string[] files = Directory.GetFiles(buildFolder, "ScottPlot.xml", SearchOption.AllDirectories);

        return files.Length != 0 ? files[0] : throw new FileNotFoundException("ScottPlot.xml not found in build folder");
    }
}
