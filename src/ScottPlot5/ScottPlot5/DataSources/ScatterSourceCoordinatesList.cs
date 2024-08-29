namespace ScottPlot.DataSources;

/// <summary>
///     This data source manages X/Y points as a collection of coordinates
/// </summary>
public class ScatterSourceCoordinatesList(List<Coordinates> coordinates) : IScatterSource
{
    public int MinRenderIndex { get; set; }

    public int MaxRenderIndex { get; set; } = int.MaxValue;

    private int RenderIndexCount => Math.Min(coordinates.Count - 1, MaxRenderIndex) - MinRenderIndex + 1;

    public IReadOnlyList<Coordinates> GetScatterPoints() => coordinates.Skip(MinRenderIndex).Take(RenderIndexCount).ToList();

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();
        limits.Expand(coordinates.Skip(MinRenderIndex).Take(RenderIndexCount));

        return limits.AxisLimits;
    }

    public CoordinateRange GetLimitsX() => GetLimits().Rect.XRange;

    public CoordinateRange GetLimitsY() => GetLimits().Rect.YRange;

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i2 = 0; i2 < RenderIndexCount; i2++)
        {
            int i = MinRenderIndex + i2;
            double dX = (coordinates[i].X - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (coordinates[i].Y - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = (dX * dX) + (dY * dY);

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = coordinates[i].X;
                closestY = coordinates[i].Y;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared ? new DataPoint(closestX, closestY, closestIndex) : DataPoint.None;
    }

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => throw
               // TODO: Implement GetNearestX() in this DataSource
               // Code can be copied from ScatterSourceDoubleArray.GetNearestX() and modified as needed
               // Contributions are welcome!
               // https://github.com/ScottPlot/ScottPlot/issues/3807
               new NotImplementedException();
}
