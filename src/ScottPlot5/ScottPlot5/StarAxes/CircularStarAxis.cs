namespace ScottPlot.StarAxes;

public class CircularStarAxis : SpokedStarAxis
{
    public override LineStyle AxisStyle { get; set; } = new LineStyle { Color = Colors.DarkGray };

    public override void Render(RenderPack rp, IAxes axes, double maxSpokeLength, int numSpokes, float rotationDegrees = 0)
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
        rp.Canvas.RotateDegrees(rotationDegrees); // Won't matter for the circles, but will if and when we add spokes

        foreach (float tick in ticks)
        {
            rp.Canvas.DrawCircle(0, 0, (float)(tick * maxRadius), paint);
        }
    }
}
