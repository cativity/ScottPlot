using System.Globalization;

namespace ScottPlotTests.RenderTests;

internal class InternationalTests
{
    [Test]
    public void Test_Tick_DefaultCulture()
    {
        Plot plot = new Plot();
        plot.Add.Signal(Generate.Sin(100, 500_000));
        plot.Should().RenderInMemoryWithoutThrowing();
        List<string> tickLabels = plot.Axes.Left.TickGenerator?.Ticks.Select(static x => x.Label).Where(static x => !string.IsNullOrEmpty(x)).ToList() ?? [];
        Console.WriteLine(string.Join("\n", tickLabels));
        tickLabels.Should().Contain("-200,000");
    }

    [Test]
    public void Test_Tick_OtherCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

        Plot plot = new Plot();
        plot.Add.Signal(Generate.Sin(100, 500_000));
        plot.Should().RenderInMemoryWithoutThrowing();
        List<string> tickLabels = plot.Axes.Left.TickGenerator?.Ticks.Select(static x => x.Label).Where(static x => !string.IsNullOrEmpty(x)).ToList() ?? [];
        Console.WriteLine(string.Join("\n", tickLabels));
        tickLabels.Should().Contain("-200 000");
    }
}
