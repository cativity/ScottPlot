namespace ScottPlot.Rendering.RenderActions;

public class AutoScaleUnsetAxes : IRenderAction
{
    public void Render(RenderPack rp)
    {
        IEnumerable<IPlottable> visiblePlottables = rp.Plot.PlottableList.Where(static x => x.IsVisible);

        if (visiblePlottables.Any())
        {
            AutoscaleUnsetAxesToData(rp.Plot);
        }
        else
        {
            ApplyDefaultLimitsToUnsetAxes(rp.Plot);
        }
    }

    private static void AutoscaleUnsetAxesToData(Plot plot)
    {
        foreach (IPlottable plottable in plot.PlottableList)
        {
            Debug.Assert(plottable.Axes.XAxis is not null);
            Debug.Assert(plottable.Axes.YAxis is not null);
            bool xAxisNeedsScaling = !plottable.Axes.XAxis.Range.HasBeenSet;
            bool yAxisNeedsScaling = !plottable.Axes.YAxis.Range.HasBeenSet;

            if (xAxisNeedsScaling || yAxisNeedsScaling)
            {
                plot.Axes.AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis, xAxisNeedsScaling, yAxisNeedsScaling);
            }
        }
    }

    private static void ApplyDefaultLimitsToUnsetAxes(Plot plot)
    {
        if (!plot.Axes.Bottom.Range.HasBeenSet)
        {
            plot.Axes.SetLimitsX(AxisLimits.Default);
        }

        if (!plot.Axes.Left.Range.HasBeenSet)
        {
            plot.Axes.SetLimitsY(AxisLimits.Default);
        }
    }
}
