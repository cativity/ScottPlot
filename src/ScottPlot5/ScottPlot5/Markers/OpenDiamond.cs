namespace ScottPlot.Markers;

internal class OpenDiamond : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        SKPoint[] pointsList =
        [
            new SKPoint(center.X + radius, center.Y), new SKPoint(center.X, center.Y + radius), new SKPoint(center.X - radius, center.Y),
            new SKPoint(center.X, center.Y - radius)
        ];

        SKPath path = new SKPath();
        path.AddPoly(pointsList);

        Drawing.DrawPath(canvas, paint, path, markerStyle.LineStyle);
    }
}
