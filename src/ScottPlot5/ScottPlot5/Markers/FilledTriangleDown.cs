﻿namespace ScottPlot.Markers;

internal class FilledTriangleDown : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // Length of each side of triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        // Bottom, right, and left vertices
        SKPoint[] pointsList =
        [
            new SKPoint(center.X, center.Y + radius), new SKPoint(center.X + xOffset, center.Y - yOffset),
            new SKPoint(center.X - xOffset, center.Y - yOffset)
        ];

        SKPath path = new SKPath();
        path.AddPoly(pointsList);

        PixelRect rect = new PixelRect(center, radius);
        Drawing.DrawPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.OutlineStyle);
    }
}
