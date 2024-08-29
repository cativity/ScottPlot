namespace ScottPlotCookbook.Website;

internal class FrontPage(JsonCookbookInfo cb) : PageBase
{
    public void Generate(string outputFolder)
    {
        Sb.AppendLine("# ScottPlot 5.0 Cookbook");
        Sb.AppendLine();

        AddVersionInformation();

        // table of contents
        foreach (string chapter in cb.Chapters)
        {
            Sb.Append("<div class='mt-3 fs-4'><strong>").Append(chapter).AppendLine("</strong></div>");
            Sb.AppendLine("<ul>");

            foreach (JsonCookbookInfo.JsonCategoryInfo category in cb.Categories.Where(x => x.Chapter == chapter))
            {
                Sb.Append("<li><a href='")
                  .Append(category.Url)
                  .Append("'>")
                  .Append(category.Name)
                  .Append("</a> - ")
                  .Append(category.Description)
                  .AppendLine("</li>");
            }

            if (chapter == "Miscellaneous")
            {
                Sb.AppendLine("<li><a href='palettes'>Color Palettes</a> - Collections of colors which can be used to represent categorical data</li>");
                Sb.AppendLine("<li><a href='colormaps'>Colormaps</a> - Color gradients available to represent continuous data</li>");
            }

            Sb.AppendLine("</ul>");
        }

        // individual recipes
        foreach (string chapter in cb.Chapters)
        {
            Sb.Append("<h1>").Append(chapter).AppendLine("</h1>");

            foreach (JsonCookbookInfo.JsonCategoryInfo category in cb.Categories.Where(x => x.Chapter == chapter))
            {
                AddCategory(category);
            }
        }

        const string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        const string breadcrumbUrl1 = "/cookbook/5.0/";

        string[] fm = [$"BreadcrumbNames: [\"{breadcrumbName1}\"]", $"BreadcrumbUrls: [\"{breadcrumbUrl1}\"]"];

        Save(outputFolder, "ScottPlot 5.0 Cookbook", "Example plots shown next to the code used to create them", "index_.md", "/cookbook/5.0/", fm);
    }

    private void AddCategory(JsonCookbookInfo.JsonCategoryInfo category)
    {
        //IEnumerable<WebRecipe> recipes = RecipesByCategory[category];
        //string categoryUrl = recipes.First().CategoryUrl;

        Sb.Append("<h2 class=''><a href='").Append(category.Url).Append("' class='text-dark'>").Append(category.Name).AppendLine("</a></h2>");
        Sb.Append("<div>").Append(category.Description).AppendLine("</div>");

        foreach (JsonCookbookInfo.JsonRecipeInfo recipe in cb.Recipes.Where(x => x.Category == category.Name))
        {
            AddRecipe(recipe);
        }
    }

    private void AddRecipe(JsonCookbookInfo.JsonRecipeInfo recipe)
    {
        Sb.AppendLine("<div class='row my-4'>");
        Sb.AppendLine("<div class='col'>");
        Sb.Append("<a href='").Append(recipe.RecipeUrl).Append("'><img class='img-fluid' src='").Append(recipe.ImageUrl).AppendLine("' /></a>");
        Sb.AppendLine("</div>");
        Sb.AppendLine("<div class='col'>");
        Sb.Append("<div><a href='").Append(recipe.RecipeUrl).Append("'><b>").Append(recipe.Name).AppendLine("</b></a></div>");
        Sb.Append("<div>").Append(recipe.Description).AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
    }
}
