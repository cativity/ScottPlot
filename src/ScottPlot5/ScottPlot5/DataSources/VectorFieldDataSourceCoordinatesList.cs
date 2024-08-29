namespace ScottPlot.DataSources;

public class VectorFieldDataSourceCoordinatesList(IList<RootedCoordinateVector> rootedVectors) : IVectorFieldSource
{
    public int MinRenderIndex { get; set; }

    public int MaxRenderIndex { get; set; } = int.MaxValue;

    private int RenderIndexCount => Math.Min(rootedVectors.Count - 1, MaxRenderIndex) - MinRenderIndex + 1;

    public IReadOnlyList<RootedCoordinateVector> GetRootedVectors() => rootedVectors.Skip(MinRenderIndex).Take(RenderIndexCount).ToList();

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits();
        limits.Expand(rootedVectors.Select(v => v.Point).Skip(MinRenderIndex).Take(RenderIndexCount));

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
            double dX = (rootedVectors[i].Point.X - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (rootedVectors[i].Point.Y - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = (dX * dX) + (dY * dY);

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = rootedVectors[i].Point.X;
                closestY = rootedVectors[i].Point.Y;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared ? new DataPoint(closestX, closestY, closestIndex) : DataPoint.None;
    }
}
