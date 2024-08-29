using ScottPlot.DataSources;
using ScottPlot.Plottables;

namespace ScottPlotTests.RenderTests.Plottable;

internal class SignalTests
{
    [Test]
    public void TestSignalHorizontalLine()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2933
        // https://github.com/ScottPlot/ScottPlot/pull/2935

        double[] data = new double[10_000];
        double[] sin = Generate.Sin(data.Length / 10);
        Array.Copy(sin, 0, data, 0, sin.Length);
        Array.Copy(sin, 0, data, data.Length - sin.Length, sin.Length);

        Plot plt = new Plot();
        plt.Add.Signal(data);
        plt.HideGrid();
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalVerticalGap()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2933
        // https://github.com/ScottPlot/ScottPlot/pull/2935
        // https://github.com/ScottPlot/ScottPlot/issues/2949

        double[] data = Generate.SquareWave(low: 10, high: 15);
        Generate.AddNoiseInPlace(data, .001);

        Plot plt = new Plot();
        plt.Add.Signal(data);
        plt.HideGrid();
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalOffsets()
    {
        Plot plt = new Plot();
        Signal sig = plt.Add.Signal(Generate.Sin());
        sig.Data.XOffset = 100;
        sig.Data.YOffset = 10;
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalScale()
    {
        Plot plt = new Plot();

        Signal sig = plt.Add.Signal(Generate.Sin());
        sig.Data.YScale = 100;

        Signal sig2 = plt.Add.Signal(Generate.Sin(51));
        sig2.Data.YScale = 100;
        sig2.Data.XOffset = 10;
        sig2.Data.YOffset = 50;

        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalXYScale()
    {
        Plot plt = new Plot();

        SignalXY sig = plt.Add.SignalXY(Generate.Consecutive(51), Generate.Sin(51));
        sig.Data.YScale = 100;

        SignalXY sig2 = plt.Add.SignalXY(Generate.Consecutive(51), Generate.Sin(51));
        sig2.Data.YScale = 100;
        sig2.Data.XOffset = 10;
        sig2.Data.YOffset = 50;

        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalGenericType()
    {
        ushort[] values = [1, 3, 2, 4];
        const double period = 1.0;
        SignalSourceGenericArray<ushort> source = new SignalSourceGenericArray<ushort>(values, period);

        Plot plt = new Plot();
        plt.Add.Signal(source);
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalAddGenericArray()
    {
        ushort[] values = [1, 3, 2, 4];

        Plot plt = new Plot();
        plt.Add.Signal(values);
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalAddGenericList()
    {
        List<ushort> values = [1, 3, 2, 4];

        Plot plt = new Plot();
        plt.Add.Signal(values);
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalLowDensityInvertedX()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin());
        plt.Axes.InvertX();
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalHighDensityInvertedX()
    {
        Plot plt = new Plot();
        plt.Add.Signal(Generate.Sin(100_000));
        plt.Axes.InvertX();
        plt.SaveTestImage();
    }

    [Test]
    public void TestSignalEmptyDoubleArray()
    {
        double[] values = [];

        Plot plt = new Plot();
        plt.Add.Signal(values);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void TestSignalEmptyGenericArray()
    {
        int[] values = [];

        Plot plt = new Plot();
        plt.Add.Signal(values);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void TestSignalEmptyGenericList()
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        List<double> values = [];

        Plot plt = new Plot();
        plt.Add.Signal(values);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void TestSignalXYSinglePointOffScreen()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3926

        Plot plt = new Plot();

        // single plot with single point
        for (int i = 1; i <= 4; i++)
        {
            double[] xs = Generate.Consecutive(i);
            double[] ys = Generate.Sin(i);
            plt.Add.SignalXY(xs, ys);

            // signal plot is outside the data area
            plt.Axes.SetLimits(1, 2, 1, 2);

            plt.Should().RenderInMemoryWithoutThrowing();
        }
    }

    [Test]
    public void TestSignalSinglePointOffScreen()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3926

        Plot plt = new Plot();

        // single plot with single point
        double[] ys = [0];
        plt.Add.Signal(ys);

        // signal plot is outside the data area
        plt.Axes.SetLimits(1, 2, 1, 2);

        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_Signal_ReadOnlyList()
    {
        // https://github.com/ScottPlot/ScottPlot/pull/3978

        Plot plt = new Plot();

        // single plot with single point
        IReadOnlyList<float> ys = [1, 4, 9];
        plt.Add.Signal(ys);

        // signal plot is outside the data area
        plt.Axes.SetLimits(1, 2, 1, 2);

        plt.Should().RenderInMemoryWithoutThrowing();
    }
}
