using System.ComponentModel;
using System.Windows;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace ScottPlot.WPF;

[ToolboxItem(false)]
[DesignTimeVisible(true)]
[TemplatePart(Name = _partSKElement, Type = typeof(SKGLElement))]
public class WpfPlotGL : WpfPlotBase
{
    private const string _partSKElement = "PART_SKElement";

    private SKGLElement? _skElement;

    protected override FrameworkElement? PlotFrameworkElement => _skElement;

    public override GRContext GRContext => _skElement?.GrContext ?? GRContext.CreateGl();

    static WpfPlotGL()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfPlotGL), new FrameworkPropertyMetadata(typeof(WpfPlotGL)));
    }

    public override void OnApplyTemplate()
    {
        _skElement = Template.FindName(_partSKElement, this) as SKGLElement;

        if (_skElement is null)
        {
            return;
        }

        _skElement.PaintSurface += (sender, e) =>
        {
            float width = e.Surface.Canvas.LocalClipBounds.Width;
            float height = e.Surface.Canvas.LocalClipBounds.Height;
            PixelRect rect = new PixelRect(0, width, height, 0);
            Plot.Render(e.Surface.Canvas, rect);
        };

        _skElement.MouseDown += SKElementMouseDown;
        _skElement.MouseUp += SKElementMouseUp;
        _skElement.MouseMove += SKElementMouseMove;
        _skElement.MouseWheel += SKElementMouseWheel;
        _skElement.KeyDown += SKElementKeyDown;
        _skElement.KeyUp += SKElementKeyUp;
    }

    public override void Refresh()
    {
        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(Refresh);

            return;
        }

        _skElement?.InvalidateVisual();
    }
}
