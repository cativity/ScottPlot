using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ScottPlot.Control;
using ScottPlot.Interactivity;
using SkiaSharp;

namespace ScottPlot.WPF;

public abstract class WpfPlotBase : System.Windows.Controls.Control, IPlotControl
{
    public abstract GRContext? GRContext { get; }

    public abstract void Refresh();

    public Plot Plot { get; internal set; }

    public IPlotInteraction Interaction { get; set; }

    public float DisplayScale { get; set; }

    public IPlotMenu Menu { get; set; }

    public UserInputProcessor UserInputProcessor { get; }

    protected abstract FrameworkElement? PlotFrameworkElement { get; }

    static WpfPlotBase()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfPlotBase), new FrameworkPropertyMetadata(typeof(WpfPlotBase)));
    }

    protected WpfPlotBase()
    {
        Plot = new Plot() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        UserInputProcessor = new UserInputProcessor(Plot);
        Menu = new WpfPlotMenu(this);
        Focusable = true;
    }

    public void Reset()
    {
        Reset(new Plot());
    }

    public void Reset(Plot newPlot)
    {
        Plot oldPlot = Plot;
        Plot = newPlot;
        oldPlot?.Dispose();
        Plot.PlotControl = this;
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    internal void SKElementMouseDown(object? sender, MouseButtonEventArgs e)
    {
        Keyboard.Focus(this);
        Debug.Assert(PlotFrameworkElement is not null);
        Interaction.MouseDown(e.ToPixel(PlotFrameworkElement), e.OldToButton());
        UserInputProcessor.ProcessMouseDown(PlotFrameworkElement, e);
        (sender as UIElement)?.CaptureMouse();

        if (e.ClickCount == 2)
        {
            Interaction.DoubleClick();
        }
    }

    internal void SKElementMouseUp(object? sender, MouseButtonEventArgs e)
    {
        Debug.Assert(PlotFrameworkElement is not null);
        Interaction.MouseUp(e.ToPixel(PlotFrameworkElement), e.OldToButton());
        UserInputProcessor.ProcessMouseUp(PlotFrameworkElement, e);
        (sender as UIElement)?.ReleaseMouseCapture();
    }

    internal void SKElementMouseMove(object? sender, MouseEventArgs e)
    {
        Debug.Assert(PlotFrameworkElement is not null);
        Interaction.OnMouseMove(e.ToPixel(PlotFrameworkElement));
        UserInputProcessor.ProcessMouseMove(PlotFrameworkElement, e);
    }

    internal void SKElementMouseWheel(object? sender, MouseWheelEventArgs e)
    {
        Debug.Assert(PlotFrameworkElement is not null);
        Interaction.MouseWheelVertical(e.ToPixel(PlotFrameworkElement), e.Delta);
        UserInputProcessor.ProcessMouseWheel(PlotFrameworkElement, e);
    }

    internal void SKElementKeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.OldToKey());
        UserInputProcessor.ProcessKeyDown(e);
    }

    internal void SKElementKeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.OldToKey());
        UserInputProcessor.ProcessKeyUp(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        Interaction.KeyDown(e.OldToKey());
        UserInputProcessor.ProcessKeyDown(e);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        Interaction.KeyUp(e.OldToKey());
        UserInputProcessor.ProcessKeyUp(e);
        base.OnKeyUp(e);
    }

    public float DetectDisplayScale() => (float)VisualTreeHelper.GetDpi(this).DpiScaleX;
}
