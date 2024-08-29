using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

/// <summary>
///     This class contains logic to pair recipes located using reflection with the source code parsed from .cs files.
/// </summary>
public class SourceDatabase
{
    public readonly List<RecipeInfo> Recipes = [];

    private readonly Dictionary<ICategory, IEnumerable<IRecipe>> _recipesByCategory = Query.GetRecipesByCategory();

    public SourceDatabase()
    {
        foreach (string sourceFilePath in GetRecipeSourceFilePaths())
        {
            Recipes.AddRange(GetRecipeSources(sourceFilePath));
        }
    }

    public RecipeInfo? GetInfo(IRecipe recipe)
    {
        return Recipes.Find(x => x.Name == recipe.Name);
    }

    private static List<string> GetRecipeSourceFilePaths()
    {
        List<string> paths = [];

        if (!Directory.Exists(Paths.RecipeSourceFolder))
        {
            throw new DirectoryNotFoundException(Paths.RecipeSourceFolder);
        }

        paths.AddRange(Directory.GetFiles(Paths.RecipeSourceFolder, "*.cs"));

        foreach (string subFolder in Directory.GetDirectories(Paths.RecipeSourceFolder))
        {
            paths.AddRange(Directory.GetFiles(subFolder, "*.cs"));
        }

        if (paths.Count > 0)
        {
            return paths;
        }

        throw new InvalidOperationException("no source files found");
    }

    private string GetDescription(string recipeName)
    {
        foreach (IRecipe recipe in _recipesByCategory.SelectMany(kv => kv.Value.Where(recipe => recipe.Name == recipeName)))
        {
            return recipe.Description;
        }

        throw new InvalidOperationException($"unable to locate recipe named {recipeName}");
    }

    private List<RecipeInfo> GetRecipeSources(string sourceFilePath)
    {
        string[] rawLines = File.ReadAllLines(sourceFilePath);
        sourceFilePath = sourceFilePath.Replace(Paths.RepoFolder, "").Replace("\\", "/").Trim('/').Replace(" ", "%20");

        List<RecipeInfo> recipes = [];

        string recipeClassName = string.Empty;
        string categoryClassName = string.Empty;
        string chapter = string.Empty;
        string category = string.Empty;
        string recipeName = string.Empty;
        StringBuilder source = new StringBuilder();
        bool inRecipe = false;

        foreach (string line in rawLines)
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("public class") && trimmedLine.EndsWith(": ICategory"))
            {
                categoryClassName = trimmedLine.Split(" ")[2];

                continue;
            }

            if (trimmedLine.StartsWith("public class") && trimmedLine.EndsWith(": RecipeBase"))
            {
                recipeClassName = trimmedLine.Split(" ")[2];

                continue;
            }

            if (trimmedLine.StartsWith("public string Chapter =>"))
            {
                chapter = line.Split('"')[1];

                continue;
            }

            if (trimmedLine.StartsWith("public string CategoryName =>"))
            {
                category = line.Split('"')[1];

                continue;
            }

            if (trimmedLine.StartsWith("public override string Name =>"))
            {
                recipeName = line.Split('"')[1];

                continue;
            }

            // NOTE: indentation-specific identification of code blocks is okay
            // becuase the CI system runs the autoformatter automatically.

            // start of the Execute() code block
            if (line.StartsWith("        {"))
            {
                inRecipe = true;

                continue;
            }

            // end of the Execute() code block
            if (inRecipe && line.StartsWith("        }"))
            {
                //string shortVersionString = Version.VersionString.Replace(".", ", ").Split("-")[0];

                StringBuilder sb = new StringBuilder();
                //Sb.AppendLine($"ScottPlot.Version.ShouldBe({shortVersionString});");
                sb.AppendLine("ScottPlot.Plot myPlot = new();");
                sb.AppendLine();
                sb.AppendLine(source.ToString().Trim());
                sb.AppendLine();
                sb.AppendLine("myPlot.SavePng(\"demo.png\", 400, 300);");

                string description = GetDescription(recipeName);

                RecipeInfo thisRecipe = new RecipeInfo(chapter,
                                                       category,
                                                       recipeName,
                                                       description,
                                                       sb.ToString(),
                                                       categoryClassName,
                                                       recipeClassName,
                                                       sourceFilePath);

                recipes.Add(thisRecipe);

                inRecipe = false;
                source.Clear();

                continue;
            }

            if (inRecipe)
            {
                string newSourceLine = line.Trim().Length == 0
                                           ? string.Empty // preserve double linebreaks in recipe sources
                                           : line[12..]; // de-indent recipe sources

                source.AppendLine(newSourceLine);
            }
        }

        return recipes;
    }
}
