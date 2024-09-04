using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class MultiAxis : Form, IDemoWindow
{
    public string Title => "Multi-Axis";

    public string Description => "Display data which visually overlaps but is plotted on different axes";

    private readonly IYAxis _yAxis1;

    private readonly IYAxis _yAxis2;

    public MultiAxis()
    {
        InitializeComponent();

        // Store the primary Y axis so we can refer to it later
        _yAxis1 = formsPlot1.Plot.Axes.Left;

        // Create a second Y axis, add it to the plot, and save it for later
        _yAxis2 = formsPlot1.Plot.Axes.AddLeftAxis();

        // setup button actions
        btnRandomize.Click += (_, _) => PlotRandomData();

        // plot random data to start
        PlotRandomData();
    }

    private void PlotRandomData()
    {
        formsPlot1.Plot.Clear();

        double[] data1 = RandomDataGenerator.Generate.RandomWalk(51, 1);
        Signal sig1 = formsPlot1.Plot.Add.Signal(data1);
        sig1.Axes.YAxis = _yAxis1;
        _yAxis1.Label.Text = "YAxis1";
        _yAxis1.Label.ForeColor = sig1.Color;

        double[] data2 = RandomDataGenerator.Generate.RandomWalk(51, 1000);
        Signal sig2 = formsPlot1.Plot.Add.Signal(data2);
        sig2.Axes.YAxis = _yAxis2;
        _yAxis2.Label.Text = "YAxis2";
        _yAxis2.Label.ForeColor = sig2.Color;

        formsPlot1.Plot.Axes.AutoScale();
        formsPlot1.Plot.Axes.Zoom(.8, .7); // zoom out slightly
        formsPlot1.Refresh();
    }

    private void BtnRandomizeClick(object sender, EventArgs e)
    {
        PlotRandomData();
    }

    private void BtnManualScaleClick(object sender, EventArgs e)
    {
        formsPlot1.Plot.Axes.SetLimits(0, 50, -20, 20, formsPlot1.Plot.Axes.Bottom, _yAxis1);
        formsPlot1.Plot.Axes.SetLimits(0, 50, -20_000, 20_000, formsPlot1.Plot.Axes.Bottom, _yAxis2);
        formsPlot1.Refresh();
    }

    private void BtnAutoScaleClick(object sender, EventArgs e)
    {
        formsPlot1.Plot.Axes.Margins();
        formsPlot1.Plot.Axes.AutoScale();
        formsPlot1.Refresh();
    }

    private void BtnAutoScaleTightClick(object sender, EventArgs e)
    {
        formsPlot1.Plot.Axes.Margins(0, 0);
        formsPlot1.Refresh();
    }

    private void BtnAutoScaleWithPaddingClick(object sender, EventArgs e)
    {
        formsPlot1.Plot.Axes.Margins(1, 1);
        formsPlot1.Refresh();
    }
}
