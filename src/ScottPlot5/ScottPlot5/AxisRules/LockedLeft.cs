namespace ScottPlot.AxisRules;

public class LockedLeft(IXAxis xAxis, double xMin) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Range.Min = xMin;
    }
}
