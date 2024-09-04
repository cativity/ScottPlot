namespace ScottPlot.StarAxes;

public abstract class SpokedStarAxis : IStarAxis
{
    public abstract LineStyle AxisStyle { get; set; }

    public abstract void Render(RenderPack rp, IAxes axes, double maxSpokeLength, int numSpokes, float rotationDegrees = 0);

    public void RenderSpokes(RenderPack rp, IAxes axes, double spokeLength, int numSpokes, float rotationDegrees = 0)
    {
        SKPaint paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        double sweepAngle = 2 * Math.PI / numSpokes;
        Pixel origin = axes.GetPixel(Coordinates.Origin);

        using SKAutoCanvasRestore _ = new SKAutoCanvasRestore(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(rotationDegrees);

        for (int i = 0; i < numSpokes; i++)
        {
            double theta = (i * sweepAngle) + (sweepAngle / 2);
            float x = (float)(spokeLength * Math.Cos(theta));
            float y = (float)(spokeLength * Math.Sin(theta));
            rp.Canvas.DrawLine(0, 0, x, y, paint);
        }
    }
}
