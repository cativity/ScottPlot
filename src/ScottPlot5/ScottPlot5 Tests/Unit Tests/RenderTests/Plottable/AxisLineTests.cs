using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class AxisLineTests
{
    [Test]
    public void TestAxisLineRender()
    {
        Plot plot = new Plot();
        plot.HideGrid();

        plot.Add.VerticalLine(123.45, 1, pattern: LinePattern.Dashed);
        plot.Add.VerticalLine(123.55, 2, pattern: LinePattern.DenselyDashed);
        plot.Add.VerticalLine(123.65, 3, pattern: LinePattern.Dotted);
        plot.Add.VerticalLine(123.85, 10);

        plot.Add.HorizontalLine(123.45, 1, pattern: LinePattern.Dashed);
        plot.Add.HorizontalLine(123.55, 2, pattern: LinePattern.DenselyDashed);
        plot.Add.HorizontalLine(123.65, 3, pattern: LinePattern.Dotted);
        plot.Add.HorizontalLine(123.85, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void TestAxisLineLabel()
    {
        Plot plot = new Plot();
        plot.HideGrid();

        VerticalLine vert = plot.Add.VerticalLine(123.45);
        vert.Text = "Vertical";

        HorizontalLine horiz = plot.Add.HorizontalLine(456.78);
        horiz.Text = "Horizontal";

        plot.SaveTestImage();
    }

    [Test]
    public void TestAxisLineStyle()
    {
        Plot plot = new Plot();

        HorizontalLine hl = plot.Add.HorizontalLine(0.5);
        hl.Text = "HLine";
        hl.LabelFontSize = 10;
        hl.LabelFontColor = Colors.Yellow;

        VerticalLine vl = plot.Add.VerticalLine(0.5);
        vl.Text = "VLine";
        vl.LabelFontSize = 22;
        vl.Color = Colors.Magenta;
        vl.LineWidth = 3;
        vl.LinePattern = LinePattern.Dotted;

        plot.Axes.SetLimits(-10, 10, -10, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void TestAxisLineZeroWidth()
    {
        Plot plot = new Plot();

        HorizontalLine hl = plot.Add.HorizontalLine(0.5);
        hl.LineWidth = 0;

        plot.Axes.SetLimits(-10, 10, -10, 10);

        plot.SaveTestImage();
    }

    [Test]
    public void TestAxisLineNoLabel()
    {
        Plot plot = new Plot();
        plot.Add.HorizontalLine(0.5);
        plot.Axes.SetLimits(-10, 10, -10, 10);
        plot.SaveTestImage();
    }
}
