namespace ScottPlot;

public readonly struct PixelSize : IEquatable<PixelSize>
{
    public readonly float Width;
    public readonly float Height;

    public float Area => Width * Height;

    public float Diagonal => (float)Math.Sqrt((Width * Width) + (Height * Height));

    public PixelSize(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public PixelSize(double width, double height)
    {
        Width = (float)width;
        Height = (float)height;
    }

    public override string ToString() => $"PixelSize: Width={Width}, Height={Height}";

    public static PixelSize Zero => new PixelSize(0, 0);

    public static PixelSize NaN => new PixelSize(float.NaN, float.NaN);

    public static PixelSize Infinity => new PixelSize(float.PositiveInfinity, float.PositiveInfinity);

    public PixelRect ToPixelRect() => new PixelRect(0, Width, Height, 0);

    public PixelRect ToPixelRect(Pixel pixel, Alignment alignment)
    {
        PixelRect rect = new PixelRect(pixel.X, pixel.X + Width, pixel.Y + Height, pixel.Y);

        return rect.WithDelta(-Width * alignment.HorizontalFraction(), -Height * (1 - alignment.VerticalFraction()));
    }

    public bool Contains(PixelSize size) => Width >= size.Width && Height >= size.Height;

    public PixelSize Max(PixelSize rect2) => new PixelSize(Math.Max(Width, rect2.Width), Math.Max(Height, rect2.Height));

    public bool Equals(PixelSize other) => Equals(Width, other.Width) && Equals(Height, other.Height);

    public override bool Equals(object? obj)
    {
        return obj is PixelSize other && Equals(other);
    }

    public static bool operator ==(PixelSize a, PixelSize b) => a.Equals(b);

    public static bool operator !=(PixelSize a, PixelSize b) => !a.Equals(b);

    public override int GetHashCode() => Width.GetHashCode() ^ Height.GetHashCode();

    public PixelSize Expanded(PixelPadding pad) => new PixelSize(Width + pad.Left + pad.Right, Height + pad.Top + pad.Bottom);

    public PixelSize Contracted(PixelPadding pad) => new PixelSize(Width - pad.Left - pad.Right, Height - pad.Top - pad.Bottom);
}
