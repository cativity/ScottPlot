namespace SharedTests;

internal class SharedColorTests
{
    [Test]
    public void TestRgbColorRgbConstructor()
    {
        Color color = new Color(33, 66, 99);
        color.R.Should().Be(33);
        color.G.Should().Be(66);
        color.B.Should().Be(99);
        color.A.Should().Be(255);
    }

    [Test]
    public void TestRgbColorHexConstructor()
    {
        Color color = Color.FromHex("#336699");
        color.R.Should().Be(51);
        color.G.Should().Be(102);
        color.B.Should().Be(153);
        color.A.Should().Be(255);
    }

    [Test]
    public void TestRgbColorHexConstructorWithAlpha()
    {
        Color color = Color.FromHex("#336699AA");
        color.R.Should().Be(51);
        color.G.Should().Be(102);
        color.B.Should().Be(153);
        color.A.Should().Be(170);
    }
}
