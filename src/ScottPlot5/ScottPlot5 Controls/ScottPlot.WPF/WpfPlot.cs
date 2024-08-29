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

    protected override FrameworkElement PlotFrameworkElement => _skElement!;

    public override GRContext GRContext => null!;

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
