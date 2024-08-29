namespace ScottPlot;

/// <summary>
///     This data structure describes a single vertical column of pixels
///     that represents the Y span of an X range of data points.
/// </summary>
public readonly struct PixelColumn(float x, float enter, float exit, float bottom, float top)
{
    public readonly float X = x;
    public readonly float Enter = enter;
    public readonly float Exit = exit;
    public readonly float Bottom = bottom;
    public readonly float Top = top;

    public bool HasData => !float.IsNaN(Enter);

    public static PixelColumn WithoutData(float x) => new PixelColumn(x, float.NaN, float.NaN, float.NaN, float.NaN);

    public override string ToString() => $"x={X} y=[{Bottom}, {Top}], edges=[{Enter}, {Exit}]";
}
