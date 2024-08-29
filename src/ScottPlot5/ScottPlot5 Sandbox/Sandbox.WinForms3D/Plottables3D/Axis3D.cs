using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;

namespace Sandbox.WinForms3D.Plottables3D;

public class Axis3D : IPlottable3D
{
    private readonly LabelStyle _labelStyle = new LabelStyle { FontSize = 15, Bold = true, Alignment = Alignment.MiddleCenter };

    public void Render(RenderPack3D rp)
    {
        RenderGrid(rp);
        RenderSpines(rp);
        RenderAxisLabels(rp);
    }

    private void RenderGrid(RenderPack3D rp)
    {
        const int divisions = 10;

        List<Line3D> lines = [];

        for (int xIndex = 0; xIndex <= divisions; xIndex++)
        {
            double x = xIndex * 1.0 / divisions;
            Point3D xStart = new Point3D(x, 0, 0);
            Point3D yEnd = new Point3D(x, 1, 0);
            Point3D zEnd = new Point3D(x, 0, 1);
            lines.Add(new Line3D(xStart, yEnd));
            lines.Add(new Line3D(xStart, zEnd));
        }

        for (int yIndex = 0; yIndex <= divisions; yIndex++)
        {
            double y = yIndex * 1.0 / divisions;
            Point3D yStart = new Point3D(0, y, 0);
            Point3D xEnd = new Point3D(1, y, 0);
            Point3D zEnd = new Point3D(0, y, 1);
            lines.Add(new Line3D(yStart, xEnd));
            lines.Add(new Line3D(yStart, zEnd));
        }

        for (int zIndex = 0; zIndex <= divisions; zIndex++)
        {
            double z = zIndex * 1.0 / divisions;
            Point3D zStart = new Point3D(0, 0, z);
            Point3D xEnd = new Point3D(1, 0, z);
            Point3D yEnd = new Point3D(0, 1, z);
            lines.Add(new Line3D(zStart, xEnd));
            lines.Add(new Line3D(zStart, yEnd));
        }

        foreach (Line3D line in lines)
        {
            line.LineStyle.Color = Colors.Black.WithAlpha(.2);
            line.Render(rp);
        }
    }

    private static void RenderSpines(RenderPack3D rp)
    {
        Point3D origin = new Point3D(0, 0, 0);
        Point3D xUnit = new Point3D(1, 0, 0);
        Point3D yUnit = new Point3D(0, 1, 0);
        Point3D zUnit = new Point3D(0, 0, 1);

        List<Line3D> lines2 =
        [
            new Line3D(origin, xUnit, 2, Colors.Red),
            new Line3D(origin, yUnit, 2, Colors.Green),
            new Line3D(origin, zUnit, 2, Colors.Blue),
        ];

        foreach (Line3D line in lines2)
        {
            line.Render(rp);
        }
    }

    private void RenderAxisLabels(RenderPack3D rp)
    {
        const double padding = .1;
        Point3D ptX = new Point3D(1 + padding, 0, 0);
        Point3D ptY = new Point3D(0, 1 + padding, 0);
        Point3D ptZ = new Point3D(0, 0, 1 + padding);

        _labelStyle.Text = "X";
        _labelStyle.ForeColor = Colors.Red;
        _labelStyle.Render(rp.Canvas, rp.GetPixel(ptX), rp.Paint);

        _labelStyle.Text = "Y";
        _labelStyle.ForeColor = Colors.Green;
        _labelStyle.Render(rp.Canvas, rp.GetPixel(ptY), rp.Paint);

        _labelStyle.Text = "Z";
        _labelStyle.ForeColor = Colors.Blue;
        _labelStyle.Render(rp.Canvas, rp.GetPixel(ptZ), rp.Paint);
    }
}
