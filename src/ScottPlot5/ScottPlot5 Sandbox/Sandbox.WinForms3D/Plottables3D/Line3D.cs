using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;
using Color = ScottPlot.Color;

namespace Sandbox.WinForms3D.Plottables3D;

public readonly struct Line3D : IPlottable3D
{
    public Primitives3D.Line3D Line { get; }

    public LineStyle LineStyle { get; }

    public Line3D(Point3D start, Point3D end, float lineWidth = 1, Color? color = null)
    {
        Line = new Primitives3D.Line3D(start, end);

        LineStyle = new LineStyle { Width = lineWidth, Color = color ?? Colors.Black };
    }

    public void Render(RenderPack3D rp)
    {
        Pixel start = rp.GetPixel(Line.Start);
        Pixel end = rp.GetPixel(Line.End);
        PixelLine line = new PixelLine(start, end);
        Drawing.DrawLine(rp.Canvas, rp.Paint, line, LineStyle);
    }
}
