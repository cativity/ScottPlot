namespace ScottPlot.AxisRules;

public class LockedTop(IYAxis yAxis, double yMax) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Max = yMax;
    }
}
