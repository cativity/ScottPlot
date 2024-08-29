using ScottPlot.Control;
using ScottPlot.Interactivity;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public abstract class FormsPlotBase : UserControl, IPlotControl
{
    public abstract GRContext? GRContext { get; }

    public Plot Plot { get; internal set; }

    public IPlotInteraction Interaction { get; set; }

    public IPlotMenu Menu { get; set; }

    public UserInputProcessor UserInputProcessor { get; }

    public float DisplayScale { get; set; }

    protected FormsPlotBase()
    {
        Plot = new Plot { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        UserInputProcessor = new UserInputProcessor(Plot);
        Menu = new FormsPlotMenu(this);

        // TODO: replace this with an annotation instead of title
        bool isDesignMode = Process.GetCurrentProcess().ProcessName == "devenv";
        Plot.Title(isDesignMode ? $"ScottPlot {Version.VersionString}" : string.Empty);
    }

    // make it so changing the background color of the control changes background color of the plot too
    public override System.Drawing.Color BackColor
    {
        get => base.BackColor;
        set
        {
            base.BackColor = value;

            //if (Plot is not null)
            //{
            Debug.Assert(Plot is not null);
            Plot.FigureBackground.Color = Color.FromColor(value);
            //}
        }
    }

    public void Reset()
    {
        Plot plot = new Plot();
        plot.FigureBackground.Color = Color.FromColor(BackColor);
        plot.DataBackground.Color = Colors.White;
        Reset(plot);
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
        Plot.PlotControl = this;
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    internal void SKElementMouseDown(object? sender, MouseEventArgs e)
    {
        Interaction.MouseDown(e.Pixel(), e.Button());
        UserInputProcessor.ProcessMouseDown(e);
        base.OnMouseDown(e);
    }

    internal void SKElementMouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.Button());
        UserInputProcessor.ProcessMouseUp(e);
        base.OnMouseUp(e);
    }

    internal void SKElementMouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
        UserInputProcessor.ProcessMouseMove(e);
        base.OnMouseMove(e);
    }

    internal void SKElementDoubleClick(object? sender, EventArgs e)
    {
        Interaction.DoubleClick();
        base.OnDoubleClick(e);
    }

    internal void SKElementMouseWheel(object? sender, MouseEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta);
        UserInputProcessor.ProcessMouseWheel(e);
        base.OnMouseWheel(e);
    }

    internal void SKElementKeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        UserInputProcessor.ProcessKeyDown(e);
        base.OnKeyDown(e);
    }

    internal void SKElementKeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        UserInputProcessor.ProcessKeyUp(e);
        base.OnKeyUp(e);
    }

    public float DetectDisplayScale()
    {
        using Graphics gfx = CreateGraphics();
        const int defaultDpi = 96;

        return gfx.DpiX / defaultDpi;
    }
}
