namespace ScottPlot.Control;

/// <summary>
///     This class defines which buttons and keys perform which actions to manipulate the plot.
/// </summary>
public class InputBindings
{
    public MouseButton? DragPanButton;
    public MouseButton? DragZoomButton;
    public MouseButton? DragZoomRectangleButton;
    public MouseButton? DoubleClickButton;
    public MouseButton? ClickAutoAxisButton;
    public MouseButton? ClickContextMenuButton;

    public MouseWheelDirection? ZoomInWheelDirection;
    public MouseWheelDirection? ZoomOutWheelDirection;
    public MouseWheelDirection? PanUpWheelDirection;
    public MouseWheelDirection? PanDownWheelDirection;
    public MouseWheelDirection? PanLeftWheelDirection;
    public MouseWheelDirection? PanRightWheelDirection;

    public Key? LockHorizontalAxisKey;
    public Key? LockVerticalAxisKey;
    public Key? PanZoomRectangleKey;

    /// <summary>
    ///     Returns <see langword="true" /> if <paramref name="keys" /> contains the key which locks the X axis
    /// </summary>
    public virtual bool ShouldLockX(IEnumerable<Key> keys, MouseButton? button = null)
        => LockHorizontalAxisKey.HasValue && keys.Contains(LockHorizontalAxisKey.Value);

    /// <summary>
    ///     Returns <see langword="true" /> if <paramref name="keys" /> contains the key which locks the Y axis
    /// </summary>
    public virtual bool ShouldLockY(IEnumerable<Key> keys, MouseButton? button = null)
        => LockVerticalAxisKey.HasValue && keys.Contains(LockVerticalAxisKey.Value);

    /// <summary>
    ///     Returns <see langword="true" /> if the combination of pressed buttons and keys results in a click-drag zoom
    ///     rectangle
    /// </summary>
    public virtual bool ShouldZoomRectangle(MouseButton button, IEnumerable<Key> keys)
    {
        if (button == DragZoomRectangleButton)
        {
            return true;
        }

        if (button == DragPanButton)
        {
            if (PanZoomRectangleKey.HasValue && keys.Contains(PanZoomRectangleKey.Value))
            {
                return true;
            }
        }

        return false;
    }

    public static InputBindings Standard()
        => new InputBindings
        {
            DragPanButton = MouseButton.Left,
            DragZoomRectangleButton = MouseButton.Middle,
            DragZoomButton = MouseButton.Right,
            ClickAutoAxisButton = MouseButton.Middle,
            ClickContextMenuButton = MouseButton.Right,
            DoubleClickButton = MouseButton.Left,
            ZoomInWheelDirection = MouseWheelDirection.Up,
            ZoomOutWheelDirection = MouseWheelDirection.Down,
            LockHorizontalAxisKey = Key.Shift,
            LockVerticalAxisKey = Key.Ctrl,
            PanZoomRectangleKey = Key.Alt,
        };

    public static InputBindings NonInteractive() => new InputBindings();
}
