﻿namespace ScottPlotTests.UnitTests.PrimitiveTests;

internal class ExpandingAxisLimitsTests
{
    [Test]
    public void TestDefaultIsNotSet()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        limits.Left.Should().Be(double.NaN);
        limits.Right.Should().Be(double.NaN);
        limits.Bottom.Should().Be(double.NaN);
        limits.Top.Should().Be(double.NaN);

        limits.AxisLimits.Should().Be(AxisLimits.NoLimits);
    }

    [Test]
    public void TestInitAxisLimits()
    {
        AxisLimits initialLimits = new AxisLimits(-13, 17, -42, 69);
        ExpandingAxisLimits limits = new ExpandingAxisLimits(initialLimits);
        limits.AxisLimits.Should().Be(initialLimits);
    }

    [Test]
    public void TestExpandXY()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        limits.Expand(-7, 13);
        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(-7);
        limits.Bottom.Should().Be(13);
        limits.Top.Should().Be(13);

        limits.Expand(42, -69);
        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(42);
        limits.Bottom.Should().Be(-69);
        limits.Top.Should().Be(13);
    }

    [Test]
    public void TestExpandX()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        limits.ExpandX(-7);
        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(-7);

        limits.ExpandX(42);
        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(42);
    }

    [Test]
    public void TestExpandY()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        limits.ExpandY(13);
        limits.Bottom.Should().Be(13);
        limits.Top.Should().Be(13);

        limits.ExpandY(-69);
        limits.Bottom.Should().Be(-69);
        limits.Top.Should().Be(13);
    }

    [Test]
    public void TestExpandCoordinates()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        limits.Expand(new Coordinates(-7, 13));
        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(-7);
        limits.Bottom.Should().Be(13);
        limits.Top.Should().Be(13);

        limits.Expand(new Coordinates(42, -69));
        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(42);
        limits.Bottom.Should().Be(-69);
        limits.Top.Should().Be(13);
    }

    [Test]
    public void TestExpandCoordinateList()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        List<Coordinates> coordinates =
        [
            new Coordinates(-7, 13),
            new Coordinates(42, -69)
        ];

        limits.Expand(coordinates);

        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(42);
        limits.Bottom.Should().Be(-69);
        limits.Top.Should().Be(13);
    }

    [Test]
    public void TestExpandCoordinateRect()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        CoordinateRect rect = new CoordinateRect(-7, 42, -69, 13);

        limits.Expand(rect);

        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(42);
        limits.Bottom.Should().Be(-69);
        limits.Top.Should().Be(13);
    }

    [Test]
    public void TestExpandAxisLimits()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();

        AxisLimits axisLimits = new AxisLimits(-7, 42, -69, 13);

        limits.Expand(axisLimits);

        limits.Left.Should().Be(-7);
        limits.Right.Should().Be(42);
        limits.Bottom.Should().Be(-69);
        limits.Top.Should().Be(13);
    }
}
