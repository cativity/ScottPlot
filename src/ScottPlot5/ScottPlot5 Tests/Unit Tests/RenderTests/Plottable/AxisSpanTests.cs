namespace ScottPlotTests.RenderTests.Plottable;

internal class AxisSpanTests
{
    [Test]
    public void Test_AxisSpan_ExtremelyNarrow()
    {
        Plot plot = new Plot();
        const double width = 1e-10;

        for (int i = 0; i < 10; i++)
        {
            plot.Add.VerticalSpan(i, i + width);
            plot.Add.HorizontalSpan(i, i + width);
        }

        plot.HideGrid();
        plot.SaveTestImage();
    }
}
