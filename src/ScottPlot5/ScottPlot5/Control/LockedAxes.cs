namespace ScottPlot.Control;

public struct LockedAxes(bool lockedX, bool lockedY)
{
    public bool X { get; set; } = lockedX;

    public bool Y { get; set; } = lockedY;
}
