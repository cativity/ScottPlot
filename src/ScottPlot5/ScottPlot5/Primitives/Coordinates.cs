namespace ScottPlot;

/// <summary>
///     Represents a point in coordinate space (X and Y axis units)
/// </summary>
public struct Coordinates(double x, double y) : IEquatable<Coordinates>
{
    public double X { get; set; } = x;

    public double Y { get; set; } = y;

    public readonly bool AreReal => NumericConversion.IsReal(X) && NumericConversion.IsReal(Y);

    public readonly double DistanceSquared(Coordinates pt)
    {
        double dX = Math.Abs(X - pt.X);
        double dY = Math.Abs(Y - pt.Y);

        return (dX * dX) + (dY * dY);
    }

    public readonly double Distance(Coordinates pt) => Math.Sqrt(DistanceSquared(pt));

    public override readonly string ToString() => $"Coordinates {{ X = {X}, Y = {Y} }}";

    public static Coordinates NaN => new Coordinates(double.NaN, double.NaN);

    public static Coordinates Origin => new Coordinates(0, 0);

    public readonly Coordinates Infinity => new Coordinates(double.PositiveInfinity, double.PositiveInfinity);

    public readonly Coordinates Rotated => new Coordinates(Y, X);

    public readonly bool Equals(Coordinates other) => Equals(X, other.X) && Equals(Y, other.Y);

    public override readonly bool Equals(object? obj)
    {
        return obj is Coordinates other && Equals(other);
    }

    public static bool operator ==(Coordinates a, Coordinates b) => a.Equals(b);

    public static bool operator !=(Coordinates a, Coordinates b) => !a.Equals(b);

    public override readonly int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

    public readonly CoordinateRect ToRect(double radiusX, double radiusY) => new(X - radiusX, X + radiusX, Y - radiusY, Y + radiusY);

    public readonly CoordinateRect ToRect(double radius) => ToRect(radius, radius);

    public readonly Coordinates WithDelta(double dX, double dY) => new(X + dX, Y + dY);
}
