using ScottPlot.Testing;

namespace ScottPlotTests.InteractivityTests.UserInputActionTests;

internal class DoubleClickBenchmarkTests
{
    [Test]
    public void TestDoubleClickShowsBenchmark()
    {
        MockPlotControl plotControl = new MockPlotControl();

        plotControl.Plot.Benchmark.IsVisible.Should().BeFalse();

        plotControl.LeftClick(plotControl.Center);
        plotControl.LeftClick(plotControl.Center);
        plotControl.Plot.Benchmark.IsVisible.Should().BeTrue();

        plotControl.LeftClick(plotControl.Center);
        plotControl.LeftClick(plotControl.Center);
        plotControl.Plot.Benchmark.IsVisible.Should().BeFalse();
    }
}
