namespace ScottPlot.Markers;

internal class FilledDiamond : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        float radius = size / 2;

        PixelRect rect = new PixelRect(center, radius);

        SKPoint[] pointsList =
        [
            new SKPoint(center.X + radius, center.Y), new SKPoint(center.X, center.Y + radius), new SKPoint(center.X - radius, center.Y),
            new SKPoint(center.X, center.Y - radius)
        ];

        SKPath path = new SKPath();
        path.AddPoly(pointsList);

        Drawing.DrawPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.OutlineStyle);
    }
}
