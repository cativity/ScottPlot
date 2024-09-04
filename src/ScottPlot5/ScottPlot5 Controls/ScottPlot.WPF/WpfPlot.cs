using System.ComponentModel;
using System.Windows;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace ScottPlot.WPF;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[TemplatePart(Name = _partSkElement, Type = typeof(SKElement))]
public class WpfPlot : WpfPlotBase
{
    private const string _partSkElement = "PART_SKElement";

    private SKElement? _skElement;

    protected override FrameworkElement? PlotFrameworkElement => _skElement;

    public override GRContext? GRContext => null;

    static WpfPlot()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfPlot), new FrameworkPropertyMetadata(typeof(WpfPlot)));
    }

    public override void OnApplyTemplate()
    {
        _skElement = Template.FindName(_partSkElement, this) as SKElement;

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
