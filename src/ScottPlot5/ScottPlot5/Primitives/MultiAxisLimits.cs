﻿namespace ScottPlot;

/// <summary>
///     Stores the <see cref="CoordinateRange" /> for all axes on the plot
///     and has methods that can be used to recall them at a future point in time.
/// </summary>
public class MultiAxisLimits(Plot plot)
{
    private readonly Dictionary<IAxis, CoordinateRange> _axisRanges = plot.Axes.GetAxes().ToDictionary(static x => x, static x => x.Range.ToCoordinateRange);

    /// <summary>
    ///     Set all axis limits to their original ranges
    /// </summary>
    public void Recall()
    {
        foreach (KeyValuePair<IAxis, CoordinateRange> axisAndRange in _axisRanges)
        {
            axisAndRange.Key.Range.Set(axisAndRange.Value);
        }
    }

    /// <summary>
    ///     Set limits of the given axis to its original range
    /// </summary>
    public void Recall(IAxis axis)
    {
        if (_axisRanges.TryGetValue(axis, out CoordinateRange range))
        {
            axis.Range.Set(range);
        }
    }
}
