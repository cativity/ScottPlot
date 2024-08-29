namespace ScottPlot.Control;

/// <summary>
///     This object remembers limits of all axes so it can restore them later.
///     It is used for mouse interaction to translate pixel distances
///     to coordinate distances based on previous renders.
/// </summary>
public class MultiAxisLimitManager
{
    private readonly Dictionary<IAxis, CoordinateRange> _rememberedLimits = [];

    public MultiAxisLimitManager()
    {
    }

    public MultiAxisLimitManager(Plot plot)
    {
        Remember(plot.Axes.GetAxes());
    }

    public MultiAxisLimitManager(IPlotControl control)
    {
        Remember(control.Plot.Axes.GetAxes());
    }

    /// <summary>
    ///     Remember the current limits of the given axis
    /// </summary>
    private void Remember(IAxis axis)
    {
        _rememberedLimits[axis] = new CoordinateRange(axis.Min, axis.Max);
    }

    /// <summary>
    ///     Remember the current limits of all given axes
    /// </summary>
    private void Remember(IEnumerable<IAxis> axes)
    {
        foreach (IAxis axis in axes)
        {
            Remember(axis);
        }
    }

    /// <summary>
    ///     Clear memory of previous axes and remember all the given axis limits
    /// </summary>
    public void Remember(MultiAxisLimitManager newMultiLimits)
    {
        foreach (IAxis axis in newMultiLimits._rememberedLimits.Keys)
        {
            CoordinateRange range = newMultiLimits._rememberedLimits[axis];
            _rememberedLimits[axis] = new CoordinateRange(range.Min, range.Max);
        }
    }

    /// <summary>
    ///     Update all axis limits of the given plot with those previously remembered
    /// </summary>
    public void Apply(Plot plot)
    {
        foreach (IAxis axis in plot.Axes.GetAxes())
        {
            if (_rememberedLimits.TryGetValue(axis, out CoordinateRange range))
            {
                axis.Range.Min = range.Min;
                axis.Range.Max = range.Max;
            }
        }
    }
}
