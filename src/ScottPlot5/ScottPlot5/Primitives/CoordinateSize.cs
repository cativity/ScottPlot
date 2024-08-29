namespace ScottPlot;

public readonly struct CoordinateSize(double width, double height) : IEquatable<CoordinateSize>
{
    public readonly double Width = width;
    public readonly double Height = height;

    public double Area => Width * Height;

    public bool Equals(CoordinateSize other) => Equals(Width, other.Width) && Equals(Height, other.Height);

    public override bool Equals(object? obj)
    {
        return obj is CoordinateSize other && Equals(other);
    }

    public static bool operator ==(CoordinateSize a, CoordinateSize b) => a.Equals(b);

    public static bool operator !=(CoordinateSize a, CoordinateSize b) => !a.Equals(b);

    public override int GetHashCode() => Width.GetHashCode() ^ Height.GetHashCode();
}
