namespace ScottPlot.AxisRules;

public class LockedVertical(IYAxis yAxis, double yMin, double yMax) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public double YMin { get; set; } = yMin;

    public double Max { get; set; } = yMax;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Set(YMin, Max);
    }
}
