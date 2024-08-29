using ScottPlot.DataSources;
using ScottPlot.Plottables;

namespace ScottPlot;

/// <summary>
///     This class extends Plot.Add.* to add additional plottables provided by this NuGet package
/// </summary>
public static class AddPlottableExtensions
{
    /// <summary>
    ///     Add an OpenGL-accelerated scatter plot
    /// </summary>
    public static ScatterGL ScatterGL(this PlottableAdder add, IPlotControl control, double[] xs, double[] ys)
    {
        ScatterSourceDoubleArray source = new ScatterSourceDoubleArray(xs, ys);
        IScatterSource sourceWithCaching = new CacheScatterLimitsDecorator(source);
        ScatterGL sp = new ScatterGL(sourceWithCaching, control);
        Color nextColor = add.GetNextColor();
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.FillColor = nextColor;
        add.Plottable(sp);

        return sp;
    }

    /// <summary>
    ///     Add an OpenGL-accelerated scatter plot with customizable line width
    /// </summary>
    public static ScatterGLCustom ScatterGLCustom(this PlottableAdder add, IPlotControl control, double[] xs, double[] ys)
    {
        ScatterSourceDoubleArray data = new ScatterSourceDoubleArray(xs, ys);
        ScatterGLCustom sp = new ScatterGLCustom(data, control);
        Color nextColor = add.GetNextColor();
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.FillColor = nextColor;
        add.Plottable(sp);

        return sp;
    }
}
