using ScottPlot.DataSources;
using ScottPlot.Plottables;

namespace ScottPlotTests.UnitTests;

internal class ScatterDataTests
{
    [Test]
    public void TestScatterLimits()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        IScatterSource source = new ScatterSourceDoubleArray(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().BeApproximately(-1, .1);
        limits.Top.Should().BeApproximately(1, .1);
    }

    [Test]
    public void TestScatterLimitsWithNoRealPoint()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[22] = 5; // single real X
        ys[33] = 7; // single real Y
        // but no real X,Y point

        IScatterSource source = new ScatterSourceDoubleArray(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_DoubleArray()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[44] = 5;
        ys[44] = 7;

        IScatterSource source = new ScatterSourceDoubleArray(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesArray()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[44] = 5;
        ys[44] = 7;

        Coordinates[] cs = Enumerable.Range(0, xs.Length).Select(x => new Coordinates(xs[x], ys[x])).ToArray();

        IScatterSource source = new ScatterSourceCoordinatesArray(cs);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesList()
    {
        double[] xs = Generate.NaN(51);
        double[] ys = Generate.NaN(51);

        xs[44] = 5;
        ys[44] = 7;

        List<Coordinates> cs = Enumerable.Range(0, xs.Length).Select(x => new Coordinates(xs[x], ys[x])).ToList();

        IScatterSource source = new ScatterSourceCoordinatesList(cs);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesGenericArray()
    {
        float[] xs = Enumerable.Range(0, 51).Select(static _ => float.NaN).ToArray();
        float[] ys = Enumerable.Range(0, 51).Select(static _ => float.NaN).ToArray();

        xs[44] = 5;
        ys[44] = 7;

        IScatterSource source = new ScatterSourceGenericArray<float, float>(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOnePoint_CoordinatesGenericList()
    {
        List<float> xs = Enumerable.Range(0, 51).Select(static _ => float.NaN).ToList();
        List<float> ys = Enumerable.Range(0, 51).Select(static _ => float.NaN).ToList();

        xs[44] = 5;
        ys[44] = 7;

        IScatterSource source = new ScatterSourceGenericList<float, float>(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(5);
        limits.Right.Should().Be(5);
        limits.Bottom.Should().Be(7);
        limits.Top.Should().Be(7);
    }

    [Test]
    public void Test_ScatterLimits_WithOneMissingPoint()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        xs[44] = double.NaN;
        ys[44] = double.NaN;

        IScatterSource source = new ScatterSourceDoubleArray(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().BeApproximately(-1, .1);
        limits.Top.Should().BeApproximately(1, .1);
    }

    [Test]
    public void TestScatterLimitsMissingLeft()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        for (int i = 0; i < 25; i++)
        {
            xs[i] = double.NaN;
            ys[i] = double.NaN;
        }

        IScatterSource source = new ScatterSourceDoubleArray(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(25);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().BeApproximately(-1, .1);
        limits.Top.Should().BeApproximately(0, .1);
    }

    [Test]
    public void TestScatterLimitsMissingRight()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        for (int i = 26; i < ys.Length; i++)
        {
            xs[i] = double.NaN;
            ys[i] = double.NaN;
        }

        IScatterSource source = new ScatterSourceDoubleArray(xs, ys);
        AxisLimits limits = source.GetLimits();

        limits.Left.Should().Be(0);
        limits.Right.Should().Be(25);
        limits.Bottom.Should().BeApproximately(0, .1);
        limits.Top.Should().BeApproximately(1, .1);
    }

    [Test]
    public void TestScatterGetNearestCoordinatesArray()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        Coordinates[] cs = Enumerable.Range(0, xs.Length).Select(x => new Coordinates(xs[x], ys[x])).ToArray();

        Plot plot = new Plot();
        Scatter spDoubleArray = plot.Add.Scatter(cs);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        RenderDetails renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new Coordinates(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void TestScatterGetNearestCoordinatesList()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        List<Coordinates> cs = Enumerable.Range(0, xs.Length).Select(x => new Coordinates(xs[x], ys[x])).ToList();

        Plot plot = new Plot();
        Scatter spDoubleArray = plot.Add.Scatter(cs);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        RenderDetails renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new Coordinates(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void TestScatterGetNearestDoubleArray()
    {
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);

        Plot plot = new Plot();
        Scatter spDoubleArray = plot.Add.Scatter(xs, ys);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        RenderDetails renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new Coordinates(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void TestScatterGetNearestGenericArray()
    {
        float[] xs = Generate.Consecutive(51).Select(static x => (float)x).ToArray();
        float[] ys = Generate.Sin(51).Select(static x => (float)x).ToArray();

        Plot plot = new Plot();
        Scatter spDoubleArray = plot.Add.Scatter(xs, ys);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        RenderDetails renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new Coordinates(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }

    [Test]
    public void TestScatterGetNearestGenericList()
    {
        List<float> xs = Generate.Consecutive(51).Select(static x => (float)x).ToList();
        List<float> ys = Generate.Sin(51).Select(static x => (float)x).ToList();

        Plot plot = new Plot();
        Scatter spDoubleArray = plot.Add.Scatter(xs, ys);

        // force a render so we can get dimension info
        plot.GetImage(600, 400);
        RenderDetails renderInfo = plot.RenderManager.LastRender;

        Coordinates location = new Coordinates(25, 0.8);
        DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, 100);
        nearest.Index.Should().Be(20);
        nearest.X.Should().Be(20);
        nearest.Y.Should().BeApproximately(0.58778, .001);
    }
}
