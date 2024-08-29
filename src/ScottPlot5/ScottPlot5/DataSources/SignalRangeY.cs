namespace ScottPlot.DataSources;

public readonly struct SignalRangeY(double min, double max)
{
    public double Min { get; } = min;

    public double Max { get; } = max;

    public override string ToString() => $"Range [{Min}, {Max}]";
}
