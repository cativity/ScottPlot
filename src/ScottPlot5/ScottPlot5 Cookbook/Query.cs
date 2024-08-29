namespace ScottPlotCookbook;

public static class Query
{
    public static readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = GetRecipesByCategory();

    public readonly record struct RecipeInfo(string Chapter, ICategory Category, IRecipe Recipe);

    public static List<RecipeInfo> GetRecipes()
    {
        List<RecipeInfo> list = [];

        Dictionary<string, ICategory> categoriesByName = RecipesByCategory.ToDictionary(x => x.Key.CategoryName, x => x.Key);

        foreach (ICategory category in categoriesByName.Keys.Select(categoryName => categoriesByName[categoryName]))
        {
            list.AddRange(RecipesByCategory[category].Select(recipe => new RecipeInfo(category.Chapter, category, recipe)));
        }

        return list;
    }

    public static string[] GetChapterNamesInOrder() => ["Introduction", "Axis", "Layout", "Plot Types", "Statistics", "Miscellaneous"];

    public static IEnumerable<ICategory> GetCategories()
    {
        List<ICategory> categories = AppDomain.CurrentDomain.GetAssemblies()
                                              .SelectMany(static x => x.GetTypes())
                                              .Where(static type => type.IsClass && !type.IsAbstract && typeof(ICategory).IsAssignableFrom(type))
                                              .Select(Activator.CreateInstance)
                                              .Cast<ICategory>()
                                              .ToList();

        foreach (string name in GetChapterNamesInOrder().Reverse())
        {
            if (categories.Find(x => string.Equals(x.Chapter, name, StringComparison.InvariantCultureIgnoreCase)) is ICategory category)
            {
                categories.Remove(category);
                categories.Insert(0, category);
            }
        }

        return categories;
    }

    public static Dictionary<ICategory, IEnumerable<IRecipe>> GetRecipesByCategory()
    {
        Dictionary<ICategory, IEnumerable<IRecipe>> recipesByCategory = [];

        foreach (ICategory category in GetCategories())
        {
            recipesByCategory[category] = category.GetType()
                                                  .GetNestedTypes()
                                                  .Where(static type => typeof(IRecipe).IsAssignableFrom(type))
                                                  .Select(Activator.CreateInstance)
                                                  .Cast<IRecipe>();
        }

        return recipesByCategory;
    }
}
