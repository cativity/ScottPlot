using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

[ToolboxItem(false)]
public class FormsPlotGL : FormsPlotBase
{
    public SKGLControl SKElement { get; }

    public override GRContext GRContext => SKElement.GRContext;

    public FormsPlotGL()
    {
        SKElement = new SKGLControl { Dock = DockStyle.Fill, VSync = true };
        SKElement.PaintSurface += SKControlPaintSurface;
        SKElement.MouseDown += SKElementMouseDown;
        SKElement.MouseUp += SKElementMouseUp;
        SKElement.MouseMove += SKElementMouseMove;
        SKElement.DoubleClick += SKElementDoubleClick;
        SKElement.MouseWheel += SKElementMouseWheel;
        SKElement.KeyDown += SKElementKeyDown;
        SKElement.KeyUp += SKElementKeyUp;

        Controls.Add(SKElement);

        HandleDestroyed += (_, _) => SKElement.Dispose();
    }

    private void SKControlPaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    public override void Refresh()
    {
        SKElement.Invalidate();
        base.Refresh();
    }
}
