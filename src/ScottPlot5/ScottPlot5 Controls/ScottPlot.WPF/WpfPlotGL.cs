using System.ComponentModel;
using System.Windows;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace ScottPlot.WPF;

[ToolboxItem(false)]
[DesignTimeVisible(true)]
[TemplatePart(Name = _partSkElement, Type = typeof(SKGLElement))]
public class WpfPlotGL : WpfPlotBase
{
    private const string _partSkElement = "PART_SKElement";

    private SKGLElement? _skElement;

    protected override FrameworkElement PlotFrameworkElement => _skElement!;

    public override GRContext GRContext => _skElement?.GrContext ?? GRContext.CreateGl();

    static WpfPlotGL()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfPlotGL), new FrameworkPropertyMetadata(typeof(WpfPlotGL)));
    }

    public override void OnApplyTemplate()
    {
        _skElement = Template.FindName(_partSkElement, this) as SKGLElement;

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

        _skElement.MouseDown += SKElement_MouseDown;
        _skElement.MouseUp += SKElement_MouseUp;
        _skElement.MouseMove += SKElement_MouseMove;
        _skElement.MouseWheel += SKElement_MouseWheel;
        _skElement.KeyDown += SKElement_KeyDown;
        _skElement.KeyUp += SKElement_KeyUp;
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
