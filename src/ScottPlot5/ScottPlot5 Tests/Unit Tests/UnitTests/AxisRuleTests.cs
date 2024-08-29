using ScottPlot.AxisRules;

namespace ScottPlotTests.UnitTests;

internal class AxisRuleTests
{
    [Test]
    public void TestAxisRuleLockedBottom()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedBottom(plt.Axes.Left, -123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Bottom.Should().Be(AxisLimits.Unset.Bottom);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);
    }

    [Test]
    public void TestAxisRuleLockedCenterX()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedCenterX(plt.Axes.Bottom, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Bottom.Should().Be(AxisLimits.Unset.Bottom);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.HorizontalCenter.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.HorizontalCenter.Should().Be(123);
    }

    [Test]
    public void TestAxisRuleLockedCenterY()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedCenterY(plt.Axes.Left, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Left.Should().Be(AxisLimits.Unset.Left);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.VerticalCenter.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Rect.VerticalCenter.Should().Be(123);
    }

    [Test]
    public void TestAxisRuleLockedHorizontal()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedHorizontal(plt.Axes.Bottom, -123, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Left.Should().Be(AxisLimits.Unset.Left);
        plt.Axes.GetLimits().Right.Should().Be(AxisLimits.Unset.Right);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);
        plt.Axes.GetLimits().Right.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);
        plt.Axes.GetLimits().Right.Should().Be(123);
    }

    [Test]
    public void TestAxisRuleLockedLeft()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedLeft(plt.Axes.Bottom, -123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Left.Should().Be(AxisLimits.Unset.Left);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Left.Should().Be(-123);
    }

    [Test]
    public void TestAxisRuleLockedRight()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedRight(plt.Axes.Bottom, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Right.Should().Be(AxisLimits.Unset.Right);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Right.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Right.Should().Be(123);
    }

    [Test]
    public void TestAxisRuleLockedTop()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedTop(plt.Axes.Left, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Top.Should().Be(AxisLimits.Unset.Top);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Top.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Top.Should().Be(123);
    }

    [Test]
    public void TestAxisRuleLockedVertical()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new LockedVertical(plt.Axes.Left, -123, 123));

        // limits start out unset (+inf, -inf)
        plt.Axes.GetLimits().Bottom.Should().Be(AxisLimits.Unset.Bottom);
        plt.Axes.GetLimits().Top.Should().Be(AxisLimits.Unset.Top);

        // rules should be applied to the first render
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);
        plt.Axes.GetLimits().Top.Should().Be(123);

        // rules should persist after plot manipulation and re-rendering
        plt.Axes.Pan(new CoordinateOffset(1, 1));
        plt.Axes.ZoomIn(1, 1);
        plt.RenderInMemory();
        plt.Axes.GetLimits().Bottom.Should().Be(-123);
        plt.Axes.GetLimits().Top.Should().Be(123);
    }

    [Test]
    public void TestAxisRuleMaximumBoundary()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new MaximumBoundary(plt.Axes.Bottom, plt.Axes.Left, new AxisLimits(-123, 123, -456, 456)));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-9999, 9999, -9999, 9999);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-123);
            plt.Axes.GetLimits().Right.Should().Be(123);
            plt.Axes.GetLimits().Bottom.Should().Be(-456);
            plt.Axes.GetLimits().Top.Should().Be(456);
        }
    }

    [Test]
    public void TestAxisRuleMinimumBoundary()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new MinimumBoundary(plt.Axes.Bottom, plt.Axes.Left, new AxisLimits(-1, 1, -2, 2)));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-.5, .5, -.5, .5);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-1);
            plt.Axes.GetLimits().Right.Should().Be(1);
            plt.Axes.GetLimits().Bottom.Should().Be(-2);
            plt.Axes.GetLimits().Top.Should().Be(2);
        }
    }

    [Test]
    public void TestAxisRuleMaximumSpan()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new MaximumSpan(plt.Axes.Bottom, plt.Axes.Left, 1, 2));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-5, 5, -5, 5);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-.5);
            plt.Axes.GetLimits().Right.Should().Be(.5);
            plt.Axes.GetLimits().Bottom.Should().Be(-1);
            plt.Axes.GetLimits().Top.Should().Be(1);
        }
    }

    [Test]
    public void TestAxisRuleMinimumSpan()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new MinimumSpan(plt.Axes.Bottom, plt.Axes.Left, 1, 2));

        for (int i = 0; i < 3; i++)
        {
            plt.Axes.SetLimits(-.05, .05, -.05, .05);
            plt.RenderInMemory();
            plt.Axes.GetLimits().Left.Should().Be(-.5);
            plt.Axes.GetLimits().Right.Should().Be(.5);
            plt.Axes.GetLimits().Bottom.Should().Be(-1);
            plt.Axes.GetLimits().Top.Should().Be(1);
        }
    }

    [Test]
    public void TestAxisRuleSquarePreserveX()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new SquarePreserveX(plt.Axes.Bottom, plt.Axes.Left));

        for (int i = 0; i < 3; i++)
        {
            plt.RenderInMemory();
            plt.RenderManager.LastRender.UnitsPerPxX.Should().BeApproximately(plt.RenderManager.LastRender.UnitsPerPxY, 1e-6);
        }
    }

    [Test]
    public void TestAxisRuleSquarePreserveY()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new SquarePreserveY(plt.Axes.Bottom, plt.Axes.Left));

        for (int i = 0; i < 3; i++)
        {
            plt.RenderInMemory();
            plt.RenderManager.LastRender.UnitsPerPxX.Should().BeApproximately(plt.RenderManager.LastRender.UnitsPerPxY, 1e-6);
        }
    }

    [Test]
    public void TestAxisRuleSquareZoom()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new SquareZoomOut(plt.Axes.Bottom, plt.Axes.Left));

        // NOTE: ticks are not always stable across successive render requests!

        for (int i = 0; i < 3; i++)
        {
            plt.RenderInMemory();
            plt.RenderManager.LastRender.UnitsPerPxX.Should().BeApproximately(plt.RenderManager.LastRender.UnitsPerPxY, 1e-6);
        }
    }

    [Test]
    public void TestAxisRuleSnapTicksX()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new SnapToTicksX(plt.Axes.Bottom));

        for (int i = 0; i < 3; i++)
        {
            // WARNING: CANNOT TEST TICK SNAPPING BECAUSE TICKS ARE FONT AND SYSTEM DEPENDENT
            plt.Should().RenderInMemoryWithoutThrowing();
        }
    }

    [Test]
    public void TestAxisRuleSnapTicksY()
    {
        Plot plt = new Plot();

        plt.Add.Signal(Generate.Sin(51));

        plt.Axes.Rules.Add(new SnapToTicksY(plt.Axes.Left));

        for (int i = 0; i < 3; i++)
        {
            // WARNING: CANNOT TEST TICK SNAPPING BECAUSE TICKS ARE FONT AND SYSTEM DEPENDENT
            plt.Should().RenderInMemoryWithoutThrowing();
        }
    }
}
