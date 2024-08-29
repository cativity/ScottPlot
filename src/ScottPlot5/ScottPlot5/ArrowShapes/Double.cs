namespace ScottPlot.ArrowShapes;

public class Double : IArrowShape
{
    public void Render(RenderPack rp, PixelLine arrowLine, ArrowStyle arrowStyle)
    {
        float length = arrowLine.Length;

        rp.CanvasState.Save();
        rp.CanvasState.Translate(arrowLine.Pixel2);
        rp.CanvasState.RotateDegrees(arrowLine.AngleDegrees + 90);

        // origin is the tip, base extends to the right
        Pixel[] pixels =
        [
            new Pixel(0, 0),
            new Pixel(arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth / 2),
            new Pixel(arrowStyle.ArrowheadAxisLength, arrowStyle.ArrowWidth / 2),
            new Pixel(length - arrowStyle.ArrowheadAxisLength, arrowStyle.ArrowWidth / 2),
            new Pixel(length - arrowStyle.ArrowheadLength, arrowStyle.ArrowheadWidth / 2),
            new Pixel(length, 0),
            new Pixel(length - arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth / 2),
            new Pixel(length - arrowStyle.ArrowheadAxisLength, -arrowStyle.ArrowWidth / 2),
            new Pixel(arrowStyle.ArrowheadAxisLength, -arrowStyle.ArrowWidth / 2),
            new Pixel(arrowStyle.ArrowheadLength, -arrowStyle.ArrowheadWidth / 2),
            new Pixel(0, 0),
        ];

        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, arrowStyle.FillStyle);
        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, arrowStyle.LineStyle);

        rp.CanvasState.Restore();
    }
}
