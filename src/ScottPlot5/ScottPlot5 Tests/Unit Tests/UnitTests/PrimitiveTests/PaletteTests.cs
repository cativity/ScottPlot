namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class PaletteTests
{
    [Test]
    public void TestGetPaletteReturnsPalettes()
    {
        IPalette[] palettes = Palette.GetPalettes();
        palettes.Should().NotBeNullOrEmpty();
        Console.WriteLine("Palettes: " + string.Join(", ", palettes.Select(static x => x.ToString())));
    }

    [Test]
    public void TestCustomPalette()
    {
        string[] customColors = ["#019d9f", "#7d3091", "#57e075", "#e5b5fa", "#009118"];
        IPalette pal = Palette.FromColors(customColors);
        pal.Colors.Length.Should().Be(customColors.Length);
    }
}
