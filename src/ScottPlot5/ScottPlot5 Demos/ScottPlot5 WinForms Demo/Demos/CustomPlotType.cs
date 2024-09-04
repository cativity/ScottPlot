using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Colormaps;
using SkiaSharp;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class CustomPlotType : Form, IDemoWindow
{
    public string Title => "Custom Plot Type";

    public string Description
        => "How to create a custom plot type that implements IPlottable to achieve full customization over how data is rendered on a plot.";

    public CustomPlotType()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(20);
        double[] ys = Generate.Sin(20);
        RainbowPlot rainbowPlot = new RainbowPlot(xs, ys);

        formsPlot1.Plot.Add.Plottable(rainbowPlot);
    }
}

internal class RainbowPlot(double[] xs, double[] ys) : IPlottable
{
    // data and customization options
    private double[] Xs { get; } = xs;

    private double[] Ys { get; } = ys;

    public float Radius { get; set; } = 10;

    private IColormap Colormap { get; set; } = new Turbo();

    // items required by IPlottable
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => new AxisLimits(Xs.Min(), Xs.Max(), Ys.Min(), Ys.Max());

    public void Render(RenderPack rp)
    {
        FillStyle fillStyle = new FillStyle();
        using SKPaint paint = new SKPaint();

        for (int i = 0; i < Xs.Length; i++)
        {
            Coordinates centerCoordinates = new Coordinates(Xs[i], Ys[i]);
            Pixel centerPixel = Axes.GetPixel(centerCoordinates);
            fillStyle.Color = Colormap.GetColor(i / (Xs.Length - 1.0));
            Drawing.DrawCircle(rp.Canvas, centerPixel, Radius, fillStyle, paint);
        }
    }
}
