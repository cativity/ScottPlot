using Eto.Forms;
using ScottPlot.Control;
using SkiaSharp;
using Eto.Drawing;
using System.Runtime.InteropServices;
using ScottPlot.Interactivity;

namespace ScottPlot.Eto;

public class EtoPlot : Drawable, IPlotControl
{
    public Plot Plot { get; internal set; }

    public GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }

    public UserInputProcessor UserInputProcessor { get; }

    public IPlotMenu Menu { get; set; }

    public float DisplayScale { get; set; }

    public EtoPlot()
    {
        Plot = new Plot { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        UserInputProcessor = new UserInputProcessor(Plot);
        Menu = new EtoPlotMenu(this);

        MouseDown += OnMouseDown;
        MouseUp += OnMouseUp;
        MouseMove += OnMouseMove;
        MouseWheel += OnMouseWheel;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        MouseDoubleClick += OnDoubleClick;
        SizeChanged += (s, e) => Refresh();
    }

    public void Reset()
    {
        Plot plot = new Plot { PlotControl = this };
        Reset(plot);
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
    }

    public void Refresh()
    {
        Invalidate();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    protected override void OnPaint(PaintEventArgs args)
    {
        base.OnPaint(args);

        SKImageInfo imageInfo = new SKImageInfo(Bounds.Width, Bounds.Height);

        using SKSurface? surface = SKSurface.Create(imageInfo);

        if (surface is null)
        {
            return;
        }

        Plot.Render(surface.Canvas, (int)surface.Canvas.LocalClipBounds.Width, (int)surface.Canvas.LocalClipBounds.Height);

        SKImage img = surface.Snapshot();
        SKPixmap pixels = img.ToRasterImage().PeekPixels();
        byte[] bytes = pixels.GetPixelSpan().ToArray();

        Bitmap? bmp = new Bitmap(Bounds.Width, Bounds.Height, PixelFormat.Format32bppRgba);

        using (BitmapData? data = bmp.Lock())
        {
            Marshal.Copy(bytes, 0, data.Data, bytes.Length);
        }

        args.Graphics.DrawImage(bmp, 0, 0);
    }

    private void OnMouseDown(object? sender, MouseEventArgs e)
    {
        Focus();
        Interaction.MouseDown(e.Pixel(), e.OldToButton());
        UserInputProcessor.ProcessMouseDown(e);
    }

    private void OnMouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.OldToButton());
        UserInputProcessor.ProcessMouseUp(e);
    }

    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
        UserInputProcessor.ProcessMouseMove(e);
    }

    private void OnMouseWheel(object? sender, MouseEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta.Height);
        UserInputProcessor.ProcessMouseWheel(e);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.OldToKey());
        UserInputProcessor.ProcessKeyDown(e);
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.OldToKey());
        UserInputProcessor.ProcessKeyUp(e);
    }

    private void OnDoubleClick(object? sender, MouseEventArgs e)
    {
        Interaction.DoubleClick();
    }

    public float DetectDisplayScale() => 1.0f;
    // TODO: improve support for DPI scale detection
    // https://github.com/ScottPlot/ScottPlot/issues/2760
}
