namespace ScottPlot.StarAxes;

public class PolygonalStarAxis : SpokedStarAxis
{
    public override LineStyle AxisStyle { get; set; } = new LineStyle { Color = Colors.DarkGray };

    public override void Render(RenderPack rp, IAxes axes, double maxSpokeLength, int numSpokes, float rotationDegrees)
    {
        SKPaint paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        float[] ticks = [0.25f, 0.5f, 1];
        Pixel origin = axes.GetPixel(Coordinates.Origin);

        float minX = Math.Abs(axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(axes.GetPixelY(1) - origin.Y);
        double maxRadius = Math.Min(minX, minY) * maxSpokeLength;

        RenderSpokes(rp, axes, maxRadius, numSpokes, rotationDegrees);

        using SKAutoCanvasRestore _ = new SKAutoCanvasRestore(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(rotationDegrees);

        double sweepAngle = 2 * Math.PI / numSpokes;

        foreach (float tick in ticks)
        {
            double cumRotation = 0;
            SKPath path = new SKPath();

            for (int i = 0; i < numSpokes; i++)
            {
                double theta = cumRotation + (sweepAngle / 2);
                float x = (float)(tick * maxRadius * Math.Cos(theta));
                float y = (float)(tick * maxRadius * Math.Sin(theta));

                if (i == 0)
                {
                    path.MoveTo(x, y);
                }
                else
                {
                    path.LineTo(x, y);
                }

                cumRotation += sweepAngle;
            }

            path.Close();
            rp.Canvas.DrawPath(path, paint);
        }
    }
}
