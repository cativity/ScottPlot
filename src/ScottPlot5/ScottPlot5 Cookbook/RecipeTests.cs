using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;
using System.Reflection;
using ScottPlot.Testing;

namespace ScottPlotCookbook;

internal class RecipeTests
{
    [Test]
    public void TestQueryCategoriesHavePopulatedFields()
    {
        ICategory[] categories = Query.GetCategories().ToArray();

        categories.Should().NotBeNullOrEmpty();

        foreach (ICategory category in categories)
        {
            category.Chapter.Should().NotBeNullOrWhiteSpace();
            category.CategoryName.Should().NotBeNullOrWhiteSpace();
            category.CategoryDescription.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public void TestQueryCategoriesHaveUniqueNames()
    {
        DuplicateIdentifier<ICategory> ids = new DuplicateIdentifier<ICategory>("category name");

        foreach (ICategory category in Query.GetCategories())
        {
            ids.Add(category.CategoryName, category);
        }

        ids.ShouldHaveNoDuplicates();
    }

    [Test]
    public void TestQueryCategoriesHaveUniqueDescriptions()
    {
        DuplicateIdentifier<ICategory> ids = new DuplicateIdentifier<ICategory>("category description");

        foreach (ICategory category in Query.GetCategories())
        {
            ids.Add(category.CategoryDescription, category);
        }

        ids.ShouldHaveNoDuplicates();
    }

    [Test]
    public static void TestRecipeSourcesFoundAndValid()
    {
        SourceDatabase db = new SourceDatabase();

        db.Recipes.Should().NotBeNullOrEmpty();

        foreach (RecipeInfo recipe in db.Recipes)
        {
            recipe.Chapter.Should().NotBeNullOrEmpty();
            recipe.Category.Should().NotBeNullOrEmpty();
            recipe.Name.Should().NotBeNullOrEmpty();
            recipe.Description.Should().NotBeNullOrEmpty();
            recipe.Source.Should().NotBeNullOrEmpty();
            recipe.RecipeClassName.Should().NotBeNullOrEmpty();
            recipe.CategoryClassName.Should().NotBeNullOrEmpty();
        }

        db.Recipes.Select(static x => x.RecipeClassName).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(static x => x.Name).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(static x => x.Description).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(static x => x.ImageUrl).Should().OnlyHaveUniqueItems();
        db.Recipes.Select(static x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
    }

    [Test]
    public static void TestChaptersListHasAllChapters()
    {
        string[] orderedChapterNames = Query.GetChapterNamesInOrder();

        foreach (string chapter in Query.GetCategories().Select(static x => x.Chapter).Distinct())
        {
            orderedChapterNames.Should().Contain(chapter);
        }
    }

    [Test]
    public static void TestRecipesHaveTestAttribute()
    {
        List<Type> recipeTypes = Assembly.GetAssembly(typeof(IRecipe))?.GetTypes().Where(static x => x.IsAssignableTo(typeof(RecipeBase)) && !x.IsAbstract).ToList() ?? [];

        Assert.That(recipeTypes, Is.Not.Empty);

        foreach (Type recipeType in recipeTypes.Where(recipeType => !recipeType.Methods().Single(static x => x.Name == "Execute" && !x.IsAbstract).GetCustomAttributes<TestAttribute>(false).Any()))
        {
            Assert.Fail($"{recipeType.Name}'s Execute() method is missing the [Test] attribute");
        }
    }

    [Test]
    public static void TestRecipesArePublic()
    {
        List<Type> recipeTypes = Assembly.GetAssembly(typeof(IRecipe))?.GetTypes().Where(static x => x.IsAssignableTo(typeof(RecipeBase)) && !x.IsAbstract).ToList() ?? [];

        Assert.That(recipeTypes, Is.Not.Empty);

        foreach (TypeInfo recipeClassInfo in recipeTypes.Select(recipeType => recipeType.GetTypeInfo()))
        {
            recipeClassInfo.IsVisible.Should().BeTrue($"{recipeClassInfo.Namespace}.{recipeClassInfo.Name} should be public");
        }
    }
}
