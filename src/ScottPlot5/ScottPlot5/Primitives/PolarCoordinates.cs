namespace ScottPlot;

/// <summary>
///     Represents a point in polar coordinate space
/// </summary>
public struct PolarCoordinates(double radius, Angle angle)
{
    public double Radius { get; set; } = radius;

    public Angle Angle { get; set; } = angle;

    public override readonly string ToString() => $"PolarCoordinates {{ Radius = {Radius}, {Angle} }}";

    public readonly Coordinates CartesianCoordinates => new Coordinates(Radius * Math.Cos(Angle.Radians), Radius * Math.Sin(Angle.Radians));
}
