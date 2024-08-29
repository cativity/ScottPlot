using ScottPlot.Testing;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class ScrollWheelZoomTests
{
    [Test]
    public void Test_ScrollWheel_UpZoomsIn()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.ScrollWheelUp(plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ScrollWheel_DownZoomsOut()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.ScrollWheelDown(plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan
        newLimits.HorizontalCenter.Should().BeLessThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeLessThan(originalLimits.VerticalCenter);

        // assert zoom-in
        newLimits.HorizontalSpan.Should().BeGreaterThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeGreaterThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ShiftScrollWheel_UpZoomsInVertically()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressShift();
        plotControl.ScrollWheelUp(plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan
        newLimits.HorizontalCenter.Should().Be(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().BeGreaterThan(originalLimits.VerticalCenter);

        // assert zoom-in
        newLimits.HorizontalSpan.Should().Be(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().BeLessThan(originalLimits.VerticalSpan);
    }

    [Test]
    public void Test_ShiftScrollWheel_UpZoomsInHorizontally()
    {
        MockPlotControl plotControl = new MockPlotControl();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        plotControl.PressCtrl();
        plotControl.ScrollWheelUp(plotControl.Center.MovedRight(100).MovedUp(100));
        AxisLimits newLimits = plotControl.Plot.Axes.GetLimits();

        // assert pan
        newLimits.HorizontalCenter.Should().BeGreaterThan(originalLimits.HorizontalCenter);
        newLimits.VerticalCenter.Should().Be(originalLimits.VerticalCenter);

        // assert zoom-in
        newLimits.HorizontalSpan.Should().BeLessThan(originalLimits.HorizontalSpan);
        newLimits.VerticalSpan.Should().Be(originalLimits.VerticalSpan);
    }
}
