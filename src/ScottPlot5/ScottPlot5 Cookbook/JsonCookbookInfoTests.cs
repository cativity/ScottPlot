using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

internal class JsonCookbookInfoTests
{
    [Test]
    public void Test_JsonCookbook_Loads()
    {
        string json = JsonFile.Generate();

        JsonCookbookInfo cb = new JsonCookbookInfo(json);
        cb.Version.Should().NotBeNullOrWhiteSpace();

        cb.Chapters.Should().NotBeEmpty();
        cb.Chapters.Should().OnlyHaveUniqueItems();

        cb.Categories.Should().NotBeEmpty();
        cb.Categories.Select(static x => x.Name).Should().OnlyHaveUniqueItems();
        cb.Categories.Select(static x => x.Description).Should().OnlyHaveUniqueItems();
        cb.Categories.Select(static x => x.Url).Should().OnlyHaveUniqueItems();

        cb.Recipes.Should().NotBeEmpty();
        cb.Recipes.Select(static x => x.RecipeUrl).Should().OnlyHaveUniqueItems();
    }
}
