namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class CoordinateTests
{
    [Test]
    public void TestCoordinateDefaultValues()
    {
        Coordinates coord = new Coordinates();
        coord.X.Should().Be(0);
        coord.Y.Should().Be(0);
    }

    [TestCase(0, 0)]
    [TestCase(-1, -2)]
    [TestCase(-3, 4)]
    [TestCase(5, -6)]
    [TestCase(0, double.NaN)] // permitted
    [TestCase(0, double.PositiveInfinity)] // permitted
    public void TestCoordinateConstruct(double x, double y)
    {
        Coordinates coord = new Coordinates(x, y);
        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
    }

    [Test]
    public void TestCoordinateCustomToString()
    {
        new Coordinates().ToString().Should().Contain("X =");
    }
}
