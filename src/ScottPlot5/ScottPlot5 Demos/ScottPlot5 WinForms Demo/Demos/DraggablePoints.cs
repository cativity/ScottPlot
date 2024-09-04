using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggablePoints : Form, IDemoWindow
{
    public string Title => "Draggable Data Points";

    public string Description
        => "GUI events can be used to interact with data "
           + "drawn on the plot. This example shows how to achieve drag-and-drop behavior "
           + "for points of a scatter plot. Extra code may be added to limit how far points may be moved.";

    private readonly double[] _xs = Generate.RandomAscending(10);
    private readonly double[] _ys = Generate.RandomSample(10);
    private readonly Scatter _scatter;
    private int? _indexBeingDragged;

    public DraggablePoints()
    {
        InitializeComponent();

        _scatter = formsPlot1.Plot.Add.Scatter(_xs, _ys);
        _scatter.LineWidth = 2;
        _scatter.MarkerSize = 10;
        _scatter.Smooth = true;

        formsPlot1.MouseMove += FormsPlot1MouseMove;
        formsPlot1.MouseDown += FormsPlot1MouseDown;
        formsPlot1.MouseUp += FormsPlot1MouseUp;
    }

    private void FormsPlot1MouseDown(object? sender, MouseEventArgs e)
    {
        Pixel mousePixel = new Pixel(e.Location.X, e.Location.Y);
        Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
        DataPoint nearest = _scatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
        _indexBeingDragged = nearest.IsReal ? nearest.Index : null;

        if (_indexBeingDragged.HasValue)
        {
            formsPlot1.Interaction.Disable();
        }
    }

    private void FormsPlot1MouseUp(object? sender, MouseEventArgs e)
    {
        _indexBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1MouseMove(object? sender, MouseEventArgs e)
    {
        Pixel mousePixel = new Pixel(e.Location.X, e.Location.Y);
        Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
        DataPoint nearest = _scatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
        formsPlot1.Cursor = nearest.IsReal ? Cursors.Hand : Cursors.Arrow;

        if (_indexBeingDragged is int indexBeingDragged)
        {
            _xs[indexBeingDragged] = mouseLocation.X;
            _ys[indexBeingDragged] = mouseLocation.Y;
            formsPlot1.Refresh();
        }
    }
}
