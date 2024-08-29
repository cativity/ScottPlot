namespace ScottPlot;

/// <summary>
///     Represents a distance in pixel units
/// </summary>
public readonly struct PixelLength(float length)
{
    public readonly float Length = length;

    public static implicit operator PixelLength(float length) => new PixelLength(length);

    public override string ToString() => $"{Length} pixels";
}
