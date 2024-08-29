namespace ScottPlot.DataSources;

public class FunctionSource(Func<double, double> func) : IFunctionSource
{
    public CoordinateRange RangeX { get; set; } = CoordinateRange.Infinity;

    public Func<double, double> Function { get; set; } = func;

    public Func<CoordinateRange, CoordinateRange>? GetRangeYFunc { get; set; }

    public double Get(double x) => Function(x);

    public CoordinateRange GetRangeY(CoordinateRange xs)
    {
        return GetRangeYFunc?.Invoke(xs) ?? CoordinateRange.NotSet;
    }
}
