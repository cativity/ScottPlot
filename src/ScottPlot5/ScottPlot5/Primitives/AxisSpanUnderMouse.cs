using ScottPlot.Plottables;

namespace ScottPlot;

public class AxisSpanUnderMouse
{
    public required AxisSpan Span;
    public required Coordinates MouseStart;
    public required CoordinateRange OriginalRange;
    public bool ResizeEdge1;
    public bool ResizeEdge2;

    public bool IsResizing => ResizeEdge1 || ResizeEdge2;

    public bool IsMoving => !IsResizing;

    public bool IsResizingVertically => IsResizing && Span is VerticalSpan;

    public bool IsResizingHorizontally => IsResizing && Span is HorizontalSpan;

    public void DragTo(Coordinates mouseNow) => Span.DragTo(this, mouseNow);
}
