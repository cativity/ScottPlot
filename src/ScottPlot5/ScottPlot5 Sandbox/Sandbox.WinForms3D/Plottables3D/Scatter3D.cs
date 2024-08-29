using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;

namespace Sandbox.WinForms3D.Plottables3D;

public class Scatter3D : IPlottable3D
{
    public List<Point3D> Points { get; } = [];

    public LineStyle LineStyle { get; } = new LineStyle { Width = 1, Color = Colors.Blue };

    public MarkerStyle MarkerStyle { get; } = new MarkerStyle { IsVisible = true, Shape = MarkerShape.FilledCircle, FillColor = Colors.Blue, Size = 5, };

    public Scatter3D()
    {
        const double m = 10;

        for (double z = 0; z < 1.0; z += 0.01)
        {
            double x = (Math.Sin(z * m) / 2) + .5;
            double y = (Math.Cos(z * m) / 2) + .5;
            Points.Add(new Point3D(x, y, z));
        }
    }

    public void Render(RenderPack3D rp)
    {
        Pixel[] pixels = Points.Select(rp.GetPixel).ToArray();
        Drawing.DrawLines(rp.Canvas, rp.Paint, pixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, rp.Paint, pixels, MarkerStyle);
    }
}
