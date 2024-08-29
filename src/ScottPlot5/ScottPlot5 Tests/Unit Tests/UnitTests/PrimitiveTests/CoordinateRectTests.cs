namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class CoordinateRectTests
{
    [Test]
    public void TestCoordinateRectDefaults()
    {
        CoordinateRect cRect = new CoordinateRect();
        cRect.Left.Should().Be(0);
        cRect.Right.Should().Be(0);
        cRect.Bottom.Should().Be(0);
        cRect.Top.Should().Be(0);
        cRect.HasArea.Should().BeFalse();
    }

    [Test]
    public void TestCoordinateRectConstructor()
    {
        CoordinateRect cRect = new CoordinateRect(-3, 7, -13, 11);
        cRect.Left.Should().Be(-3);
        cRect.Right.Should().Be(7);
        cRect.Bottom.Should().Be(-13);
        cRect.Top.Should().Be(11);
        cRect.Width.Should().Be(10);
        cRect.Height.Should().Be(24);
        cRect.HasArea.Should().BeTrue();
    }

    [Test]
    public void TestCoordinateRectExpanded()
    {
        Coordinates coord = new Coordinates(13, 42);

        CoordinateRect cRect = new CoordinateRect(-10, 10, -10, 10);
        cRect.Contains(coord).Should().BeFalse();

        CoordinateRect cRect2 = cRect.Expanded(coord);
        cRect2.Contains(coord).Should().BeTrue();
    }
}
