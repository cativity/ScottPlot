namespace ScottPlot;

/// <summary>
///     Describes a rectangle in 2D coordinate space.
/// </summary>
public struct CoordinateRect : IEquatable<CoordinateRect>
{
    public double Left { get; set; }

    public double Right { get; set; }

    public double Bottom { get; set; }

    public double Top { get; set; }

    public readonly double HorizontalCenter => (Right + Left) / 2;

    public readonly double VerticalCenter => (Top + Bottom) / 2;

    public readonly Coordinates Center => new Coordinates(HorizontalCenter, VerticalCenter);

    public readonly Coordinates TopLeft => new Coordinates(Left, Top);

    public readonly Coordinates TopRight => new Coordinates(Right, Top);

    public readonly Coordinates BottomLeft => new Coordinates(Left, Bottom);

    public readonly Coordinates BottomRight => new Coordinates(Right, Bottom);

    public readonly CoordinateRange XRange => new CoordinateRange(Left, Right);

    public readonly CoordinateRange YRange => new CoordinateRange(Bottom, Top);

    public readonly double Width => Right - Left;

    public readonly double Height => Top - Bottom;

    public readonly double Area => Width * Height;

    public readonly bool HasArea => Area != 0 && !double.IsNaN(Area) && !double.IsInfinity(Area);

    public readonly bool IsInvertedX => Left > Right;

    public readonly bool IsInvertedY => Top < Bottom;

    public CoordinateRect(CoordinateRange xRange, CoordinateRange yRange)
    {
        Left = xRange.Min;
        Right = xRange.Max;
        Bottom = yRange.Min;
        Top = yRange.Max;
    }

    public CoordinateRect(CoordinateRangeMutable xRange, CoordinateRangeMutable yRange)
    {
        Left = xRange.Min;
        Right = xRange.Max;
        Bottom = yRange.Min;
        Top = yRange.Max;
    }

    public CoordinateRect(IAxes axes)
        : this(axes.XAxis?.Range ?? throw new InvalidOperationException(), axes.YAxis?.Range ?? throw new InvalidOperationException())
    {
    }

    public CoordinateRect(Coordinates pt1, Coordinates pt2)
    {
        Left = Math.Min(pt1.X, pt2.X);
        Right = Math.Max(pt1.X, pt2.X);
        Bottom = Math.Min(pt1.Y, pt2.Y);
        Top = Math.Max(pt1.Y, pt2.Y);
    }

    public CoordinateRect(double left, double right, double bottom, double top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }

    public CoordinateRect(Coordinates point, CoordinateSize size)
    {
        Coordinates pt2 = new Coordinates(point.X + size.Width, point.Y + size.Height);
        Left = Math.Min(point.X, pt2.X);
        Right = Math.Max(point.X, pt2.X);
        Bottom = Math.Min(point.Y, pt2.Y);
        Top = Math.Max(point.Y, pt2.Y);
    }

    public readonly bool Contains(double x, double y) => x >= Left && x <= Right && y >= Bottom && y <= Top;

    public readonly bool ContainsX(double x) => x >= Left && x <= Right;

    public readonly bool ContainsY(double y) => y >= Bottom && y <= Top;

    public readonly CoordinateRect Expanded(Coordinates point)
    {
        double exLeft = Left;
        double exRight = Right;
        double exBottom = Bottom;
        double exTop = Top;

        if (!Contains(point))
        {
            exLeft = Math.Min(exLeft, point.X);
            exRight = Math.Max(exRight, point.X);
            exBottom = Math.Min(exBottom, point.Y);
            exTop = Math.Max(exTop, point.Y);
        }

        return new CoordinateRect(exLeft, exRight, exBottom, exTop);
    }

    public readonly bool Contains(Coordinates point) => Contains(point.X, point.Y);

    public static CoordinateRect Empty => new CoordinateRect(double.NaN, double.NaN, double.NaN, double.NaN);

    public readonly CoordinateRect WithTranslation(Coordinates p) => new CoordinateRect(Left + p.X, Right + p.X, Bottom + p.Y, Top + p.Y);

    public override readonly string ToString() => $"PixelRect: Left={Left} Right={Right} Bottom={Bottom} Top={Top}";

    public readonly bool Equals(CoordinateRect other)
        => Equals(Left, other.Left) && Equals(Right, other.Right) && Equals(Top, other.Top) && Equals(Bottom, other.Bottom);

    public override readonly bool Equals(object? obj)
    {
        return obj is CoordinateRect other && Equals(other);
    }

    public static bool operator ==(CoordinateRect a, CoordinateRect b) => a.Equals(b);

    public static bool operator !=(CoordinateRect a, CoordinateRect b) => !a.Equals(b);

    public override readonly int GetHashCode() => Left.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode() ^ Top.GetHashCode();
}
