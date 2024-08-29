using System.Diagnostics.CodeAnalysis;
using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class ScatterTests
{
    [Test]
    public void TestScatterEmptyRender()
    {
        Plot plt = new Plot();
        double[] xs = [];
        double[] ys = [];
        plt.Add.Scatter(xs, ys);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void TestScatterEmptyStepDisplayRender()
    {
        Plot plt = new Plot();
        double[] xs = [];
        double[] ys = [];
        Scatter sp = plt.Add.Scatter(xs, ys);

        sp.ConnectStyle = ConnectStyle.StepHorizontal;
        plt.Should().RenderInMemoryWithoutThrowing();

        sp.ConnectStyle = ConnectStyle.StepVertical;
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void TestScatterSinglePointRender()
    {
        Plot plt = new Plot();
        double[] xs = [1];
        double[] ys = [1];
        plt.Add.Scatter(xs, ys);
        plt.Should().RenderInMemoryWithoutThrowing();
        plt.SaveTestImage();

        Assert.That(plt.Axes.GetLimits().Rect.Area, Is.Not.Zero);
    }

    [Test]
    public void TestScatterRepeatedYsRender()
    {
        Plot plt = new Plot();
        double[] xs = [1, 2, 3, 4, 5];
        double[] ys = [7, 7, 7, 7, 7];
        plt.Add.Scatter(xs, ys);
        plt.Should().RenderInMemoryWithoutThrowing();
        plt.SaveTestImage();

        Assert.That(plt.Axes.GetLimits().Rect.Area, Is.Not.Zero);
    }

    [Test]
    public void TestGetStepDisplayPixelsRight()
    {
        //Plot plt = new Plot();
        double[] xs = [1, 2, 3, 4, 5];
        double[] ys = [2, 4, 5, 8, 10];
        double[] expectedXs = [1, 2, 2, 3, 3, 4, 4, 5, 5];
        double[] expectedYs = [2, 2, 4, 4, 5, 5, 8, 8, 10];

        Pixel[] pixels = Enumerable.Range(0, 5).Select(x => new Pixel(xs[x], ys[x])).ToArray();

        Pixel[] result = Scatter.GetStepDisplayPixels(pixels, true);

        Pixel[] expectedPixels = Enumerable.Range(0, 9).Select(x => new Pixel(expectedXs[x], expectedYs[x])).ToArray();

        Assert.That(result, Is.EquivalentTo(expectedPixels));
    }

    [Test]
    public void TestGetStepDisplayPixelsLeft()
    {
        //Plot plt = new Plot();
        double[] xs = [1, 2, 3, 4, 5];
        double[] ys = [2, 4, 5, 8, 10];
        double[] expectedXs = [1, 1, 2, 2, 3, 3, 4, 4, 5];
        double[] expectedYs = [2, 4, 4, 5, 5, 8, 8, 10, 10];

        Pixel[] pixels = Enumerable.Range(0, 5).Select(x => new Pixel(xs[x], ys[x])).ToArray();

        Pixel[] result = Scatter.GetStepDisplayPixels(pixels, false);

        Pixel[] expectedPixels = Enumerable.Range(0, 9).Select(x => new Pixel(expectedXs[x], expectedYs[x])).ToArray();

        Assert.That(result, Is.EquivalentTo(expectedPixels));
    }

    [Test]
    public void TestScatterAddGenericArray()
    {
        float[] xs = [1, 2, 3, 4];
        ushort[] ys = [1, 3, 2, 4];

        Plot plt = new Plot();
        plt.Add.Scatter(xs, ys);
        plt.SaveTestImage();
    }

    [Test]
    public void TestScatterDateTimeXs()
    {
        DateTime firstDay = new DateTime(2024, 01, 01);
        DateTime[] days = Generate.ConsecutiveDays(365, firstDay);
        double[] values = Generate.RandomWalk(days.Length);

        Plot plt = new Plot();
        plt.Add.Scatter(days, values);
        plt.Axes.DateTimeTicksBottom();
        plt.SaveTestImage();
    }

    [Test]
    public void TestScatterAddGenericList()
    {
        List<float> xs = [1, 2, 3, 4];
        List<ushort> ys = [1, 3, 2, 4];

        Plot plt = new Plot();
        plt.Add.Scatter(xs, ys);
        plt.SaveTestImage();
    }

    [Test]
    public void TestScatterInvertedX()
    {
        Plot plt = new Plot();
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        plt.Add.Scatter(xs, ys);

        plt.Axes.InvertX();

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterInvertedY()
    {
        Plot plt = new Plot();
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        plt.Add.Scatter(xs, ys);

        plt.Axes.InvertY();

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterMinRenderIndex()
    {
        Plot plt = new Plot();
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        Scatter sp = plt.Add.Scatter(xs, ys);

        sp.Data.MinRenderIndex = 20;

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterMaxRenderIndex()
    {
        Plot plt = new Plot();
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        Scatter sp = plt.Add.Scatter(xs, ys);

        sp.Data.MaxRenderIndex = 30;

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterMinAndMaxRenderIndexCoordinatesList()
    {
        Plot plt = new Plot();
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        List<Coordinates> cs = Enumerable.Range(0, xs.Length).Select(i => new Coordinates(xs[i], ys[i])).ToList();

        Scatter sp = plt.Add.Scatter(cs);

        sp.Data.MinRenderIndex = 20;
        sp.Data.MaxRenderIndex = 30;

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterMinAndMaxRenderIndexCoordinatesArray()
    {
        Plot plt = new Plot();
        double[] xs = Generate.Consecutive(51);
        double[] ys = Generate.Sin(51);
        Coordinates[] cs = Enumerable.Range(0, xs.Length).Select(i => new Coordinates(xs[i], ys[i])).ToArray();

        Scatter sp = plt.Add.Scatter(cs);

        sp.Data.MinRenderIndex = 20;
        sp.Data.MaxRenderIndex = 30;

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterMinAndMaxRenderIndexGenericArray()
    {
        Plot plt = new Plot();
        int[] xs = Generate.Consecutive(51).Select(static x => (int)x).ToArray();
        float[] ys = Generate.Sin(51).Select(static x => (float)x).ToArray();
        Scatter sp = plt.Add.Scatter(xs, ys);

        sp.Data.MinRenderIndex = 20;
        sp.Data.MaxRenderIndex = 30;

        plt.Should().SavePngWithoutThrowing();
    }

    [Test]
    public void TestScatterMinAndMaxRenderIndexGenericList()
    {
        Plot plt = new Plot();
        List<int> xs = Generate.Consecutive(51).Select(static x => (int)x).ToList();
        List<float> ys = Generate.Sin(51).Select(static x => (float)x).ToList();
        Scatter sp = plt.Add.Scatter(xs, ys);

        sp.Data.MinRenderIndex = 20;
        sp.Data.MaxRenderIndex = 30;

        plt.Should().SavePngWithoutThrowing();
    }
}
