using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;
using Rectangle = ScottPlot.Plottables.Rectangle;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class SelectPoints : Form, IDemoWindow
{
    private readonly Coordinates[] _dataPoints;
    private Coordinates _mouseDownCoordinates;
    private Coordinates _mouseNowCoordinates;

    private CoordinateRect MouseSlectionRect => new CoordinateRect(_mouseDownCoordinates, _mouseNowCoordinates);

    private bool _mouseIsDown;
    private readonly Rectangle _rectanglePlot;

    public string Title => "Select Data Points";

    public string Description => "Demonstrates how to use mouse events to draw a rectangle around data points to select them";

    public SelectPoints()
    {
        InitializeComponent();

        // add sample data to the plot and keep track of the points
        _dataPoints = Generate.RandomCoordinates(100);
        Scatter sp = formsPlot1.Plot.Add.Scatter(_dataPoints);
        sp.LineWidth = 0;

        // add a rectangle we can use as a selection indicator
        _rectanglePlot = formsPlot1.Plot.Add.Rectangle(0, 0, 0, 0);
        _rectanglePlot.FillStyle.Color = Colors.Red.WithAlpha(.2);

        // add events to trigger in response to mouse actions
        formsPlot1.MouseMove += FormsPlot1MouseMove;
        formsPlot1.MouseDown += FormsPlot1MouseDown;
        formsPlot1.MouseUp += FormsPlot1MouseUp;
    }

    private void FormsPlot1MouseDown(object? sender, MouseEventArgs e)
    {
        if (!checkBox1.Checked)
        {
            return;
        }

        _mouseIsDown = true;
        _rectanglePlot.IsVisible = true;
        _mouseDownCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
        formsPlot1.Interaction.Disable(); // disable the default click-drag-pan behavior
    }

    private void FormsPlot1MouseUp(object? sender, MouseEventArgs e)
    {
        if (!checkBox1.Checked)
        {
            return;
        }

        _mouseIsDown = false;
        _rectanglePlot.IsVisible = false;

        // clear old markers
        formsPlot1.Plot.Remove<Marker>();

        // identify selectedPoints
        IEnumerable<Coordinates> selectedPoints = _dataPoints.Where(MouseSlectionRect.Contains);

        // add markers to outline selected points
        foreach (Marker newMarker in selectedPoints.Select(selectedPoint => formsPlot1.Plot.Add.Marker(selectedPoint)))
        {
            newMarker.MarkerStyle.Shape = MarkerShape.OpenCircle;
            newMarker.MarkerStyle.Size = 10;
            newMarker.MarkerStyle.FillColor = Colors.Red.WithAlpha(.2);
            newMarker.MarkerStyle.LineColor = Colors.Red;
            newMarker.MarkerStyle.LineWidth = 1;
        }

        // reset the mouse positions
        _mouseDownCoordinates = Coordinates.NaN;
        _mouseNowCoordinates = Coordinates.NaN;

        // update the plot
        formsPlot1.Refresh();
        formsPlot1.Interaction.Enable(); // re-enable the default click-drag-pan behavior
    }

    private void FormsPlot1MouseMove(object? sender, MouseEventArgs e)
    {
        if (!_mouseIsDown || !checkBox1.Checked)
        {
            return;
        }

        _mouseNowCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
        _rectanglePlot.CoordinateRect = MouseSlectionRect;
        formsPlot1.Refresh();
    }
}
