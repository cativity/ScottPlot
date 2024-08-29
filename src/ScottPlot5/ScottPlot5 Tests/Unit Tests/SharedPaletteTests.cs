namespace ScottPlotTests;

internal class SharedPaletteTests
{
    [Test]
    public void Test_SharedPalette_GetPalettes()
    {
        Palette.GetPalettes().Should().NotBeNullOrEmpty();
    }

    [Test]
    public void Test_PalleteTitle_ShouldBePopulated()
    {
        foreach (IPalette palette in Palette.GetPalettes().Where(static palette => string.IsNullOrEmpty(palette.Name)))
        {
            throw new InvalidOperationException($"Palette has invalid title: {palette}");
        }
    }

    [Test]
    public void Test_Palletes_ShouldHaveUniqueTitles()
    {
        HashSet<string> titles = [];

        foreach (IPalette palette in Palette.GetPalettes().Where(palette => !titles.Add(palette.Name)))
        {
            throw new InvalidOperationException($"duplicate Palette title: {palette.Name}");
        }
    }
}
