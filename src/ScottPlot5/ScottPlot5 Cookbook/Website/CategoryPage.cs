namespace ScottPlotCookbook.Website;

internal class CategoryPage(JsonCookbookInfo cb, JsonCookbookInfo.JsonCategoryInfo category) : PageBase
{
    public void Generate(string outputFolder)
    {
        Sb.AppendLine($"# {category.Name}");
        Sb.AppendLine();

        AddVersionInformation();

        foreach (JsonCookbookInfo.JsonRecipeInfo recipe in cb.Recipes.Where(x => x.Category == category.Name))
        {
            Sb.AppendLine();
            Sb.AppendLine($"<h2><a href='{recipe.RecipeUrl}'>{recipe.Name}</a></h2>");
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
            Sb.AppendLine("<hr class='my-5 invisible'>");
            Sb.AppendLine();
        }

        const string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        const string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = category.Name;
        string breadcrumbUrl2 = category.Url;

        string[] fm =
        [
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\"]"
        ];

        Save(outputFolder, category.Name + " - ScottPlot 5.0 Cookbook", category.Description, $"{Path.GetFileName(category.Url)}.md", category.Url, fm);
    }
}
