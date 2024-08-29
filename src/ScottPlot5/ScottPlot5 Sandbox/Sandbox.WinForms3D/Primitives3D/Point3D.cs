namespace Sandbox.WinForms3D.Primitives3D;

public record struct Point3D(double X, double Y, double Z)
{
    public static readonly Point3D Origin = new Point3D(0, 0, 0);

    public readonly Point3D WithZoom(double zoom) => new Point3D(X * zoom, Y * zoom, Z * zoom);

    public readonly Point3D WithPan(double x, double y, double z) => new Point3D(X + x, Y + y, Z + z);

    public readonly Point3D Translated(Point3D oldOrigin, Point3D newOrigin)
    {
        double dX = newOrigin.X - oldOrigin.X;
        double dY = newOrigin.Y - oldOrigin.Y;
        double dZ = newOrigin.Z - oldOrigin.Z;

        return new Point3D(X + dX, Y + dY, Z + dZ);
    }

    public readonly Point3D RotatedX(double degrees)
    {
        double radians = Math.PI * degrees / 180.0f;
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);

        return new Point3D(X, (Y * cos) + (Z * sin), (Y * -sin) + (Z * cos));
    }

    public readonly Point3D RotatedY(double degrees)
    {
        double radians = Math.PI * degrees / 180.0;
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);

        return new Point3D((X * cos) + (Z * sin), Y, (X * -sin) + (Z * cos));
    }

    public readonly Point3D RotatedZ(double degrees)
    {
        double radians = Math.PI * degrees / 180.0;
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);

        return new Point3D((X * cos) + (Y * sin), (X * -sin) + (Y * cos), Z);
    }
}
