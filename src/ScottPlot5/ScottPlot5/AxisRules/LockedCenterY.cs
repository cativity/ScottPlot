namespace ScottPlot.AxisRules;

public class LockedCenterY(IYAxis yAxis, double yCenter) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Pan(yCenter - YAxis.Range.Center);
    }
}
