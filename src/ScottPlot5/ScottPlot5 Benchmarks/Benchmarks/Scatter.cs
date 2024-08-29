using BenchmarkDotNet.Attributes;
using ScottPlot;
using SkiaSharp;

namespace ScottPlotBench.Benchmarks;

public class Scatter
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
        double[] xs = gen.RandomSample(Points);
        double[] ys = gen.RandomSample(Points);

        _plot.Add.ScatterLine(xs, ys);
    }

    [Benchmark]
    public void ScatterLines()
    {
        _plot.Render(_surface);
    }
}
