using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class SignalXYDrag : Form, IDemoWindow
{
    public string Title => "Mouse Interactive SignalXY Plots";

    public string Description
        => "Demonstrates how to create SignalXY plots "
           + "which can be dragged with the mouse, and also how to display informatoin "
           + "about which point is nearest the cursor.";

    private SignalXY? _plottableBeingDragged;
    private DataPoint _startingDragPosition = DataPoint.None;
    private double _startingDragOffset;
    private readonly Marker _highlightedPointMarker;

    public SignalXYDrag()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(100);
        double[] ys1 = Generate.Sin(100);
        double[] ys2 = Generate.Cos(100);

        formsPlot1.Plot.Add.SignalXY(xs, ys1);
        formsPlot1.Plot.Add.SignalXY(xs, ys2);

        _highlightedPointMarker = formsPlot1.Plot.Add.Marker(0, 0);
        _highlightedPointMarker.IsVisible = false;
        _highlightedPointMarker.Size = 15;
        _highlightedPointMarker.LineWidth = 2;
        _highlightedPointMarker.Shape = MarkerShape.OpenCircle;

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        (SignalXY? sigXY, DataPoint dataPoint) = GetSignalXYUnderMouse(formsPlot1.Plot, e.X, e.Y);

        if (sigXY is null)
        {
            return;
        }

        _plottableBeingDragged = sigXY;
        _startingDragPosition = dataPoint;
        _startingDragOffset = sigXY.Data.XOffset;
        formsPlot1.Interaction.Disable(); // disable panning while dragging
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        _plottableBeingDragged = null;
        _startingDragPosition = DataPoint.None;
        formsPlot1.Interaction.Enable(); // enable panning again
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y, 5);

        // update the cursor to reflect what is beneath it
        if (_plottableBeingDragged is null)
        {
            (SignalXY? signalUnderMouse, DataPoint dp) = GetSignalXYUnderMouse(formsPlot1.Plot, e.X, e.Y);
            Cursor = signalUnderMouse is null ? Cursors.Arrow : Cursors.SizeWE;
            _highlightedPointMarker.IsVisible = signalUnderMouse is not null;

            if (signalUnderMouse is not null)
            {
                _highlightedPointMarker.Location = dp.Coordinates;
                _highlightedPointMarker.Color = signalUnderMouse.Color;
                Text = $"Index {dp.Index} at {dp.Coordinates}";
                formsPlot1.Refresh();
            }

            return;
        }

        // update the position of the plottable being dragged
        if (_plottableBeingDragged is SignalXY sigXY)
        {
            _highlightedPointMarker.IsVisible = false;
            sigXY.Data.XOffset = rect.HorizontalCenter - _startingDragPosition.X + _startingDragOffset;
            formsPlot1.Refresh();
        }
    }

    /// <summary>
    ///     Returns the SignalXY object and data point beneath the mouse,
    ///     or null if nothing is beneath the mouse.
    /// </summary>
    private static (SignalXY? signalXY, DataPoint point) GetSignalXYUnderMouse(Plot plot, double x, double y)
    {
        Pixel mousePixel = new Pixel(x, y);

        Coordinates mouseLocation = plot.GetCoordinates(mousePixel);

        foreach (SignalXY signal in plot.GetPlottables<SignalXY>().Reverse())
        {
            DataPoint nearest = signal.Data.GetNearest(mouseLocation, plot.LastRender);

            if (nearest.IsReal)
            {
                return (signal, nearest);
            }
        }

        return (null, DataPoint.None);
    }
}
