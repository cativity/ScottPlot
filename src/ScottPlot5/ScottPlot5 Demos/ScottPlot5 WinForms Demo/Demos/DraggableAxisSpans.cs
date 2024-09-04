using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class DraggableAxisSpans : Form, IDemoWindow
{
    public string Title => "Draggable Axis Spans";

    public string Description => "Demonstrates how to create a mouse-interactive axis span that can be resized or dragged";

    private AxisSpanUnderMouse? _spanBeingDragged;

    public DraggableAxisSpans()
    {
        InitializeComponent();

        // place axis spans on the plot
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        VerticalSpan vs = formsPlot1.Plot.Add.VerticalSpan(.23, .78);
        vs.IsDraggable = true;
        vs.IsResizable = true;

        HorizontalSpan hs = formsPlot1.Plot.Add.HorizontalSpan(23, 42);
        hs.IsDraggable = true;
        hs.IsResizable = true;

        formsPlot1.Refresh();

        // use events for custom mouse interactivity
        formsPlot1.MouseDown += FormsPlot1MouseDown;
        formsPlot1.MouseUp += FormsPlot1MouseUp;
        formsPlot1.MouseMove += FormsPlot1MouseMove;
    }

    private void FormsPlot1MouseDown(object? sender, MouseEventArgs e)
    {
        if (GetSpanUnderMouse(e.X, e.Y) is AxisSpanUnderMouse thingUnderMouse)
        {
            _spanBeingDragged = thingUnderMouse;
            formsPlot1.Interaction.Disable(); // disable panning while dragging
        }
    }

    private void FormsPlot1MouseUp(object? sender, MouseEventArgs e)
    {
        _spanBeingDragged = null;
        formsPlot1.Interaction.Enable(); // enable panning
        formsPlot1.Refresh();
    }

    private void FormsPlot1MouseMove(object? sender, MouseEventArgs e)
    {
        if (_spanBeingDragged is not null)
        {
            // currently dragging something so update it
            Coordinates mouseNow = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
            _spanBeingDragged.DragTo(mouseNow);
            formsPlot1.Refresh();
        }
        else
        {
            // not dragging anything so just set the cursor based on what's under the mouse

            if (GetSpanUnderMouse(e.X, e.Y) is AxisSpanUnderMouse spanUnderMouse)
            {
                if (spanUnderMouse.IsResizingHorizontally)
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (spanUnderMouse.IsResizingVertically)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (spanUnderMouse.IsMoving)
                {
                    Cursor = Cursors.SizeAll;
                }
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
    }

    private AxisSpanUnderMouse? GetSpanUnderMouse(float x, float y)
    {
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(x, y, 10);

        return formsPlot1.Plot.GetPlottables<AxisSpan>().Reverse().Select(span => span.UnderMouse(rect)).OfType<AxisSpanUnderMouse>().FirstOrDefault();
    }
}
