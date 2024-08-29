using BenchmarkDotNet.Attributes;
using ScottPlot;
using SkiaSharp;

namespace ScottPlotBench.Benchmarks;

public class DataLogger
{
    [Params(100, 1_000, 10_000, 100_000)]
    public int Points { get; set; }

    private Plot _plot;

    private SKSurface _surface;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _plot = new Plot();
        _surface = Drawing.CreateSurface(400, 600);

        RandomDataGenerator gen = new RandomDataGenerator(0);
        double[] ys = gen.RandomSample(Points);

        ScottPlot.Plottables.DataLogger dl = _plot.Add.DataLogger();
        dl.Add(ys);
    }

    [Benchmark]
    public void DataLoggerRender()
    {
        _plot.Render(_surface);
    }
}
