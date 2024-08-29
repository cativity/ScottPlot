using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggableAxisLines : Form, IDemoWindow
{
    public string Title => "Draggable Axis Lines";

    public string Description => "Demonstrates how to add mouse interactivity to plotted objects";

    private AxisLine? _plottableBeingDragged;

    public DraggableAxisLines()
    {
        InitializeComponent();

        // place axis lines on the plot
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        VerticalLine vl = formsPlot1.Plot.Add.VerticalLine(23);
        vl.IsDraggable = true;
        vl.Text = "VLine";

        HorizontalLine hl = formsPlot1.Plot.Add.HorizontalLine(0.42);
        hl.IsDraggable = true;
        hl.Text = "HLine";

        formsPlot1.Refresh();

        // use events for custom mouse interactivity
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        if (GetLineUnderMouse(e.X, e.Y) is AxisLine lineUnderMouse)
        {
            _plottableBeingDragged = lineUnderMouse;
            formsPlot1.Interaction.Disable(); // disable panning while dragging
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        _plottableBeingDragged = null;
        formsPlot1.Interaction.Enable(); // enable panning again
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y, 10);

        if (_plottableBeingDragged is null)
        {
            // set cursor based on what's beneath the plottable

            if (GetLineUnderMouse(e.X, e.Y) is AxisLine lineUnderMouse)
            {
                if (lineUnderMouse.IsDraggable)
                {
                    Cursor = lineUnderMouse switch
                    {
                        VerticalLine => Cursors.SizeWE,
                        HorizontalLine => Cursors.SizeNS,
                        _ => Cursor
                    };
                }
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
        else
        {
            // update the position of the plottable being dragged
            if (_plottableBeingDragged is HorizontalLine hl)
            {
                hl.Y = rect.VerticalCenter;
                hl.Text = $"{hl.Y:0.00}";
            }
            else if (_plottableBeingDragged is VerticalLine vl)
            {
                vl.X = rect.HorizontalCenter;
                vl.Text = $"{vl.X:0.00}";
            }

            formsPlot1.Refresh();
        }
    }

    private AxisLine? GetLineUnderMouse(float x, float y)
    {
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(x, y, 10);

        return formsPlot1.Plot.GetPlottables<AxisLine>().Reverse().FirstOrDefault(axLine => axLine.IsUnderMouse(rect));
    }
}
