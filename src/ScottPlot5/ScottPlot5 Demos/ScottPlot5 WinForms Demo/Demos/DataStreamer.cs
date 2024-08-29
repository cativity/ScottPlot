using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.DataGenerators;
using ScottPlot.DataViews;
using Timer = System.Windows.Forms.Timer;

namespace WinForms_Demo.Demos;

public partial class DataStreamer : Form, IDemoWindow
{
    public string Title => "Data Streamer";

    public string Description => "Plots live streaming data as a fixed-width line plot, shifting old data out as new data comes in.";

    private readonly Timer _addNewDataTimer = new Timer { Interval = 10, Enabled = true };
    private readonly Timer _updatePlotTimer = new Timer { Interval = 50, Enabled = true };

    private readonly RandomWalker _walker1 = new RandomWalker(0);
    private readonly RandomWalker _walker2 = new RandomWalker(1);

    public DataStreamer()
    {
        InitializeComponent();

        ScottPlot.Plottables.DataStreamer streamer1 = formsPlot1.Plot.Add.DataStreamer(1000);
        ScottPlot.Plottables.DataStreamer streamer2 = formsPlot1.Plot.Add.DataStreamer(1000);
        VerticalLine vLine = formsPlot1.Plot.Add.VerticalLine(0, 2, Colors.Red);

        // disable mouse interaction by default
        formsPlot1.Interaction.Disable();

        // only show marker button in scroll mode
        btnMark.Visible = false;

        // setup a timer to add data to the streamer periodically
        _addNewDataTimer.Tick += (_, _) =>
        {
            const int count = 5;

            // add new sample data
            streamer1.AddRange(_walker1.Next(count));
            streamer2.AddRange(_walker2.Next(count));

            // slide marker to the left
            formsPlot1.Plot.GetPlottables<Marker>().ToList().ForEach(m => m.X -= count);

            // remove off-screen marks
            formsPlot1.Plot.GetPlottables<Marker>().Where(m => m.X < 0).ToList().ForEach(m => formsPlot1.Plot.Remove(m));
        };

        // setup a timer to request a render periodically
        _updatePlotTimer.Tick += (_, _) =>
        {
            if (streamer1.HasNewData)
            {
                formsPlot1.Plot.Title($"Processed {streamer1.Data.CountTotal:N0} points");
                vLine.IsVisible = streamer1.Renderer is Wipe;
                vLine.Position = (streamer1.Data.NextIndex * streamer1.Data.SamplePeriod) + streamer1.Data.OffsetX;
                formsPlot1.Refresh();
            }
        };

        // setup configuration actions
        btnWipeRight.Click += (_, _) =>
        {
            streamer1.ViewWipeRight(0.1);
            streamer2.ViewWipeRight(0.1);
            btnMark.Visible = false;
            formsPlot1.Plot.Remove<Marker>();
        };

        btnScrollLeft.Click += (_, _) =>
        {
            streamer1.ViewScrollLeft();
            streamer2.ViewScrollLeft();
            btnMark.Visible = true;
        };

        btnMark.Click += (_, _) =>
        {
            double x1 = streamer1.Count;
            double y1 = streamer1.Data.NewestPoint;
            Marker marker1 = formsPlot1.Plot.Add.Marker(x1, y1);
            marker1.Size = 20;
            marker1.Shape = MarkerShape.OpenCircle;
            marker1.Color = streamer1.LineColor;
            marker1.LineWidth = 2;

            double x2 = streamer2.Count;
            double y2 = streamer2.Data.NewestPoint;
            Marker marker2 = formsPlot1.Plot.Add.Marker(x2, y2);
            marker2.Size = 20;
            marker2.Shape = MarkerShape.OpenCircle;
            marker2.Color = streamer2.LineColor;
            marker2.LineWidth = 2;
        };

        rbManage.CheckedChanged += (s, _) =>
        {
            if ((s as RadioButton)?.Checked == true)
            {
                formsPlot1.Plot.Axes.ContinuouslyAutoscale = false;
                streamer1.ManageAxisLimits = true;
                streamer2.ManageAxisLimits = true;
            }
            else
            {
                formsPlot1.Plot.Axes.ContinuouslyAutoscale = true;
                streamer1.ManageAxisLimits = false;
                streamer2.ManageAxisLimits = false;
            }
        };
    }
}
