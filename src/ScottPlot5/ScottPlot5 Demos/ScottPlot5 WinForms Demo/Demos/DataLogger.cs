using JetBrains.Annotations;
using ScottPlot.AxisPanels;
using ScottPlot.DataGenerators;
using Timer = System.Windows.Forms.Timer;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class DataLogger : Form, IDemoWindow
{
    public string Title => "Data Logger";

    public string Description => "Plots live streaming data as a growing line plot.";

    private readonly Timer _addNewDataTimer = new Timer { Interval = 10, Enabled = true };
    private readonly Timer _updatePlotTimer = new Timer { Interval = 50, Enabled = true };

    private readonly RandomWalker _walker1 = new RandomWalker(0, multiplier: 0.01);
    private readonly RandomWalker _walker2 = new RandomWalker(1, multiplier: 1000);

    public DataLogger()
    {
        InitializeComponent();

        // disable interactivity by default
        formsPlot1.Interaction.Disable();

        // create two loggers and add them to the plot
        ScottPlot.Plottables.DataLogger logger1 = formsPlot1.Plot.Add.DataLogger();
        ScottPlot.Plottables.DataLogger logger2 = formsPlot1.Plot.Add.DataLogger();

        // use the right axis (already there by default) for the first logger
        RightAxis axis1 = (RightAxis)formsPlot1.Plot.Axes.Right;
        logger1.Axes.YAxis = axis1;
        axis1.Color(logger1.Color);

        // create and add a secondary right axis to use for the other logger
        RightAxis axis2 = formsPlot1.Plot.Axes.AddRightAxis();
        logger2.Axes.YAxis = axis2;
        axis2.Color(logger2.Color);

        _addNewDataTimer.Tick += (_, _) =>
        {
            const int count = 5;
            logger1.Add(_walker1.Next(count));
            logger2.Add(_walker2.Next(count));
        };

        _updatePlotTimer.Tick += (_, _) =>
        {
            if (logger1.HasNewData || logger2.HasNewData)
            {
                formsPlot1.Refresh();
            }
        };

        // wire our buttons to change the view modes of each logger
        btnFull.Click += (_, _) =>
        {
            logger1.ViewFull();
            logger2.ViewFull();
        };

        btnJump.Click += (_, _) =>
        {
            logger1.ViewJump();
            logger2.ViewJump();
        };

        btnSlide.Click += (_, _) =>
        {
            logger1.ViewSlide();
            logger2.ViewSlide();
        };
    }
}
