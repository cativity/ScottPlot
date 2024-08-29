namespace ScottPlot.AxisRules;

public class SquareZoomOut(IXAxis xAxis, IYAxis yAxis) : IAxisRule
{
    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the DataRect must wait for the layout to occur
        if (beforeLayout)
        {
            return;
        }

        double unitsPerPxX = xAxis.Width / rp.DataRect.Width;
        double unitsPerPxY = yAxis.Height / rp.DataRect.Height;
        double maxUnitsPerPx = Math.Max(unitsPerPxX, unitsPerPxY);

        double halfHeight = rp.DataRect.Height / 2 * maxUnitsPerPx;
        double yMin = yAxis.Range.Center - halfHeight;
        double yMax = yAxis.Range.Center + halfHeight;

        bool invertedY = yAxis.Min > yAxis.Max;

        if (invertedY)
        {
            yAxis.Range.Set(yMax, yMin);
        }
        else
        {
            yAxis.Range.Set(yMin, yMax);
        }

        double halfWidth = rp.DataRect.Width / 2 * maxUnitsPerPx;
        double xMin = xAxis.Range.Center - halfWidth;
        double xMax = xAxis.Range.Center + halfWidth;

        bool invertedX = xAxis.Min > xAxis.Max;

        if (invertedX)
        {
            xAxis.Range.Set(xMax, xMin);
        }
        else
        {
            xAxis.Range.Set(xMin, xMax);
        }
    }
}
