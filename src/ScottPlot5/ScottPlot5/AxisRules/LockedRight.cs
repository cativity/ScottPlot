namespace ScottPlot.AxisRules;

public class LockedRight(IXAxis xAxis, double xMax) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Range.Max = xMax;
    }
}
