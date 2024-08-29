using ScottPlot.Testing;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class KeyPanAndZoomTests
{
    //private const int FIGURE_WIDTH = 400;
    //private const int FIGURE_HEIGHT = 300;

    [Test]
    public void TestArrowKeyPan()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.TapUpArrow();
        plotControl.TapRightArrow();
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert no zoom
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }

    [Test]
    public void TestCtrlArrowKeyZoom()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressCtrl();
        plotControl.TapUpArrow();
        plotControl.TapRightArrow();
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert no pan
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert zoom
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }
}
