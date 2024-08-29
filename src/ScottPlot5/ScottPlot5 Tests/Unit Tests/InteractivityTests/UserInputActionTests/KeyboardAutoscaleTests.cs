using ScottPlot.Interactivity;
using ScottPlot.Testing;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class KeyboardAutoscaleTests
{
    //private const int FIGURE_WIDTH = 400;
    //private const int FIGURE_HEIGHT = 300;

    //private Pixel FIGURE_CENTER => new Pixel(FIGURE_WIDTH / 2, FIGURE_HEIGHT / 2);

    [Test]
    public void TestKeyboardAutoscaleResetsAxisLimits()
    {
        MockPlotControl plotControl = new MockPlotControl();
        plotControl.Plot.Add.Signal(Generate.Sin());
        plotControl.Plot.Add.Signal(Generate.Cos());

        // start out autoscaled
        plotControl.Plot.Axes.AutoScale();
        AxisLimits originalLimits = plotControl.Plot.Axes.GetLimits();

        // slide the plot
        plotControl.LeftClickDrag(plotControl.Center, plotControl.Center.MovedRight(50).MovedUp(50));
        plotControl.Plot.Axes.GetLimits().Center.Should().NotBe(originalLimits.Center);

        // keyboard AutoAxis
        plotControl.PressKey(StandardKeys.A);
        plotControl.Plot.Axes.GetLimits().Center.Should().Be(originalLimits.Center);
    }
}
