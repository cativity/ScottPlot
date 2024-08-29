namespace ScottPlot;

/// <summary>
///     Describes X/Y location in pixel or coordinate space
/// </summary>
public class Location
{
    public LocationUnit Unit { get; init; }

    public double X { get; set; }

    public double Y { get; set; }

    private Location()
    {
    }

    public Pixel GetPixel()
    {
        if (Unit == LocationUnit.Pixel)
        {
            return new Pixel(X, Y);
        }

        throw new InvalidOperationException("units are not pixels");
    }

    public Coordinates GetCoordinates()
    {
        if (Unit == LocationUnit.Coordinates)
        {
            return new Coordinates(X, Y);
        }

        throw new InvalidOperationException("units are not pixels");
    }

    public static Location Pixel(float x, float y) => new Location { X = x, Y = y, Unit = LocationUnit.Pixel };

    public static Location Coordinates(float x, float y) => new Location { X = x, Y = y, Unit = LocationUnit.Coordinates };

    public static Location Unspecified => new Location { X = double.NaN, Y = double.NaN, Unit = LocationUnit.Unspecified };
}
