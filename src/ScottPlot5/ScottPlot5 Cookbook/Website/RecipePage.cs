namespace ScottPlotCookbook.Website;

internal class RecipePage(JsonCookbookInfo.JsonRecipeInfo recipe) : PageBase
{
    public void Generate(string outputFolder)
    {
        Sb.AppendLine($"# {recipe.Name}");
        Sb.AppendLine();

        AddVersionInformation();

        Sb.AppendLine();
        Sb.AppendLine(recipe.Description);
        Sb.AppendLine();
        Sb.AppendLine($"[![]({recipe.ImageUrl})]({recipe.ImageUrl})");
        Sb.AppendLine();
        Sb.AppendLine("{{< code-sp5 >}}");
        Sb.AppendLine();
        Sb.AppendLine("```cs");
        Sb.AppendLine(recipe.Source);
        Sb.AppendLine("```");
        Sb.AppendLine();
        Sb.AppendLine("{{< /code-sp5 >}}");
        Sb.AppendLine();
        Sb.AppendLine($"<a href='{recipe.SourceUrl}'>{InlineIcons.GitHubIcon()} Edit on GitHub</a>");
        Sb.AppendLine();

        const string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        const string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = recipe.Category;
        string breadcrumbUrl2 = recipe.AnchorUrl.Split("#")[0];

        string breadcrumbName3 = recipe.Name;
        string breadcrumbUrl3 = recipe.RecipeUrl;

        string[] fm =
        [
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\", \"{breadcrumbName3}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\", \"{breadcrumbUrl3}\"]"
        ];

        string recipeBaseName = Path.GetFileName(recipe.RecipeUrl);
        string? categoryBaseName = Path.GetFileName(Path.GetDirectoryName(recipe.RecipeUrl));

        Save(outputFolder, recipe.Name + " - ScottPlot 5.0 Cookbook", recipe.Description, $"{categoryBaseName}.{recipeBaseName}.md", recipe.RecipeUrl, fm);
    }
}
