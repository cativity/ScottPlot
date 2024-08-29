namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseDragPan(MouseButton button) : IUserActionResponse
{
    /// <summary>
    ///     Which mouse button to watch for click-drag events
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    private Pixel _mouseDownPixel;

    private MultiAxisLimits? _rememberedLimits;

    public ResponseInfo Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        // mouse down starts drag
        if (userInput is IMouseButtonAction mouseDownAction && mouseDownAction.Button == MouseButton && mouseDownAction.IsPressed)
        {
            _mouseDownPixel = mouseDownAction.Pixel;
            _rememberedLimits = new MultiAxisLimits(plot);

            return ResponseInfo.NoActionRequired;
        }

        // mouse up ends drag
        if (userInput is IMouseButtonAction mouseUpAction && mouseUpAction.Button == MouseButton && !mouseUpAction.IsPressed && _rememberedLimits is not null)
        {
            _rememberedLimits.Recall();
            _rememberedLimits = null;
            ApplyToPlot(plot, _mouseDownPixel, mouseUpAction.Pixel, keys);

            return ResponseInfo.Refresh;
        }

        // mouse move while dragging
        if (userInput is IMouseAction mouseAction && _rememberedLimits is not null)
        {
            // Ignore dragging if it's only a few pixels. This leaves room for
            // single- and double-click events that may drag by a few pixels accidentally.
            double dX = Math.Abs(mouseAction.Pixel.X - _mouseDownPixel.X);
            double dY = Math.Abs(mouseAction.Pixel.Y - _mouseDownPixel.Y);
            double maxDragDistance = Math.Max(dX, dY);

            if (maxDragDistance < 5)
            {
                return ResponseInfo.NoActionRequired;
            }

            _rememberedLimits.Recall();
            ApplyToPlot(plot, _mouseDownPixel, mouseAction.Pixel, keys);

            return new ResponseInfo { RefreshNeeded = true, IsPrimary = true };
        }

        return ResponseInfo.NoActionRequired;
    }

    private static void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyboardState keys)
    {
        if (keys.IsPressed(StandardKeys.Shift))
        {
            px2.X = px1.X;
        }

        if (keys.IsPressed(StandardKeys.Control))
        {
            px2.Y = px1.Y;
        }

        MouseAxisManipulation.DragPan(plot, px1, px2);
    }
}
