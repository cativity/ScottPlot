using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

[ToolboxItem(true)]
public class FormsPlot : FormsPlotBase
{
    public SKControl? SKControl { get; private set; }

    public override GRContext? GRContext => null;

    public FormsPlot()
    {
#if NETFRAMEWORK
        // do not attempt renders inside visual studio at design time
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
#endif

        HandleCreated += (_, _) => SetupSKControl();
        HandleDestroyed += (_, _) => TeardownSKControl();
        SetupSKControl();
        Plot.FigureBackground.Color = Color.FromColor(SystemColors.Control);
        Plot.DataBackground.Color = Colors.White;
    }

    private void SetupSKControl()
    {
        TeardownSKControl();

        SKControl = new SKControl { Dock = DockStyle.Fill };

        SKControl.PaintSurface += SKElementPaintSurface;
        SKControl.MouseDown += SKElementMouseDown;
        SKControl.MouseUp += SKElementMouseUp;
        SKControl.MouseMove += SKElementMouseMove;
        SKControl.DoubleClick += SKElementDoubleClick;
        SKControl.MouseWheel += SKElementMouseWheel;
        SKControl.KeyDown += SKElementKeyDown;
        SKControl.KeyUp += SKElementKeyUp;

        Controls.Add(SKControl);
    }

    private void TeardownSKControl()
    {
        if (SKControl is null)
        {
            return;
        }

        SKControl.PaintSurface -= SKElementPaintSurface;
        SKControl.MouseDown -= SKElementMouseDown;
        SKControl.MouseUp -= SKElementMouseUp;
        SKControl.MouseMove -= SKElementMouseMove;
        SKControl.DoubleClick -= SKElementDoubleClick;
        SKControl.MouseWheel -= SKElementMouseWheel;
        SKControl.KeyDown -= SKElementKeyDown;
        SKControl.KeyUp -= SKElementKeyUp;

        Controls.Remove(SKControl);

        if (!SKControl.IsDisposed)
        {
            SKControl.Dispose();
        }
    }

    private void SKElementPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }

    public override void Refresh()
    {
        SKControl?.Invalidate();
        base.Refresh();
    }
}
