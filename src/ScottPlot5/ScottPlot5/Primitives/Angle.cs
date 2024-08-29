namespace ScottPlot;

public struct Angle
{
    public double Degrees { get; set; }

    public double Radians
    {
        readonly get => Degrees * Math.PI / 180;
        set => Degrees = value * 180 / Math.PI;
    }

    public readonly Angle Normalized
    {
        get
        {
            double normalized = Degrees % 360;
            double degrees = normalized < 0 ? normalized + 360 : normalized;

            return FromDegrees(degrees);
        }
    }

    public static Angle FromDegrees(double degrees) => new Angle { Degrees = degrees };

    public static Angle FromRadians(double radians) => new Angle { Radians = radians };

    public override readonly string ToString() => $"Angle = {Degrees} degrees";

    public static Angle operator +(Angle a) => FromDegrees(+a.Degrees);

    public static Angle operator -(Angle a) => FromDegrees(-a.Degrees);

    public static Angle operator +(Angle a, Angle b) => FromDegrees(a.Degrees + b.Degrees);

    public static Angle operator -(Angle a, Angle b) => FromDegrees(a.Degrees - b.Degrees);
}
