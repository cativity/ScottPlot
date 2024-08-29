namespace ScottPlot.AxisRules;

public class SquarePreserveX(IXAxis xAxis, IYAxis yAxis) : IAxisRule
{
    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the DataRect must wait for the layout to occur
        if (beforeLayout)
        {
            return;
        }

        double unitsPerPxX = xAxis.Width / rp.DataRect.Width;
        double halfHeight = rp.DataRect.Height / 2 * unitsPerPxX;
        double yMin = yAxis.Range.Center - halfHeight;
        double yMax = yAxis.Range.Center + halfHeight;
        yAxis.Range.Set(yMin, yMax);
    }
}
