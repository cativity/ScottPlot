namespace ScottPlot.AxisRules;

public class LockedCenterX(IXAxis xAxis, double xCenter) : IAxisRule
{
    public IXAxis XAxis { get; } = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Range.Pan(xCenter - XAxis.Range.Center);
    }
}
