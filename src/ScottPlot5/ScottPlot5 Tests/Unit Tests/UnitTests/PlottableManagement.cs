using ScottPlot.Plottables;
using SkiaSharp;

namespace ScottPlotTests.UnitTests;

internal class PlottableManagement
{
    [Test]
    public void TestPlotRemoveInstance()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new Plot();

        // add scatter plots
        _ = myPlot.Add.Scatter(xs, ys);
        Scatter sp2 = myPlot.Add.Scatter(xs, ys);
        _ = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        Signal sig1 = myPlot.Add.Signal(ys);
        Signal sig2 = myPlot.Add.Signal(ys);
        _ = myPlot.Add.Signal(ys);

        // add duplicates
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sig2);
        myPlot.Add.Plottable(sig2);

        myPlot.PlottableList.Count.Should().Be(10);

        myPlot.Remove(sig1); // one instance
        myPlot.PlottableList.Count.Should().Be(9);

        myPlot.Remove(sp2); // 3 instances
        myPlot.PlottableList.Count.Should().Be(6);
    }

    [Test]
    public void TestPlotRemoveType()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new Plot();

        // add scatter plots
        _ = myPlot.Add.Scatter(xs, ys);
        //Scatter sp1 = myPlot.Add.Scatter(xs, ys);
        Scatter sp2 = myPlot.Add.Scatter(xs, ys);
        _ = myPlot.Add.Scatter(xs, ys);
        //Scatter sp3 = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        Signal sig1 = myPlot.Add.Signal(ys);
        Signal sig2 = myPlot.Add.Signal(ys);
        _ = myPlot.Add.Signal(ys);
        //Signal sig3 = myPlot.Add.Signal(ys);

        // add duplicates
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sig2);
        myPlot.Add.Plottable(sig2);

        myPlot.PlottableList.Count.Should().Be(10);

        myPlot.Remove(sig1.GetType());
        myPlot.PlottableList.Count.Should().Be(5);

        myPlot.Remove(typeof(Scatter));
        myPlot.PlottableList.Count.Should().Be(0);
    }

    [Test]
    public void TestPlotRemoveTypeGeneric()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new Plot();

        // add scatter plots
        _ = myPlot.Add.Scatter(xs, ys);
        //Scatter sp1 = myPlot.Add.Scatter(xs, ys);
        Scatter sp2 = myPlot.Add.Scatter(xs, ys);
        _ = myPlot.Add.Scatter(xs, ys);
        //Scatter sp3 = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        _ = myPlot.Add.Signal(ys);
        //Signal sig1 = myPlot.Add.Signal(ys);
        Signal sig2 = myPlot.Add.Signal(ys);
        _ = myPlot.Add.Signal(ys);
        //Signal sig3 = myPlot.Add.Signal(ys);

        // add duplicates
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sp2);
        myPlot.Add.Plottable(sig2);
        myPlot.Add.Plottable(sig2);

        myPlot.PlottableList.Count.Should().Be(10);

        myPlot.Remove<Signal>();
        myPlot.PlottableList.Count.Should().Be(5);

        myPlot.Remove<Scatter>();
        myPlot.PlottableList.Count.Should().Be(0);
    }

    [Test]
    public void TestPlotRemoveTypePredicate()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new Plot();

        Scatter sp1 = myPlot.Add.Scatter(xs, ys);
        sp1.LegendText = "AAA";

        Scatter sp2 = myPlot.Add.Scatter(xs, ys);
        sp2.LegendText = "ABC";

        Scatter sp3 = myPlot.Add.Scatter(xs, ys);
        sp3.LegendText = "ABAB";

        Scatter sp4 = myPlot.Add.Scatter(xs, ys);
        sp4.LegendText = "LOLOLOLOLOLOL";
        sp4.Color = Colors.Magenta;

        myPlot.PlottableList.Count.Should().Be(4);

        // match label content
        myPlot.Remove<Scatter>(static x => x.LegendText.Contains('B'));
        myPlot.PlottableList.Count.Should().Be(2);

        // match style options
        myPlot.Remove<Scatter>(static x => x.Color == Colors.Magenta);
        myPlot.PlottableList.Count.Should().Be(1);
    }

    [Test]
    public void TestPlotGetPlottables()
    {
        double[] xs = Generate.Consecutive();
        double[] ys = Generate.Sin();

        Plot myPlot = new Plot();

        // add scatter plots
        _ = myPlot.Add.Scatter(xs, ys);
        _ = myPlot.Add.Scatter(xs, ys);
        _ = myPlot.Add.Scatter(xs, ys);

        // add signal plots
        _ = myPlot.Add.Signal(ys);
        _ = myPlot.Add.Signal(ys);

        myPlot.GetPlottables().Count().Should().Be(5);
        myPlot.GetPlottables<Scatter>().Count().Should().Be(3);
        myPlot.GetPlottables<Signal>().Count().Should().Be(2);
    }

    [Test]
    public void NullTextTest()
    {
        Plot myPlot = new Plot();
        myPlot.Add.Text(null, new Coordinates());
        myPlot.RenderManager.Render(new SKCanvas(new SKBitmap()), new PixelRect());
        Assert.Pass();
    }
}
