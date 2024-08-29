namespace ScottPlot.AxisRules;

public class SquarePreserveY(IXAxis xAxis, IYAxis yAxis) : IAxisRule
{
    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the DataRect must wait for the layout to occur
        if (beforeLayout)
        {
            return;
        }

        double unitsPerPxY = yAxis.Height / rp.DataRect.Height;
        double halfWidth = rp.DataRect.Width / 2 * unitsPerPxY;
        double xMin = xAxis.Range.Center - halfWidth;
        double xMax = xAxis.Range.Center + halfWidth;
        xAxis.Range.Set(xMin, xMax);
    }
}
