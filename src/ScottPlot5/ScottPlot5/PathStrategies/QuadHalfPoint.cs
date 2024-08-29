namespace ScottPlot.PathStrategies;

/// <summary>
///     Connect points with curved lines which ease into and out of the midpoint between each pair.
///     This strategy does not "overshoot" points in the Y direction.
/// </summary>
public class QuadHalfPoint : IPathStrategy
{
    public SKPath GetPath(IEnumerable<Pixel> pixels)
    {
        SKPath path = new SKPath();

        bool moveToNextPoint = true;

        foreach (Pixel pixel in pixels)
        {
            if (float.IsNaN(pixel.X) || float.IsNaN(pixel.Y))
            {
                moveToNextPoint = true;
            }

            SKPoint thisPoint = pixel.ToSKPoint();

            if (moveToNextPoint)
            {
                path.MoveTo(thisPoint);
                moveToNextPoint = false;
            }
            else
            {
                SKPoint lastPoint = path.LastPoint;

                float halfX = (lastPoint.X + thisPoint.X) / 2;
                float halfY = (lastPoint.Y + thisPoint.Y) / 2;
                SKPoint halfPoint = new SKPoint(halfX, halfY);

                SKPoint controlPoint1 = new SKPoint(halfPoint.X, lastPoint.Y);
                SKPoint controlPoint2 = new SKPoint(halfPoint.X, thisPoint.Y);

                path.QuadTo(controlPoint1, halfPoint);
                path.QuadTo(controlPoint2, thisPoint);
            }
        }

        return path;
    }
}
