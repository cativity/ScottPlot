namespace ScottPlot.AxisRules;

public class MaximumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits) : IAxisRule
{
    public AxisLimits Limits { get; set; } = limits;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        double xSpan = Math.Min(xAxis.Range.Span, Limits.HorizontalSpan);
        double ySpan = Math.Min(yAxis.Range.Span, Limits.VerticalSpan);

        if (xAxis.Range.Max > Limits.Right)
        {
            xAxis.Range.Max = Limits.Right;
            xAxis.Range.Min = Limits.Right - xSpan;
        }

        if (xAxis.Range.Min < Limits.Left)
        {
            xAxis.Range.Min = Limits.Left;
            xAxis.Range.Max = Limits.Left + xSpan;
        }

        if (yAxis.Range.Max > Limits.Top)
        {
            yAxis.Range.Max = Limits.Top;
            yAxis.Range.Min = Limits.Top - ySpan;
        }

        if (yAxis.Range.Min < Limits.Bottom)
        {
            yAxis.Range.Min = Limits.Bottom;
            yAxis.Range.Max = Limits.Bottom + ySpan;
        }
    }
}
