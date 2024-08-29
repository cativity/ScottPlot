namespace ScottPlot;

/// <summary>
///     Represents the distance from one pixel relative to another in pixel units.
///     Increasing X offset moves a pixel to the right.
///     Increasing Y offset moves a pixel downward.
/// </summary>
public record struct PixelOffset(float X, float Y)
{
    public static PixelOffset Zero => new PixelOffset(0, 0);

    public static PixelOffset NaN => new PixelOffset(float.NaN, float.NaN);
}
