using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class PieTests
{
    [Test]
    public void Test_Pie_Render()
    {
        Plot plt = new Plot();

        PieSlice[] slices =
        [
            new PieSlice(6, Colors.Red), new PieSlice(4, Colors.Blue), new PieSlice(3, Colors.Green), new PieSlice(1, Colors.DarkCyan)
        ];

        Pie pie = plt.Add.Pie(slices);
        pie.ExplodeFraction = .1;

        pie.Slices.Should().HaveCount(4);

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Pie_Legend()
    {
        Plot plt = new Plot();

        // start with all data the same size
        List<PieSlice> slices =
        [
            new PieSlice(5, Colors.Red, "Alarm"),
            new PieSlice(5, Colors.Green, "Run"),
            new PieSlice(5, Colors.Blue, "Chill")
        ];

        Pie pie = plt.Add.Pie(slices);
        pie.LineStyle.Color = Colors.Transparent;

        plt.Legend.IsVisible = true;
        plt.SaveTestImage();
    }
}
