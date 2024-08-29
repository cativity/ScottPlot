using ScottPlot;
using ScottPlot.AxisRules;
using ScottPlot.Plottables;
using Rectangle = ScottPlot.Plottables.Rectangle;

namespace WinForms_Demo.Demos;

public partial class AxisRules : Form, IDemoWindow
{
    public string Title => "Axis Rules";

    public string Description => "Configure rules that limit how far the user can zoom in or out or enforce equal axis scaling";

    public AxisRules()
    {
        InitializeComponent();
        UnlockButtons();
        cbInvertX.CheckedChanged += (_, _) => btnReset_Click(this, EventArgs.Empty);
        cbInvertY.CheckedChanged += (_, _) => btnReset_Click(this, EventArgs.Empty);

        btnReset_Click(this, EventArgs.Empty);
    }

    private void LockButtons()
    {
        groupBox1.Enabled = false;
        groupBox2.Enabled = false;
        groupBox3.Enabled = false;
        groupBox4.Enabled = false;
        btnReset.Enabled = true;
    }

    private void UnlockButtons()
    {
        groupBox1.Enabled = true;
        groupBox2.Enabled = true;
        groupBox3.Enabled = true;
        groupBox4.Enabled = true;
        btnReset.Enabled = false;
    }

    private void btnBoundaryMin_Click(object sender, EventArgs e)
    {
        MinimumBoundary rule = new MinimumBoundary(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left, new AxisLimits(0, 1, 0, 1));

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Area inside the boundary is always in view");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnBoundaryMax_Click(object sender, EventArgs e)
    {
        MaximumBoundary rule = new MaximumBoundary(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left, new AxisLimits(0, 1, 0, 1));

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Cannot view area outside the boundary");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnScalePreserveX_Click(object sender, EventArgs e)
    {
        SquarePreserveX rule = new SquarePreserveX(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Automatically adjust Y so coordinates are square");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnScalePreserveY_Click(object sender, EventArgs e)
    {
        SquarePreserveY rule = new SquarePreserveY(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Automatically adjust X so coordinates are square");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnScaleZoom_Click(object sender, EventArgs e)
    {
        SquareZoomOut rule = new SquareZoomOut(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Automatically adjust the most zoomed-in axis so coordinates are square");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnSpanMin_Click(object sender, EventArgs e)
    {
        MinimumSpan rule = new MinimumSpan(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left, 1, 1);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Cannot zoom in beyond an axis span of 1");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnSpanMax_Click(object sender, EventArgs e)
    {
        MaximumSpan rule = new MaximumSpan(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left, 1, 1);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Cannot zoom out beyond an axis span of 1");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnLockHorizontal_Click(object sender, EventArgs e)
    {
        AxisLimits limits = formsPlot1.Plot.Axes.GetLimits();
        LockedHorizontal rule = new LockedHorizontal(formsPlot1.Plot.Axes.Bottom, limits.Left, limits.Right);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Horizontal Axis is Locked");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnLockVertical_Click(object sender, EventArgs e)
    {
        AxisLimits limits = formsPlot1.Plot.Axes.GetLimits();
        LockedVertical rule = new LockedVertical(formsPlot1.Plot.Axes.Left, limits.Bottom, limits.Top);

        formsPlot1.Plot.Axes.Rules.Clear();
        formsPlot1.Plot.Axes.Rules.Add(rule);
        formsPlot1.Plot.Title("Vertical Axis is Locked");
        formsPlot1.Refresh();
        LockButtons();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Axes.Rules.Clear();
        PlotRandomData();
        formsPlot1.Plot.Axes.AutoScale(cbInvertX.Checked, cbInvertY.Checked);
        formsPlot1.Plot.Title("No axis rules are in effect");
        formsPlot1.Refresh();
        UnlockButtons();
    }

    private void PlotRandomData()
    {
        formsPlot1.Plot.Clear();

        // generate data that fits between (0, 0) and (1, 1)
        const int pointCount = 500;
        double[] xs = Generate.Consecutive(pointCount, 1.0 / pointCount);
        double[] ys = Generate.Sin(pointCount, oscillations: 0.37);
        Generate.AddNoiseInPlace(ys, 0.1);

        Scatter sp = formsPlot1.Plot.Add.Scatter(xs, ys);
        sp.LineWidth = 0;
        sp.MarkerStyle.Size = 5;
        sp.Color = Colors.Magenta;

        Rectangle rect = formsPlot1.Plot.Add.Rectangle(0, 1, 0, 1);
        rect.FillStyle.Color = Colors.Transparent;
        rect.LineStyle.Color = Colors.Green;
        rect.LineStyle.Width = 3;
        rect.LineStyle.IsVisible = true;
    }
}
