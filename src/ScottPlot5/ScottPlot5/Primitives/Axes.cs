namespace ScottPlot;

/// <summary>
///     This object holds an X axis and Y axis and performs 2D coordinate/pixel conversions
/// </summary>
public class Axes : IAxes
{
    // TODO: these should probably be readonly and passed into the constructor
    public IXAxis? XAxis { get; set; }

    public IYAxis? YAxis { get; set; }

    public PixelRect DataRect { get; set; }

    public static Axes Default => new Axes();

    public Axes()
    {
    }

    public Axes(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public Coordinates GetCoordinates(Pixel pixel)
    {
        Debug.Assert(XAxis is not null);
        Debug.Assert(YAxis is not null);

        double x = XAxis.GetCoordinate(pixel.X, DataRect);
        double y = YAxis.GetCoordinate(pixel.Y, DataRect);

        return new Coordinates(x, y);
    }

    public double GetCoordinateX(float pixel)
    {
        Debug.Assert(XAxis is not null);

        return XAxis.GetCoordinate(pixel, DataRect);
    }

    public double GetCoordinateY(float pixel)
    {
        Debug.Assert(YAxis is not null);

        return YAxis.GetCoordinate(pixel, DataRect);
    }

    public Pixel GetPixel(Coordinates coordinates)
    {
        Debug.Assert(XAxis is not null);
        Debug.Assert(YAxis is not null);

        float x = XAxis.GetPixel(coordinates.X, DataRect);
        float y = YAxis.GetPixel(coordinates.Y, DataRect);

        return new Pixel(x, y);
    }

    public PixelLine GetPixelLine(CoordinateLine line)
    {
        Pixel pt1 = GetPixel(line.Start);
        Pixel pt2 = GetPixel(line.End);

        return new PixelLine(pt1, pt2);
    }

    public float GetPixelX(double xCoordinate)
    {
        Debug.Assert(XAxis is not null);

        return XAxis.GetPixel(xCoordinate, DataRect);
    }

    public float GetPixelY(double yCoordinate)
    {
        Debug.Assert(YAxis is not null);

        return YAxis.GetPixel(yCoordinate, DataRect);
    }

    public PixelRect GetPixelRect(CoordinateRect rect)
        => new PixelRect(GetPixelX(rect.Left), GetPixelX(rect.Right), GetPixelY(rect.Bottom), GetPixelY(rect.Top));
}
