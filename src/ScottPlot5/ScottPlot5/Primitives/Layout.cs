namespace ScottPlot;

public readonly struct Layout(PixelRect figureRect, PixelRect dataRect, Dictionary<IPanel, float> sizes, Dictionary<IPanel, float> offsets) : IEquatable<Layout>
{
    /// <summary>
    ///     Size of the figure this layout represents
    /// </summary>
    public PixelRect FigureRect { get; } = figureRect;

    /// <summary>
    ///     Final size of the data area
    /// </summary>
    public PixelRect DataRect { get; } = dataRect;

    /// <summary>
    ///     Distance (pixels) each panel is to be placed from the edge of the data rectangle
    /// </summary>
    public Dictionary<IPanel, float> PanelOffsets { get; } = offsets;

    /// <summary>
    ///     Size (pixels) of each panel in the dimension perpendicular to the data edge it is placed on
    /// </summary>
    public Dictionary<IPanel, float> PanelSizes { get; } = sizes;

    public bool Equals(Layout other)
    {
        return FigureRect.Equals(other.FigureRect) && DataRect.Equals(other.DataRect);
    }

    public override bool Equals(object? obj)
    {
        return obj is Layout layout && Equals(layout);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override int GetHashCode() => HashCode.Combine(FigureRect, DataRect);

    public static bool operator ==(Layout left, Layout right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Layout left, Layout right)
    {
        return !left.Equals(right);
    }
}
