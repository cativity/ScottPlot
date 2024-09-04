using ScottPlot.Control;
using ScottPlot.Interactivity;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.Maui;

public class MauiPlot : ContentPage, IPlotControl
{
    private readonly SKCanvasView _canvas = CreateRenderTarget();

    public Plot Plot { get; internal set; } = new Plot();

    //private readonly ContentPage? _xamlRoot;

    public GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }

    public IPlotMenu Menu { get; set; }

    public UserInputProcessor UserInputProcessor { get; }

    public float DisplayScale { get; set; } = 1;

    private readonly SKPaint _skPaint = new SKPaint() { Style = SKPaintStyle.Stroke, Color = SKColors.DeepPink, StrokeWidth = 10, IsAntialias = true, };

    public MauiPlot()
    {
        Interaction = new Interaction(this);
        UserInputProcessor = new UserInputProcessor(Plot);
        Menu = new MauiPlotMenu(this);
        PointerGestureRecognizer pointerGestureRecognizer = new PointerGestureRecognizer();
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        Background = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);

        _canvas.PaintSurface += (s, e) =>
        {
            SKSurface surface = e.Surface;
            SKCanvas? canvas = surface.Canvas;

            SKRect skRectangle = new SKRect { Size = new SKSize(100, 100), Location = new SKPoint(-100f / 2, -100f / 2) };

            canvas.DrawRect(skRectangle, _skPaint);
        };

        pointerGestureRecognizer.PointerMoved += (s, e) =>
        {
            Pixel pixel = GetMousePos(e);
            Interaction.OnMouseMove(pixel);
        };

        pointerGestureRecognizer.PointerPressed += (s, e) =>
        {
            Pixel pixel = GetMousePos(e);
            Interaction.MouseDown(pixel, MouseButton.Left);
        };

        pointerGestureRecognizer.PointerReleased += (s, e) =>
        {
            Pixel pixel = GetMousePos(e);
            Interaction.MouseUp(pixel, MouseButton.Left);
        };

        tapGestureRecognizer.Tapped += (s, e) =>
        {
            if (e.Buttons == ButtonsMask.Secondary)
            {
                // Do something
                Pixel pixel = GetMousePos(e);
            }
        };

        _canvas.GestureRecognizers.Add(pointerGestureRecognizer);
        _canvas.GestureRecognizers.Add(tapGestureRecognizer);

        /*this.Content = _canvas;*/
    }

    private static Pixel GetMousePos(TappedEventArgs e)
    {
        Point? position = e.GetPosition(null);

        if (position is null)
        {
            return Pixel.NaN;
        }

        Point tmpPos = new Point(position.Value.X, position.Value.X);

        return tmpPos.ToPixel();
    }

    private static Pixel GetMousePos(PointerEventArgs e)
    {
        Point? position = e.GetPosition(null);

        if (position is null)
        {
            return Pixel.NaN;
        }

        Point tmpPos = new Point(position.Value.X, position.Value.X);

        return tmpPos.ToPixel();
    }

    private static SKCanvasView CreateRenderTarget() => new() { Background = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Transparent) };

    public void Reset()
    {
        Reset(new Plot());
    }

    public void Reset(Plot plot)
    {
        Plot = plot;
    }

    public void Refresh()
    {
        _canvas.InvalidateSurface();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    public float DetectDisplayScale()
    {
        //if (_xamlRoot is not null)
        //{
        //    Plot.ScaleFactor = _xamlRoot.Scale;
        //    DisplayScale = (float)_xamlRoot.Scale;
        //}

        return DisplayScale;
    }
}
