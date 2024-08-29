using ScottPlot.NamedColors;

namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class ColorTests
{
    [Test]
    public void TestColorConstructor()
    {
        Color color = new Color(13, 17, 23);
        color.Red.Should().Be(13);
        color.Green.Should().Be(17);
        color.Blue.Should().Be(23);
        color.Alpha.Should().Be(255);
    }

    [Test]
    public void TestColorConstructorWithAlpha()
    {
        Color color = new Color(13, 17, 23, 27);
        color.Red.Should().Be(13);
        color.Green.Should().Be(17);
        color.Blue.Should().Be(23);
        color.Alpha.Should().Be(27);
    }

    [Test]
    public void TestColorToARGB()
    {
        // MediumVioletRed 0xFFC71585 is RGB (199, 21, 133)
        Color color = new Color(199, 21, 133);
        color.ARGB.Should().Be(0xFFC71585);
    }

    [Test]
    public void TestColorFromARGB()
    {
        // MediumVioletRed 0xFFC71585 is RGB (199, 21, 133)
        Color color = Color.FromARGB(0xFFC71585);
        color.Red.Should().Be(199);
        color.Green.Should().Be(21);
        color.Blue.Should().Be(133);
        color.Alpha.Should().Be(255);
    }

    [Test]
    public void TestColorWithRed()
    {
        Color color = new Color(12, 34, 56, 78).WithRed(99);
        color.Red.Should().Be(99);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void TestColorWithGreen()
    {
        Color color = new Color(12, 34, 56, 78).WithGreen(99);
        color.Red.Should().Be(12);
        color.Green.Should().Be(99);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void TestColorWithBlue()
    {
        Color color = new Color(12, 34, 56, 78).WithBlue(99);
        color.Red.Should().Be(12);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(99);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void TestColorWithAlpha()
    {
        Color color = new Color(12, 34, 56, 78).WithAlpha(99);
        color.Red.Should().Be(12);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(99);
    }

    [Test]
    public void TestColorToSKColor()
    {
        SKColor color = new Color(12, 34, 56, 78).ToSKColor();
        color.Red.Should().Be(12);
        color.Green.Should().Be(34);
        color.Blue.Should().Be(56);
        color.Alpha.Should().Be(78);
    }

    [Test]
    public void TestColorToHex()
    {
        Color color = new Color(12, 34, 56);
        color.ToStringRGB().Should().Be("#0C2238");
    }

    [Test]
    public void TestColorsColorValues()
    {
        Colors.Orange.ToStringRGB().Should().Be("#FFA500");
        Colors.Chocolate.ToStringRGB().Should().Be("#D2691E");
        Colors.GoldenRod.ToStringRGB().Should().Be("#DAA520");

        XkcdColors.Orange.ToStringRGB().Should().Be("#F97306");
        XkcdColors.Darkblue.ToStringRGB().Should().Be("#030764");
        XkcdColors.BabyPoopGreen.ToStringRGB().Should().Be("#8F9805");
    }

    [Test]
    public void TestColorsWebColorsHasColors()
    {
        new WebColors().GetAllColors().Should().NotBeEmpty();
    }

    [Test]
    public void TestColorsWebColorsColorValues()
    {
        WebColors.Orange.ToStringRGB().Should().Be("#FFA500");
        WebColors.Chocolate.ToStringRGB().Should().Be("#D2691E");
        WebColors.GoldenRod.ToStringRGB().Should().Be("#DAA520");
    }

    [Test]
    public void TestColorsXkcdHasColors()
    {
        new XkcdColors().GetAllColors().Should().NotBeEmpty();
    }

    [Test]
    public void TestColorsXkcdColorValues()
    {
        XkcdColors.Orange.ToStringRGB().Should().Be("#F97306");
        XkcdColors.Darkblue.ToStringRGB().Should().Be("#030764");
        XkcdColors.BabyPoopGreen.ToStringRGB().Should().Be("#8F9805");
    }

    [Test]
    public void TestColorsRandomHue()
    {
        Color[] colors = Colors.RandomHue(10);

        int[] reds = colors.Select(static x => (int)x.R).ToArray();
        reds.Average().Should().BeGreaterThan(0);
        reds.Average().Should().BeLessThan(255);

        int[] greens = colors.Select(static x => (int)x.G).ToArray();
        greens.Average().Should().BeGreaterThan(0);
        greens.Average().Should().BeLessThan(255);

        int[] blues = colors.Select(static x => (int)x.B).ToArray();
        blues.Average().Should().BeGreaterThan(0);
        blues.Average().Should().BeLessThan(255);

        int[] alphas = colors.Select(static x => (int)x.A).ToArray();
        alphas.Average().Should().Be(255);
    }

    [Test]
    public void TestColorsRainbow()
    {
        Color[] colors = Colors.RandomHue(10);
        colors.Select(static x => x.ToHex()).Should().OnlyHaveUniqueItems();
    }
}
