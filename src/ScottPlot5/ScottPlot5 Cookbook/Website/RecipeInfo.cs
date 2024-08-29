namespace ScottPlotCookbook.Website;

/// <summary>
///     This data structure contains information about a single recipe
///     and should be the only source of truth for generating filenames and URLs
/// </summary>
public readonly struct RecipeInfo(string chapter,
                                  string category,
                                  string name,
                                  string description,
                                  string source,
                                  string categoryClassName,
                                  string recipeClassName,
                                  string sourceFilePath)
{
    public string Chapter { get; } = chapter;

    public string Category { get; } = category;

    public string Name { get; } = name;

    public string Description { get; } = description;

    public string Source { get; } = source;

    public string RecipeClassName { get; } = recipeClassName;

    public string CategoryClassName { get; } = categoryClassName;

    public string SourceFilePath { get; } = sourceFilePath;

    public const string CookbookUrl = "/cookbook/5.0";

    public string CategoryUrl => $"{CookbookUrl}/{CategoryClassName}";

    public string AnchoredCategoryUrl => $"{CategoryUrl}#{RecipeClassName}";

    public string RecipeUrl => $"{CookbookUrl}/{CategoryClassName}/{RecipeClassName}";

    public string MarkdownFilename => $"{CategoryClassName}-{RecipeClassName}.md";

    public string ImageUrl => $"{CookbookUrl}/images/{RecipeClassName}.png";

    public string Sourceurl => $"https://github.com/ScottPlot/ScottPlot/blob/main/{SourceFilePath}";

    public override string ToString() => $"[{Chapter}] [{Category}] {Name}";
}
