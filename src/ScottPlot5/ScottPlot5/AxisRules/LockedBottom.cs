namespace ScottPlot.AxisRules;

public class LockedBottom(IYAxis yAxis, double yMin) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Min = yMin;
    }
}
