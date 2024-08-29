using ScottPlot.Testing;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class LeftClickDragPanTests
{
    [Test]
    public void TestLeftClickDragPanPanButDoesNotZoom()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void TestLeftClickDragPanShiftLocksHorizontalAxis()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressShift();
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void TestLeftClickDragPanCtrlLocksVerticalAxis()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressCtrl();
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedLeft(50).MovedDown(50));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan occurred
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert no zoom occurred
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
