namespace ScottPlot.AxisRules;

public class MaximumSpan(IXAxis xAxis, IYAxis yAxis, double xSpan, double ySpan) : IAxisRule
{
    public double XSpan { get; set; } = xSpan;

    public double YSpan { get; set; } = ySpan;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (xAxis.Range.Span > XSpan)
        {
            double xMin = xAxis.Range.Center - (XSpan / 2);
            double xMax = xAxis.Range.Center + (XSpan / 2);
            xAxis.Range.Set(xMin, xMax);
        }

        if (yAxis.Range.Span > YSpan)
        {
            double yMin = yAxis.Range.Center - (YSpan / 2);
            double yMax = yAxis.Range.Center + (YSpan / 2);
            yAxis.Range.Set(yMin, yMax);
        }
    }
}
